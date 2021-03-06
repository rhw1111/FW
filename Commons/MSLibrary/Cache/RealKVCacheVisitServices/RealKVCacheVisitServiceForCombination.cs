﻿using System;
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

        public async Task Clear<K,V>(string cacheConfiguration, string prefix, K key)
        {
            var configuration = JsonSerializerHelper.Deserialize<KVCacheConfiguration>(cacheConfiguration);

            for (var index = configuration.VistorNames.Count - 1; index >= 0; index--)
            {
                var visitor = _kvCacheVisitorRepositoryCacheProxy.QueryByNameSync(configuration.VistorNames[index]);
                await visitor.Clear<K,V>(key);
            }
        }

        public async Task Clear<K, V>(string cacheConfiguration, string prefix, IEnumerable<K> keys)
        {
            var configuration = JsonSerializerHelper.Deserialize<KVCacheConfiguration>(cacheConfiguration);

            for (var index = configuration.VistorNames.Count - 1; index >= 0; index--)
            {
                var visitor = _kvCacheVisitorRepositoryCacheProxy.QueryByNameSync(configuration.VistorNames[index]);
                await visitor.Clear<K, V>(keys);
            }
        }

        public void ClearSync<K,V>(string cacheConfiguration, string prefix, K key)
        {
            var configuration = JsonSerializerHelper.Deserialize<KVCacheConfiguration>(cacheConfiguration);

            for (var index = configuration.VistorNames.Count - 1; index >= 0; index--)
            {
                var visitor = _kvCacheVisitorRepositoryCacheProxy.QueryByNameSync(configuration.VistorNames[index]);
                visitor.ClearSync<K,V>(key);
            }
        }

        public void ClearSync<K, V>(string cacheConfiguration, string prefix, IEnumerable<K> keys)
        {
            var configuration = JsonSerializerHelper.Deserialize<KVCacheConfiguration>(cacheConfiguration);

            for (var index = configuration.VistorNames.Count - 1; index >= 0; index--)
            {
                var visitor = _kvCacheVisitorRepositoryCacheProxy.QueryByNameSync(configuration.VistorNames[index]);
                visitor.ClearSync<K, V>(keys);
            }
        }

        public async Task<(V,bool)> Get<K, V>(string cacheConfiguration, Func<Task<(V, bool)>> creator, string prefix, K key)
        {
            var configuration = JsonSerializerHelper.Deserialize<KVCacheConfiguration>(cacheConfiguration);

            
            Func<K, Task<(V,bool)>> currentCreator = async (k) =>
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

        public async Task<(V, bool)> GetHash<K, V>(string cacheConfiguration,  Func<Task<(V, bool)>> creator,string prefix, K key, string hashKey)
        {
            var configuration = JsonSerializerHelper.Deserialize<KVCacheConfiguration>(cacheConfiguration);


            Func<K,string, Task<(V, bool)>> currentCreator = async (k,hashK) =>
            {
                return await creator();
            };
            for (var index = configuration.VistorNames.Count - 1; index >= 0; index--)
            {
                var innerIndex = index;
                var innerCreate = currentCreator;
                currentCreator = async (k,hashK) =>
                {
                    var visitor = await _kvCacheVisitorRepositoryCacheProxy.QueryByName(configuration.VistorNames[innerIndex]);
                    return await visitor.GetHash<K,V>(innerCreate, key,hashKey);
                };
            }

            return await currentCreator(key,hashKey);
        }

        public (V, bool) GetHashSync<K, V>(string cacheConfiguration, Func<(V, bool)> creator,string prefix,  K key, string hashKey)
        {
            var configuration = JsonSerializerHelper.Deserialize<KVCacheConfiguration>(cacheConfiguration);


            Func<K, string, (V, bool)> currentCreator = (k, hashK) =>
            {
                return creator();
            };
            for (var index = configuration.VistorNames.Count - 1; index >= 0; index--)
            {
                var innerIndex = index;
                var innerCreate = currentCreator;
                currentCreator =  (k, hashK) =>
                {
                    var visitor =  _kvCacheVisitorRepositoryCacheProxy.QueryByNameSync(configuration.VistorNames[innerIndex]);
                    return  visitor.GetHashSync<K, V>(innerCreate, key, hashKey);
                };
            }

            return currentCreator(key, hashKey);
        }

        public (V,bool) GetSync<K, V>(string cacheConfiguration, Func<(V, bool)> creator, string prefix, K key)
        {
            var configuration = JsonSerializerHelper.Deserialize<KVCacheConfiguration>(cacheConfiguration);

            Func<K, (V,bool)> currentCreator = (k) =>
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

        public async Task SetHash<K, V>(string cacheConfiguration, string prefix, K key, IDictionary<string, V> values)
        {
            var configuration = JsonSerializerHelper.Deserialize<KVCacheConfiguration>(cacheConfiguration);

            for (var index = configuration.VistorNames.Count - 1; index >= 0; index--)
            {
                var visitor = _kvCacheVisitorRepositoryCacheProxy.QueryByNameSync(configuration.VistorNames[index]);
                await visitor.SetHash(key, values);
            }
        }

        public void SetHashSync<K, V>(string cacheConfiguration, string prefix, K key, IDictionary<string, V> values)
        {
            var configuration = JsonSerializerHelper.Deserialize<KVCacheConfiguration>(cacheConfiguration);

            for (var index = configuration.VistorNames.Count - 1; index >= 0; index--)
            {
                var visitor = _kvCacheVisitorRepositoryCacheProxy.QueryByNameSync(configuration.VistorNames[index]);
                visitor.SetHashSync(key, values);
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
