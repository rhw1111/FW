using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using MSLibrary.DI;
using MSLibrary.Serializer;
using MSLibrary.Thread;

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

        public async Task<V> Get<K, V>(string cacheConfiguration, Func<Task<V>> creator, string prefix, K key)
        {
            var configuration = JsonSerializerHelper.Deserialize<KVCacheConfiguration>(cacheConfiguration);
            if (!_datas.TryGetValue(prefix, out CacheContainer cacheContainer))
            {
                await _lock.WaitAsync();
                try
                {
                    if (!_datas.TryGetValue(prefix, out cacheContainer))
                    {
                        cacheContainer = new CacheContainer() { CacheDict =new HashLinkedCache<object, CacheTimeContainer<object>>() { Length = configuration.MaxLength } };
                        _datas[prefix] = cacheContainer;
                    }
                }
                finally
                {
                    _lock.Release();
                }
            }

            CacheTimeContainer<object> cacheItem = cacheContainer.CacheDict.GetValue(key);
            if (cacheItem == null || cacheItem.Expire())
            {
                    await cacheContainer.SyncOperate(
                    async()=>
                    {
                        cacheItem = cacheContainer.CacheDict.GetValue(key);
                        if (cacheItem == null || cacheItem.Expire())
                        {
                            var cacheValue = await creator();
                            cacheItem = new CacheTimeContainer<object>(cacheValue, configuration.ExpireSeconds);
                            cacheContainer.CacheDict.SetValue(key, cacheItem);
                        }
                    }
                    );

            }

            return (V)cacheItem.Value;
        }

        public V GetSync<K, V>(string cacheConfiguration, Func<V> creator, string prefix, K key)
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

            CacheTimeContainer<object> cacheItem = cacheContainer.CacheDict.GetValue(key);
            if (cacheItem == null || cacheItem.Expire())
            {
                 cacheContainer.SyncOperate(
                 () =>
                {
                    cacheItem = cacheContainer.CacheDict.GetValue(key);
                    if (cacheItem == null || cacheItem.Expire())
                    {
                        var cacheValue =  creator();
                        cacheItem = new CacheTimeContainer<object>(cacheValue, configuration.ExpireSeconds);
                        cacheContainer.CacheDict.SetValue(key, cacheItem);
                    }
                }
                );

            }

            return (V)cacheItem.Value;
        }

        public async Task Set<K, V>(string cacheConfiguration, string prefix, K key, V value)
        {
            SetSync(cacheConfiguration, prefix, key, value);
            await Task.CompletedTask;
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

            cacheContainer.CacheDict.SetValue(key, new CacheTimeContainer<object>(value, configuration.ExpireSeconds));
        }


        /// <summary>
        ///内部缓存容器
        ///提供线程同步处理方法
        /// </summary>
        private class CacheContainer
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
