using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using MSLibrary.DI;
using MSLibrary.Serializer;

namespace MSLibrary.Cache.RealKVCacheVisitServices
{
    /// <summary>
    /// 基于组合的KV缓存访问服务
    /// 通常用于做多级缓存
    /// cacheConfiguration的格式为
    /// {
    ///     "VistorNames":[访问者名称1，访问者名称2,...]
    /// }
    /// </summary>
    [Injection(InterfaceType = typeof(RealKVCacheVisitServiceForCombination), Scope = InjectionScope.Singleton)]
    public class RealKVCacheVisitServiceForCombination : IRealKVCacheVisitService
    {

        private IKVCacheVisitorRepositoryCacheProxy _kvCacheVisitorRepositoryCacheProxy;

        public RealKVCacheVisitServiceForCombination(IKVCacheVisitorRepositoryCacheProxy kvCacheVisitorRepositoryCacheProxy)
        {
            _kvCacheVisitorRepositoryCacheProxy = kvCacheVisitorRepositoryCacheProxy;
        }

        public async Task<V> Get<K, V>(string cacheConfiguration, Func<Task<V>> creator, string prefix, K key)
        {
            var configuration = JsonSerializerHelper.Deserialize<KVCacheConfiguration>(cacheConfiguration);

            Func<K, Task<V>> currentCreator = async (k) =>
            {
                return await creator();
            };
            for (var index = configuration.VistorNames.Count - 1; index >= 0; index--)
            {
                var innerIndex = index;
                var innerCreate = currentCreator;
                currentCreator = async (k) =>
                {
                    var visitor = await _kvCacheVisitorRepositoryCacheProxy.QueryByName(configuration.VistorNames[innerIndex]);
                    return await visitor.Get(innerCreate, key);
                };
            }

            return await currentCreator(key);
        }

        public V GetSync<K, V>(string cacheConfiguration, Func<V> creator, string prefix, K key)
        {
            var configuration = JsonSerializerHelper.Deserialize<KVCacheConfiguration>(cacheConfiguration);

            Func<K, V> currentCreator = (k) =>
            {
                return creator();
            };
            for (var index = configuration.VistorNames.Count - 1; index >= 0; index--)
            {
                currentCreator = (k) =>
                {
                    var visitor = _kvCacheVisitorRepositoryCacheProxy.QueryByNameSync(configuration.VistorNames[index]);
                    return visitor.GetSync(currentCreator, key);
                };
            }

            return currentCreator(key);
        }

        public async Task Set<K, V>(string cacheConfiguration, string prefix, K key, V value)
        {
            var configuration = JsonSerializerHelper.Deserialize<KVCacheConfiguration>(cacheConfiguration);

            for (var index = configuration.VistorNames.Count - 1; index >= 0; index--)
            {
                    var visitor = _kvCacheVisitorRepositoryCacheProxy.QueryByNameSync(configuration.VistorNames[index]);
                    await visitor.Set(key,value);
            }
        }

        public void SetSync<K, V>(string cacheConfiguration, string prefix, K key, V value)
        {
            var configuration = JsonSerializerHelper.Deserialize<KVCacheConfiguration>(cacheConfiguration);

            for (var index = configuration.VistorNames.Count - 1; index >= 0; index--)
            {
                var visitor = _kvCacheVisitorRepositoryCacheProxy.QueryByNameSync(configuration.VistorNames[index]);
                visitor.SetSync(key, value);
            }
        }


        /// <summary>
        /// 配置信息
        /// </summary>
        [DataContract]
        private class KVCacheConfiguration
        {
            /// <summary>
            /// 需要组合使用的KV缓存访问者名称集合
            /// </summary>
            [DataMember]
            public List<string> VistorNames { get; set; }
        }

    }
}
