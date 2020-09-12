using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using MSLibrary.DI;
using MSLibrary.Serializer;
using MSLibrary.LanguageTranslate;
using MSLibrary.Thread;
using System.Linq;

namespace MSLibrary.Cache.RealKVCacheVisitServices
{
    /// <summary>
    /// 基于本地版本号控制的KV缓存访问服务
    /// cacheConfiguration的格式为
    /// {
    ///     "MaxLength":最大缓存长度,
    ///     "VersionCallTimeout":版本服务调用间隔（单位秒）,
    ///     "VersionNameMappings":
    ///         {
    ///             "{Key的类型的FullName}-{Value的类型的FullName}":"版本名称"
    ///         }
    ///     "DefaultVersionName":"默认版本名称,找不到KV类型与版本名称的映射时使用该名称"
    /// }
    /// </summary>
    [Injection(InterfaceType = typeof(RealKVCacheVisitServiceForLocalVersion), Scope = InjectionScope.Singleton)]
    public class RealKVCacheVisitServiceForLocalVersion : IRealKVCacheVisitService
    {
        private static SemaphoreSlim _lock = new SemaphoreSlim(1, 1);

        private static Dictionary<string, CacheContainer> _datas = new Dictionary<string, CacheContainer>();

        private static Dictionary<string, IFactory<IKVCacheVersionService>> _kvCacheVersionServiceFactories = new Dictionary<string, IFactory<IKVCacheVersionService>>();

        /// <summary>
        /// KV缓存版本服务工厂键值对
        /// 键为版本名称
        /// </summary>
        public static IDictionary<string, IFactory<IKVCacheVersionService>> KVCacheVersionServiceFactories
        {
            get
            {
                return _kvCacheVersionServiceFactories;
            }
        }

        public async Task Clear<K, V>(string cacheConfiguration, string prefix, K key)
        {
            ClearSync<K, V>(cacheConfiguration, prefix, key);
            await Task.CompletedTask;
        }

        public async Task Clear<K, V>(string cacheConfiguration, string prefix, IEnumerable<K> keys)
        {
            ClearSync<K, V>(cacheConfiguration, prefix, keys);
            await Task.CompletedTask;
        }

        public void ClearSync<K, V>(string cacheConfiguration, string prefix, K key)
        {
            var versionMappingKey = $"{typeof(K).FullName}-{typeof(V).FullName}";
            var configuration = JsonSerializerHelper.Deserialize<KVCacheConfiguration>(cacheConfiguration);

            if (_datas.TryGetValue(prefix, out CacheContainer cacheContainer))
            {
                cacheContainer.CacheDict.Remove(key);
            }
        }

        public void ClearSync<K, V>(string cacheConfiguration, string prefix, IEnumerable<K> keys)
        {
            var versionMappingKey = $"{typeof(K).FullName}-{typeof(V).FullName}";
            var configuration = JsonSerializerHelper.Deserialize<KVCacheConfiguration>(cacheConfiguration);

            if (_datas.TryGetValue(prefix, out CacheContainer cacheContainer))
            {
                foreach (var item in keys)
                {
                    cacheContainer.CacheDict.Remove(item);
                }
            }

        }

        public async Task<(V, bool)> Get<K, V>(string cacheConfiguration, Func<Task<(V, bool)>> creator, string prefix, K key)
        {
            var versionMappingKey = $"{typeof(K).FullName}-{typeof(V).FullName}";
            var configuration = JsonSerializerHelper.Deserialize<KVCacheConfiguration>(cacheConfiguration);
            if (!_datas.TryGetValue(prefix, out CacheContainer cacheContainer))
            {
                await _lock.WaitAsync();
                try
                {
                    if (!_datas.TryGetValue(prefix, out cacheContainer))
                    {
                        var (versionService, versionname) = getVersionService(configuration, versionMappingKey);
                        var version = await versionService.GetVersion(versionname);
                        cacheContainer = new CacheContainer() { Version = version, LatestVersionTime = DateTime.UtcNow, CacheDict = new HashLinkedCache<object, CacheValueContainer>() { Length = configuration.MaxLength } };
                        _datas[prefix] = cacheContainer;
                    }
                }
                finally
                {
                    _lock.Release();
                }
            }
            else
            {
                if ((DateTime.UtcNow - cacheContainer.LatestVersionTime).TotalSeconds > configuration.VersionCallTimeout)
                {
                    await _lock.WaitAsync();
                    try
                    {
                        if ((DateTime.UtcNow - cacheContainer.LatestVersionTime).TotalSeconds > configuration.VersionCallTimeout)
                        {
                            var (versionService, versionname) = getVersionService(configuration, versionMappingKey);
                            var version = await versionService.GetVersion(versionname);
                            if (version != cacheContainer.Version)
                            {
                                cacheContainer = new CacheContainer() { Version = version, LatestVersionTime = DateTime.UtcNow, CacheDict = new HashLinkedCache<object, CacheValueContainer>() { Length = configuration.MaxLength } };
                                _datas[prefix] = cacheContainer;
                            }
                            else
                            {
                                cacheContainer.LatestVersionTime = DateTime.UtcNow;
                            }
                        }
                    }
                    finally
                    {
                        _lock.Release();
                    }

                }
            }


            bool isCache = true;
            var valueItem = cacheContainer.CacheDict.GetValue(key);
            if (valueItem == null)
            {
                await cacheContainer.SyncOperate(
                async () =>
                {
                    valueItem = cacheContainer.CacheDict.GetValue(key);
                    if (valueItem == null)
                    {
                        var (cacheValue, isCache) = await creator();
                        valueItem = new CacheValueContainer() { Type=0, Value = cacheValue };
                        if (isCache)
                        {
                            cacheContainer.CacheDict.SetValue(key, valueItem);
                        }

                    }
                }
                );

            }

            if (valueItem.Type!=0)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.KVCacheValueContainerTypeError,
                    DefaultFormatting = "键为{0}的KV缓存值容器类型不正确，需要的类型为{1}，当前类型为{2}，发生位置为{3}",
                    ReplaceParameters = new List<object>() { key.ToString(),"0","1", $"{this.GetType().FullName}.Get<K, V>" }
                };

                throw new UtilityException((int)Errors.KVCacheValueContainerTypeError, fragment);
            }

            return ((V)valueItem.Value, isCache);

        }

        public async Task<(V, bool)> GetHash<K, V>(string cacheConfiguration,  Func<Task<(V, bool)>> creator,string prefix, K key, string hashKey)
        {
            var versionMappingKey = $"{typeof(K).FullName}-{typeof(V).FullName}";
            var configuration = JsonSerializerHelper.Deserialize<KVCacheConfiguration>(cacheConfiguration);
            if (!_datas.TryGetValue(prefix, out CacheContainer cacheContainer))
            {
                await _lock.WaitAsync();
                try
                {
                    if (!_datas.TryGetValue(prefix, out cacheContainer))
                    {
                        var (versionService, versionname) = getVersionService(configuration, versionMappingKey);
                        var version = await versionService.GetVersion(versionname);
                        cacheContainer = new CacheContainer() { Version = version, LatestVersionTime = DateTime.UtcNow, CacheDict = new HashLinkedCache<object, CacheValueContainer>() { Length = configuration.MaxLength } };
                        _datas[prefix] = cacheContainer;
                    }
                }
                finally
                {
                    _lock.Release();
                }
            }
            else
            {
                if ((DateTime.UtcNow - cacheContainer.LatestVersionTime).TotalSeconds > configuration.VersionCallTimeout)
                {
                    await _lock.WaitAsync();
                    try
                    {
                        if ((DateTime.UtcNow - cacheContainer.LatestVersionTime).TotalSeconds > configuration.VersionCallTimeout)
                        {
                            var (versionService, versionname) = getVersionService(configuration, versionMappingKey);
                            var version = await versionService.GetVersion(versionname);
                            if (version != cacheContainer.Version)
                            {
                                cacheContainer = new CacheContainer() { Version = version, LatestVersionTime = DateTime.UtcNow, CacheDict = new HashLinkedCache<object, CacheValueContainer>() { Length = configuration.MaxLength } };
                                _datas[prefix] = cacheContainer;
                            }
                            else
                            {
                                cacheContainer.LatestVersionTime = DateTime.UtcNow;
                            }
                        }
                    }
                    finally
                    {
                        _lock.Release();
                    }

                }
            }


            bool isCache = true;
            var valueItem = cacheContainer.CacheDict.GetValue(key);
            if (valueItem == null)
            {
                await cacheContainer.SyncOperate(
                async () =>
                {
                    valueItem = cacheContainer.CacheDict.GetValue(key);
                    if (valueItem == null)
                    {
                        var (cacheValue, isCache) = await creator();
                        Dictionary<string, V> realValue = new Dictionary<string, V>();
                        if (isCache)
                        {
                            realValue[hashKey] = cacheValue;
                        }
                        valueItem = new CacheValueContainer() { Type = 1, Value = realValue };
                        cacheContainer.CacheDict.SetValue(key, valueItem);

                    }

                }
                );

            }

            if (valueItem.Type != 1)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.KVCacheValueContainerTypeError,
                    DefaultFormatting = "键为{0}的KV缓存值容器类型不正确，需要的类型为{1}，当前类型为{2}，发生位置为{3}",
                    ReplaceParameters = new List<object>() { key.ToString(), "1", valueItem.Type.ToString(), $"{this.GetType().FullName}.GetHash<K, V>" }
                };

                throw new UtilityException((int)Errors.KVCacheValueContainerTypeError, fragment);
            }

            var dictValue = (Dictionary<string, V>)valueItem.Value;

            if (!dictValue.TryGetValue(hashKey, out V realValue))
            {
                 (realValue, isCache) = await creator();
                if (isCache)
                {
                    dictValue[hashKey] = realValue;
                }
            }

            return (realValue, isCache);
        }

        public (V, bool) GetHashSync<K, V>(string cacheConfiguration, Func<(V, bool)> creator,string prefix,  K key, string hashKey)
        {
            var versionMappingKey = $"{typeof(K).FullName}-{typeof(V).FullName}";
            var configuration = JsonSerializerHelper.Deserialize<KVCacheConfiguration>(cacheConfiguration);
            if (!_datas.TryGetValue(prefix, out CacheContainer cacheContainer))
            {
                 _lock.Wait();
                try
                {
                    if (!_datas.TryGetValue(prefix, out cacheContainer))
                    {
                        var (versionService, versionname) = getVersionService(configuration, versionMappingKey);
                        var version =  versionService.GetVersionSync(versionname);
                        cacheContainer = new CacheContainer() { Version = version, LatestVersionTime = DateTime.UtcNow, CacheDict = new HashLinkedCache<object, CacheValueContainer>() { Length = configuration.MaxLength } };
                        _datas[prefix] = cacheContainer;
                    }
                }
                finally
                {
                    _lock.Release();
                }
            }
            else
            {
                if ((DateTime.UtcNow - cacheContainer.LatestVersionTime).TotalSeconds > configuration.VersionCallTimeout)
                {
                    _lock.Wait();
                    try
                    {
                        if ((DateTime.UtcNow - cacheContainer.LatestVersionTime).TotalSeconds > configuration.VersionCallTimeout)
                        {
                            var (versionService, versionname) = getVersionService(configuration, versionMappingKey);
                            var version =  versionService.GetVersionSync(versionname);
                            if (version != cacheContainer.Version)
                            {
                                cacheContainer = new CacheContainer() { Version = version, LatestVersionTime = DateTime.UtcNow, CacheDict = new HashLinkedCache<object, CacheValueContainer>() { Length = configuration.MaxLength } };
                                _datas[prefix] = cacheContainer;
                            }
                            else
                            {
                                cacheContainer.LatestVersionTime = DateTime.UtcNow;
                            }
                        }
                    }
                    finally
                    {
                        _lock.Release();
                    }

                }
            }


            bool isCache = true;
            var valueItem = cacheContainer.CacheDict.GetValue(key);
            if (valueItem == null)
            {
                cacheContainer.SyncOperate(
                () =>
                {
                    valueItem = cacheContainer.CacheDict.GetValue(key);
                    if (valueItem == null)
                    {
                        var (cacheValue, isCache) =  creator();
                        Dictionary<string, V> realValue = new Dictionary<string, V>();
                        if (isCache)
                        {
                            realValue[hashKey] = cacheValue;
                        }
                        valueItem = new CacheValueContainer() { Type = 1, Value = realValue };
                        cacheContainer.CacheDict.SetValue(key, valueItem);

                    }

                }
                );

            }

            if (valueItem.Type != 1)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.KVCacheValueContainerTypeError,
                    DefaultFormatting = "键为{0}的KV缓存值容器类型不正确，需要的类型为{1}，当前类型为{2}，发生位置为{3}",
                    ReplaceParameters = new List<object>() { key.ToString(), "1", valueItem.Type.ToString(), $"{this.GetType().FullName}.GetHashSync<K, V>" }
                };

                throw new UtilityException((int)Errors.KVCacheValueContainerTypeError, fragment);
            }

            var dictValue = (Dictionary<string, V>)valueItem.Value;

            if (!dictValue.TryGetValue(hashKey, out V realValue))
            {
                (realValue, isCache) =  creator();
                if (isCache)
                {
                    dictValue[hashKey] = realValue;
                }
            }

            return (realValue, isCache);
        }

        public (V, bool) GetSync<K, V>(string cacheConfiguration, Func<(V, bool)> creator, string prefix, K key)
        {
            var versionMappingKey = $"{typeof(K).FullName}-{typeof(V).FullName}";
            var configuration = JsonSerializerHelper.Deserialize<KVCacheConfiguration>(cacheConfiguration);
            if (!_datas.TryGetValue(prefix, out CacheContainer cacheContainer))
            {
                _lock.Wait();
                try
                {
                    if (!_datas.TryGetValue(prefix, out cacheContainer))
                    {
                        var (versionService, versionname) = getVersionService(configuration, versionMappingKey);
                        var version = versionService.GetVersionSync(versionname);
                        cacheContainer = new CacheContainer() { Version = version, LatestVersionTime = DateTime.UtcNow, CacheDict = new HashLinkedCache<object, CacheValueContainer>() { Length = configuration.MaxLength } };
                        _datas[prefix] = cacheContainer;
                    }
                }
                finally
                {
                    _lock.Release();
                }
            }
            else
            {
                if ((DateTime.UtcNow - cacheContainer.LatestVersionTime).TotalSeconds > configuration.VersionCallTimeout)
                {
                    _lock.Wait();
                    try
                    {
                        if ((DateTime.UtcNow - cacheContainer.LatestVersionTime).TotalSeconds > configuration.VersionCallTimeout)
                        {
                            var (versionService, versionname) = getVersionService(configuration, versionMappingKey);
                            var version = versionService.GetVersionSync(versionname);
                            if (version != cacheContainer.Version)
                            {
                                cacheContainer = new CacheContainer() { Version = version, LatestVersionTime = DateTime.UtcNow, CacheDict = new HashLinkedCache<object, CacheValueContainer>() { Length = configuration.MaxLength } };
                                _datas[prefix] = cacheContainer;
                            }
                            else
                            {
                                cacheContainer.LatestVersionTime = DateTime.UtcNow;
                            }
                        }
                    }
                    finally
                    {
                        _lock.Release();
                    }

                }
            }


            bool isCache = true;
            var valueItem = cacheContainer.CacheDict.GetValue(key);
            if (valueItem == null)
            {
                cacheContainer.SyncOperate(
                () =>
               {
                   valueItem = cacheContainer.CacheDict.GetValue(key);
                   if (valueItem == null)
                   {
                       var (cacheValue, isCache) = creator();
                       valueItem = new CacheValueContainer() { Type=0, Value = cacheValue };
                       if (isCache)
                       {
                           cacheContainer.CacheDict.SetValue(key, valueItem);
                       }
                   }
               }
               );

            }

            if (valueItem.Type != 0)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.KVCacheValueContainerTypeError,
                    DefaultFormatting = "键为{0}的KV缓存值容器类型不正确，需要的类型为{1}，当前类型为{2}，发生位置为{3}",
                    ReplaceParameters = new List<object>() { key.ToString(), "0", "1", $"{this.GetType().FullName}.GetSync<K, V>" }
                };

                throw new UtilityException((int)Errors.KVCacheValueContainerTypeError, fragment);
            }

            return ((V)valueItem.Value, isCache);

        }

        public async Task Set<K, V>(string cacheConfiguration, string prefix, K key, V value)
        {
            var versionMappingKey = $"{typeof(K).FullName}-{typeof(V).FullName}";
            var configuration = JsonSerializerHelper.Deserialize<KVCacheConfiguration>(cacheConfiguration);

            if (!_datas.TryGetValue(prefix, out CacheContainer cacheContainer))
            {
                await _lock.WaitAsync();
                try
                {
                    if (!_datas.TryGetValue(prefix, out cacheContainer))
                    {
                        var (versionService, versionname) = getVersionService(configuration, versionMappingKey);
                        var version = await versionService.GetVersion(versionname);
                        cacheContainer = new CacheContainer() { Version = version, LatestVersionTime = DateTime.UtcNow, CacheDict = new HashLinkedCache<object, CacheValueContainer>() { Length = configuration.MaxLength } };
                        _datas[prefix] = cacheContainer;
                    }
                }
                finally
                {
                    _lock.Release();
                }
            }
            else
            {
                if ((DateTime.UtcNow - cacheContainer.LatestVersionTime).TotalSeconds > configuration.VersionCallTimeout)
                {
                    await _lock.WaitAsync();
                    try
                    {
                        if ((DateTime.UtcNow - cacheContainer.LatestVersionTime).TotalSeconds > configuration.VersionCallTimeout)
                        {
                            var (versionService, versionname) = getVersionService(configuration, versionMappingKey);
                            var version =await versionService.GetVersion(versionname);
                            if (version != cacheContainer.Version)
                            {
                                cacheContainer = new CacheContainer() { Version = version, LatestVersionTime = DateTime.UtcNow, CacheDict = new HashLinkedCache<object, CacheValueContainer>() { Length = configuration.MaxLength } };
                                _datas[prefix] = cacheContainer;
                            }
                            else
                            {
                                cacheContainer.LatestVersionTime = DateTime.UtcNow;
                            }
                        }
                    }
                    finally
                    {
                        _lock.Release();
                    }

                }
            }

            var valueItem = new CacheValueContainer() { Type=0, Value = value };
            cacheContainer.CacheDict.SetValue(key, valueItem);
        }

        public async Task SetHash<K, V>(string cacheConfiguration, string prefix, K key, IDictionary<string, V> values)
        {
            var versionMappingKey = $"{typeof(K).FullName}-{typeof(V).FullName}";
            var configuration = JsonSerializerHelper.Deserialize<KVCacheConfiguration>(cacheConfiguration);

            if (!_datas.TryGetValue(prefix, out CacheContainer cacheContainer))
            {
                await _lock.WaitAsync();
                try
                {
                    if (!_datas.TryGetValue(prefix, out cacheContainer))
                    {
                        var (versionService, versionname) = getVersionService(configuration, versionMappingKey);
                        var version = await versionService.GetVersion(versionname);
                        cacheContainer = new CacheContainer() { Version = version, LatestVersionTime = DateTime.UtcNow, CacheDict = new HashLinkedCache<object, CacheValueContainer>() { Length = configuration.MaxLength } };
                        _datas[prefix] = cacheContainer;
                    }
                }
                finally
                {
                    _lock.Release();
                }
            }
            else
            {
                if ((DateTime.UtcNow - cacheContainer.LatestVersionTime).TotalSeconds > configuration.VersionCallTimeout)
                {
                    await _lock.WaitAsync();
                    try
                    {
                        if ((DateTime.UtcNow - cacheContainer.LatestVersionTime).TotalSeconds > configuration.VersionCallTimeout)
                        {
                            var (versionService, versionname) = getVersionService(configuration, versionMappingKey);
                            var version = await versionService.GetVersion(versionname);
                            if (version != cacheContainer.Version)
                            {
                                cacheContainer = new CacheContainer() { Version = version, LatestVersionTime = DateTime.UtcNow, CacheDict = new HashLinkedCache<object, CacheValueContainer>() { Length = configuration.MaxLength } };
                                _datas[prefix] = cacheContainer;
                            }
                            else
                            {
                                cacheContainer.LatestVersionTime = DateTime.UtcNow;
                            }
                        }
                    }
                    finally
                    {
                        _lock.Release();
                    }

                }
            }


            var valueItem = new CacheValueContainer() { Type = 1, Value = values.ToDictionary((kv)=>kv.Key,(kv)=>kv.Value) };
            cacheContainer.CacheDict.SetValue(key, valueItem);
        }

        public void SetHashSync<K, V>(string cacheConfiguration, string prefix, K key, IDictionary<string, V> values)
        {
            var versionMappingKey = $"{typeof(K).FullName}-{typeof(V).FullName}";
            var configuration = JsonSerializerHelper.Deserialize<KVCacheConfiguration>(cacheConfiguration);

            if (!_datas.TryGetValue(prefix, out CacheContainer cacheContainer))
            {
                _lock.Wait();
                try
                {
                    if (!_datas.TryGetValue(prefix, out cacheContainer))
                    {
                        var (versionService, versionname) = getVersionService(configuration, versionMappingKey);
                        var version = versionService.GetVersionSync(versionname);
                        cacheContainer = new CacheContainer() { Version = version, LatestVersionTime = DateTime.UtcNow, CacheDict = new HashLinkedCache<object, CacheValueContainer>() { Length = configuration.MaxLength } };
                        _datas[prefix] = cacheContainer;
                    }
                }
                finally
                {
                    _lock.Release();
                }
            }
            else
            {
                if ((DateTime.UtcNow - cacheContainer.LatestVersionTime).TotalSeconds > configuration.VersionCallTimeout)
                {
                    _lock.Wait();
                    try
                    {
                        if ((DateTime.UtcNow - cacheContainer.LatestVersionTime).TotalSeconds > configuration.VersionCallTimeout)
                        {
                            var (versionService, versionname) = getVersionService(configuration, versionMappingKey);
                            var version = versionService.GetVersionSync(versionname);
                            if (version != cacheContainer.Version)
                            {
                                cacheContainer = new CacheContainer() { Version = version, LatestVersionTime = DateTime.UtcNow, CacheDict = new HashLinkedCache<object, CacheValueContainer>() { Length = configuration.MaxLength } };
                                _datas[prefix] = cacheContainer;
                            }
                            else
                            {
                                cacheContainer.LatestVersionTime = DateTime.UtcNow;
                            }
                        }
                    }
                    finally
                    {
                        _lock.Release();
                    }

                }
            }

            var valueItem = new CacheValueContainer() { Type = 1, Value = values.ToDictionary((kv) => kv.Key, (kv) => kv.Value) };
            cacheContainer.CacheDict.SetValue(key, valueItem);
        }

        public void SetSync<K, V>(string cacheConfiguration, string prefix, K key, V value)
        {
            var versionMappingKey = $"{typeof(K).FullName}-{typeof(V).FullName}";
            var configuration = JsonSerializerHelper.Deserialize<KVCacheConfiguration>(cacheConfiguration);

            if (!_datas.TryGetValue(prefix, out CacheContainer cacheContainer))
            {
                _lock.Wait();
                try
                {
                    if (!_datas.TryGetValue(prefix, out cacheContainer))
                    {
                        var (versionService, versionname) = getVersionService(configuration, versionMappingKey);
                        var version = versionService.GetVersionSync(versionname);
                        cacheContainer = new CacheContainer() { Version = version, LatestVersionTime = DateTime.UtcNow, CacheDict = new HashLinkedCache<object, CacheValueContainer>() { Length = configuration.MaxLength } };
                        _datas[prefix] = cacheContainer;
                    }
                }
                finally
                {
                    _lock.Release();
                }
            }
            else
            {
                if ((DateTime.UtcNow - cacheContainer.LatestVersionTime).TotalSeconds > configuration.VersionCallTimeout)
                {
                    _lock.Wait();
                    try
                    {
                        if ((DateTime.UtcNow - cacheContainer.LatestVersionTime).TotalSeconds > configuration.VersionCallTimeout)
                        {
                            var (versionService, versionname) = getVersionService(configuration, versionMappingKey);
                            var version = versionService.GetVersionSync(versionname);
                            if (version != cacheContainer.Version)
                            {
                                cacheContainer = new CacheContainer() { Version = version, LatestVersionTime = DateTime.UtcNow, CacheDict = new HashLinkedCache<object, CacheValueContainer>() { Length = configuration.MaxLength } };
                                _datas[prefix] = cacheContainer;
                            }
                            else
                            {
                                cacheContainer.LatestVersionTime = DateTime.UtcNow;
                            }
                        }
                    }
                    finally
                    {
                        _lock.Release();
                    }

                }
            }

            var valueItem = new CacheValueContainer() { Type=0, Value = value };
            cacheContainer.CacheDict.SetValue(key, valueItem);
        }

        private (IKVCacheVersionService, string) getVersionService(KVCacheConfiguration configuration, string versionMappingKey)
        {
            if (!configuration.VersionNameMappings.TryGetValue(versionMappingKey, out string versionName))
            {
                versionName = configuration.DefaultVersionName;
            }

            if (!_kvCacheVersionServiceFactories.TryGetValue(versionName, out IFactory<IKVCacheVersionService> serviceFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundIKVCacheVersionServiceByName,
                    DefaultFormatting = "找不到版本名称为{0}的KV缓存版本服务，发生位置为{1}",
                    ReplaceParameters = new List<object>() { versionName, $"{this.GetType().FullName}.KVCacheVersionServiceFactories" }
                };

                throw new UtilityException((int)Errors.NotFoundIKVCacheVersionServiceByName, fragment);
            }

            return (serviceFactory.Create(), versionName);
        }


        /// <summary>
        /// 内部缓存值容器
        /// </summary>
        private class CacheValueContainer
        {
            public int Type { get; set; }
            public object Value { get; set; }
        }


        /// <summary>
        ///内部缓存容器
        ///提供线程同步处理方法
        /// </summary>
        private class CacheContainer:IDisposable
        {
            private LocalSemaphore _lock = new LocalSemaphore(1, 1);
            /// <summary>
            /// 缓存哈希链表存储
            /// 默认采用LRU（最近最久未访问）策略算法
            /// </summary>
            public HashLinkedCache<object, CacheValueContainer> CacheDict { get; set; }
            /// <summary>
            /// 当前版本号
            /// </summary>
            public string Version { get; set; }
            /// <summary>
            /// 最后访问版本服务的时间
            /// </summary>
            public DateTime LatestVersionTime { get; set; }

            public void Dispose()
            {
                _lock.Dispose();
            }

            public async Task SyncOperate(Func<Task> action)
            {
                await _lock.SyncOperator(
                    async () =>
                    {
                        await action();
                    }
                    );
            }

            public void SyncOperate(Action action)
            {
                _lock.SyncOperator(
                   () =>
                   {
                       action();
                   }
                   );
            }

            ~CacheContainer()
            {
                _lock.Dispose();
            }
        }
        /// <summary>
        /// 配置信息
        /// </summary>
        [DataContract]
        private class KVCacheConfiguration
        {
            /// <summary>
            /// 最大存储长度
            /// </summary>
            [DataMember]
            public int MaxLength { get; set; }
            /// <summary>
            /// 版本服务调用间隔（单位秒）
            /// </summary>

            [DataMember]
            public int VersionCallTimeout { get; set; }
            /// <summary>
            /// KV类型与版本名称映射键值对
            /// 键为{Key的类型全名}-{Value的类型全名}
            /// </summary>
            [DataMember]
            public Dictionary<string, string> VersionNameMappings { get; set; }
            /// <summary>
            /// 默认版本名称
            /// 找不到KV类型与版本名称的映射时使用该名称
            /// </summary>
            [DataMember]
            public string DefaultVersionName { get; set; }
        }


    }

    /// <summary>
    /// KV缓存版本号服务
    /// </summary>
    public interface IKVCacheVersionService
    {
        /// <summary>
        /// 获取指定版本名称
        /// </summary>
        /// <param name="versionName"></param>
        /// <returns></returns>
        Task<string> GetVersion(string versionName);
        /// <summary>
        /// 获取指定版本名称(同步)
        /// </summary>
        /// <param name="versionName"></param>
        /// <returns></returns>
        string GetVersionSync(string versionName);
    }
}
