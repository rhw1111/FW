using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Linq;
using MSLibrary.DI;
using MSLibrary.Serializer;
using MSLibrary.Thread;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.Cache.RealKVCacheVisitServices
{
    /// <summary>
    /// 基于本地超时的KV缓存访问服务
    /// cacheConfiguration的格式为
    /// {
    ///     "MaxLength":最大缓存长度,
    ///     "ExpireSeconds":缓存过期时间
    /// }
    /// </summary>
    [Injection(InterfaceType = typeof(RealKVCacheVisitServiceForLocalTimeout), Scope = InjectionScope.Singleton)]
    public class RealKVCacheVisitServiceForLocalTimeout : IRealKVCacheVisitService
    {
        private static SemaphoreSlim _lock = new SemaphoreSlim(1,1);

        private static Dictionary<string, CacheContainer> _datas = new Dictionary<string, CacheContainer>();

        public async Task Clear<K, V>(string cacheConfiguration, string prefix, K key)
        {
            ClearSync<K, V>(cacheConfiguration, prefix,key);
            await Task.CompletedTask;
        }

        public async Task Clear<K, V>(string cacheConfiguration, string prefix, IEnumerable<K> keys)
        {
            ClearSync<K, V>(cacheConfiguration, prefix, keys);
            await Task.CompletedTask;
        }

        public void ClearSync<K, V>(string cacheConfiguration, string prefix, K key)
        {
            var configuration = JsonSerializerHelper.Deserialize<KVCacheConfiguration>(cacheConfiguration);
            if (_datas.TryGetValue(prefix, out CacheContainer cacheContainer))
            {

                if (_datas.TryGetValue(prefix, out cacheContainer))
                {
                        cacheContainer.CacheDict.Remove(key);
                }
            }
        }

        public void ClearSync<K, V>(string cacheConfiguration, string prefix, IEnumerable<K> keys)
        {
            var configuration = JsonSerializerHelper.Deserialize<KVCacheConfiguration>(cacheConfiguration);
            if (_datas.TryGetValue(prefix, out CacheContainer cacheContainer))
            {

                if (_datas.TryGetValue(prefix, out cacheContainer))
                {
                    foreach (var item in keys)
                    {
                        cacheContainer.CacheDict.Remove(item);
                    }
                }
            }
        }

        public async Task<(V, bool)> Get<K, V>(string cacheConfiguration, Func<Task<(V, bool)>> creator, string prefix, K key)
        {
            var configuration = JsonSerializerHelper.Deserialize<KVCacheConfiguration>(cacheConfiguration);
            if (!_datas.TryGetValue(prefix, out CacheContainer cacheContainer))
            {
                await _lock.WaitAsync();
                try
                {
                    if (!_datas.TryGetValue(prefix, out cacheContainer))
                    {
                        cacheContainer = new CacheContainer() { CacheDict = new HashLinkedCache<object, CacheTimeContainer<object>>() { Length = configuration.MaxLength } };
                        _datas[prefix] = cacheContainer;
                    }
                }
                finally
                {
                    _lock.Release();
                }
            }

            bool isCache = true;
            CacheTimeContainer<object> cacheItem = cacheContainer.CacheDict.GetValue(key);
            if (cacheItem == null || cacheItem.Expire())
            {
                await cacheContainer.SyncOperate(
                async () =>
                {
                    cacheItem = cacheContainer.CacheDict.GetValue(key);
                    if (cacheItem == null || cacheItem.Expire())
                    {
                        var (cacheValue, isCache) = await creator();
                        cacheItem = new CacheTimeContainer<object>(cacheValue, configuration.ExpireSeconds,0);
                        if (isCache)
                        {
                            cacheContainer.CacheDict.SetValue(key, cacheItem);
                        }
                    }
                }
                );

            }

            if (cacheItem.Type != 0)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.KVCacheValueContainerTypeError,
                    DefaultFormatting = "键为{0}的KV缓存值容器类型不正确，需要的类型为{1}，当前类型为{2}，发生位置为{3}",
                    ReplaceParameters = new List<object>() { key.ToString(), "0", cacheItem.Type.ToString(), $"{this.GetType().FullName}.Get<K, V>" }
                };

                throw new UtilityException((int)Errors.KVCacheValueContainerTypeError, fragment);
            }


            return ((V)cacheItem.Value, isCache);

        }

        public async Task<(V, bool)> GetHash<K, V>(string cacheConfiguration,  Func<Task<(V, bool)>> creator,string prefix, K key, string hashKey)
        {
            var configuration = JsonSerializerHelper.Deserialize<KVCacheConfiguration>(cacheConfiguration);
            if (!_datas.TryGetValue(prefix, out CacheContainer cacheContainer))
            {
                await _lock.WaitAsync();
                try
                {
                    if (!_datas.TryGetValue(prefix, out cacheContainer))
                    {
                        cacheContainer = new CacheContainer() { CacheDict = new HashLinkedCache<object, CacheTimeContainer<object>>() { Length = configuration.MaxLength } };
                        _datas[prefix] = cacheContainer;
                    }
                }
                finally
                {
                    _lock.Release();
                }
            }

            bool isCache = true;
            CacheTimeContainer<object> cacheItem = cacheContainer.CacheDict.GetValue(key);
            if (cacheItem == null || cacheItem.Expire())
            {
                await cacheContainer.SyncOperate(
                async () =>
                {
                    cacheItem = cacheContainer.CacheDict.GetValue(key);
                    if (cacheItem == null || cacheItem.Expire())
                    {
                        var (cacheValue, isCache) = await creator();


                        Dictionary<string, V> realValue = new Dictionary<string, V>();
                        if (isCache)
                        {
                            realValue[hashKey] = cacheValue;
                        }
                        cacheItem = new CacheTimeContainer<object>(cacheValue, configuration.ExpireSeconds, 1);
                        cacheContainer.CacheDict.SetValue(key, cacheItem);
                    }
                }
                );

            }

            if (cacheItem.Type != 1)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.KVCacheValueContainerTypeError,
                    DefaultFormatting = "键为{0}的KV缓存值容器类型不正确，需要的类型为{1}，当前类型为{2}，发生位置为{3}",
                    ReplaceParameters = new List<object>() { key.ToString(), "1", cacheItem.Type.ToString(), $"{this.GetType().FullName}.GetHash<K, V>" }
                };

                throw new UtilityException((int)Errors.KVCacheValueContainerTypeError, fragment);
            }

            var dictValue = (Dictionary<string, V>)cacheItem.Value;

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

        public (V, bool) GetHashSync<K, V>(string cacheConfiguration,  Func<(V, bool)> creator,string prefix, K key, string hashKey)
        {
            var configuration = JsonSerializerHelper.Deserialize<KVCacheConfiguration>(cacheConfiguration);
            if (!_datas.TryGetValue(prefix, out CacheContainer cacheContainer))
            {
                 _lock.Wait();
                try
                {
                    if (!_datas.TryGetValue(prefix, out cacheContainer))
                    {
                        cacheContainer = new CacheContainer() { CacheDict = new HashLinkedCache<object, CacheTimeContainer<object>>() { Length = configuration.MaxLength } };
                        _datas[prefix] = cacheContainer;
                    }
                }
                finally
                {
                    _lock.Release();
                }
            }

            bool isCache = true;
            CacheTimeContainer<object> cacheItem = cacheContainer.CacheDict.GetValue(key);
            if (cacheItem == null || cacheItem.Expire())
            {
                cacheContainer.SyncOperate(
                () =>
                {
                    cacheItem = cacheContainer.CacheDict.GetValue(key);
                    if (cacheItem == null || cacheItem.Expire())
                    {
                        var (cacheValue, isCache) = creator();


                        Dictionary<string, V> realValue = new Dictionary<string, V>();
                        if (isCache)
                        {
                            realValue[hashKey] = cacheValue;
                        }
                        cacheItem = new CacheTimeContainer<object>(cacheValue, configuration.ExpireSeconds, 1);
                        cacheContainer.CacheDict.SetValue(key, cacheItem);
                    }
                }
                );

            }

            if (cacheItem.Type != 1)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.KVCacheValueContainerTypeError,
                    DefaultFormatting = "键为{0}的KV缓存值容器类型不正确，需要的类型为{1}，当前类型为{2}，发生位置为{3}",
                    ReplaceParameters = new List<object>() { key.ToString(), "1", cacheItem.Type.ToString(), $"{this.GetType().FullName}.GetHashSync<K, V>" }
                };

                throw new UtilityException((int)Errors.KVCacheValueContainerTypeError, fragment);
            }

            var dictValue = (Dictionary<string, V>)cacheItem.Value;

            if (!dictValue.TryGetValue(hashKey, out V realValue))
            {
                (realValue, isCache) = creator();
                if (isCache)
                {
                    dictValue[hashKey] = realValue;
                }
            }

            return (realValue, isCache);
        }

        public (V, bool) GetSync<K, V>(string cacheConfiguration, Func<(V, bool)> creator, string prefix, K key)
        {
            var configuration = JsonSerializerHelper.Deserialize<KVCacheConfiguration>(cacheConfiguration);
            if (!_datas.TryGetValue(prefix, out CacheContainer cacheContainer))
            {
                _lock.Wait();
                try
                {
                    if (!_datas.TryGetValue(prefix, out cacheContainer))
                    {
                        cacheContainer = new CacheContainer() { CacheDict = new HashLinkedCache<object, CacheTimeContainer<object>>() { Length = configuration.MaxLength } };
                        _datas[prefix] = cacheContainer;
                    }
                }
                finally
                {
                    _lock.Release();
                }
            }

            bool isCache = true;
            CacheTimeContainer<object> cacheItem = cacheContainer.CacheDict.GetValue(key);
            if (cacheItem == null || cacheItem.Expire())
            {
                cacheContainer.SyncOperate(
                () =>
               {
                   cacheItem = cacheContainer.CacheDict.GetValue(key);
                   if (cacheItem == null || cacheItem.Expire())
                   {

                       var (cacheValue, isCache) = creator();
                       cacheItem = new CacheTimeContainer<object>(cacheValue, configuration.ExpireSeconds,0);
                       if (isCache)
                       {
                           cacheContainer.CacheDict.SetValue(key, cacheItem);
                       }

                   }
               }
               );

            }

            if (cacheItem.Type != 0)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.KVCacheValueContainerTypeError,
                    DefaultFormatting = "键为{0}的KV缓存值容器类型不正确，需要的类型为{1}，当前类型为{2}，发生位置为{3}",
                    ReplaceParameters = new List<object>() { key.ToString(), "0", cacheItem.Type.ToString(), $"{this.GetType().FullName}.GetSync<K, V>" }
                };

                throw new UtilityException((int)Errors.KVCacheValueContainerTypeError, fragment);
            }

            return ((V)cacheItem.Value, isCache);

        }

        public async Task Set<K, V>(string cacheConfiguration, string prefix, K key, V value)
        {
            var configuration = JsonSerializerHelper.Deserialize<KVCacheConfiguration>(cacheConfiguration);
            if (!_datas.TryGetValue(prefix, out CacheContainer cacheContainer))
            {
                await _lock.WaitAsync();
                try
                {
                    if (!_datas.TryGetValue(prefix, out cacheContainer))
                    {
                        cacheContainer = new CacheContainer() { CacheDict = new HashLinkedCache<object, CacheTimeContainer<object>>() { Length = configuration.MaxLength } };
                        _datas[prefix] = cacheContainer;
                    }
                }
                finally
                {
                    _lock.Release();
                }
            }

            cacheContainer.CacheDict.SetValue(key, new CacheTimeContainer<object>(value, configuration.ExpireSeconds,0));
        }

        public async Task SetHash<K, V>(string cacheConfiguration, string prefix, K key, IDictionary<string, V> values)
        {

            var configuration = JsonSerializerHelper.Deserialize<KVCacheConfiguration>(cacheConfiguration);
            if (!_datas.TryGetValue(prefix, out CacheContainer cacheContainer))
            {
                await _lock.WaitAsync();
                try
                {
                    if (!_datas.TryGetValue(prefix, out cacheContainer))
                    {
                        cacheContainer = new CacheContainer() { CacheDict = new HashLinkedCache<object, CacheTimeContainer<object>>() { Length = configuration.MaxLength } };
                        _datas[prefix] = cacheContainer;
                    }
                }
                finally
                {
                    _lock.Release();
                }
            }

            cacheContainer.CacheDict.SetValue(key, new CacheTimeContainer<object>(values.ToDictionary((kv) => kv.Key, (kv) => kv.Value), configuration.ExpireSeconds, 1));

        }

        public void SetHashSync<K, V>(string cacheConfiguration, string prefix, K key, IDictionary<string, V> values)
        {
            var configuration = JsonSerializerHelper.Deserialize<KVCacheConfiguration>(cacheConfiguration);
            if (!_datas.TryGetValue(prefix, out CacheContainer cacheContainer))
            {
                _lock.Wait();
                try
                {
                    if (!_datas.TryGetValue(prefix, out cacheContainer))
                    {
                        cacheContainer = new CacheContainer() { CacheDict = new HashLinkedCache<object, CacheTimeContainer<object>>() { Length = configuration.MaxLength } };
                        _datas[prefix] = cacheContainer;
                    }
                }
                finally
                {
                    _lock.Release();
                }
            }

            cacheContainer.CacheDict.SetValue(key, new CacheTimeContainer<object>(values.ToDictionary((kv) => kv.Key, (kv) => kv.Value), configuration.ExpireSeconds, 1));
        }

        public void SetSync<K, V>(string cacheConfiguration, string prefix, K key, V value)
        {
            var configuration = JsonSerializerHelper.Deserialize<KVCacheConfiguration>(cacheConfiguration);
            if (!_datas.TryGetValue(prefix, out CacheContainer cacheContainer))
            {
                _lock.Wait();
                try
                {
                    if (!_datas.TryGetValue(prefix, out cacheContainer))
                    {
                        cacheContainer = new CacheContainer() { CacheDict = new HashLinkedCache<object, CacheTimeContainer<object>>() { Length = configuration.MaxLength } };
                        _datas[prefix] = cacheContainer;
                    }
                }
                finally
                {
                    _lock.Release();
                }
            }

            cacheContainer.CacheDict.SetValue(key, new CacheTimeContainer<object>(value, configuration.ExpireSeconds,0));
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
            public HashLinkedCache<object, CacheTimeContainer<object>> CacheDict { get; set; }

            public async Task SyncOperate(Func<Task> action)
            {
                await _lock.SyncOperator(
                    async()=>
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

            public void Dispose()
            {
                _lock.Dispose();
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
            /// 过期时间（单位秒）
            /// </summary>
            [DataMember]
            public int ExpireSeconds { get; set; }
        }
    }
}
