using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using MSLibrary.DI;
using MSLibrary.Serializer;
using MSLibrary.Thread;
using MSLibrary.Redis;
using MSLibrary.LanguageTranslate;
using CSRedis;
using MSLibrary.Template;

namespace MSLibrary.Cache.RealKVCacheVisitServices
{
    /// <summary>
    /// 基于Redis的KV缓存访问服务
    /// cacheConfiguration的格式为
    /// {
    ///     "RedisClientFactoryName":"客户端工厂名称",
    ///     "ExpireSeconds":缓存过期时间,<=0表示永久
    /// }
    /// </summary>
    [Injection(InterfaceType = typeof(RealKVCacheVisitServiceForRedis), Scope = InjectionScope.Singleton)]
    public class RealKVCacheVisitServiceForRedis : IRealKVCacheVisitService
    {
        private IRedisClientFactoryRepositoryCacheProxy _redisClientFactoryRepositoryCacheProxy;

        public RealKVCacheVisitServiceForRedis(IRedisClientFactoryRepositoryCacheProxy redisClientFactoryRepositoryCacheProxy)
        {
            _redisClientFactoryRepositoryCacheProxy = redisClientFactoryRepositoryCacheProxy;
        }

        public async Task Clear<K, V>(string cacheConfiguration, string prefix, K key)
        {
            var configurationObj = JsonSerializerHelper.Deserialize<Configuration>(cacheConfiguration);
            var redisClientFactory =await _redisClientFactoryRepositoryCacheProxy.QueryByName(configurationObj.RedisClientFactoryName);
            if (redisClientFactory == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundRedisClientFactoryByName,
                    DefaultFormatting = "找不到名称为{0}的Redis客户端工厂",
                    ReplaceParameters = new List<object>() { configurationObj.RedisClientFactoryName }
                };

                throw new UtilityException((int)Errors.NotFoundRedisClientFactoryByName, fragment);
            }

            var redisClient =await redisClientFactory.GenerateClient();
            string strKey;
            if (key is string)
            {
                strKey = key.ToString();
            }
            else 
            {
                strKey = JsonSerializerHelper.Serializer(key);
            }
            await redisClient.DelAsync(strKey);
        }

        public async Task Clear<K, V>(string cacheConfiguration, string prefix, IEnumerable<K> keys)
        {
            var configurationObj = JsonSerializerHelper.Deserialize<Configuration>(cacheConfiguration);
            var redisClientFactory =await _redisClientFactoryRepositoryCacheProxy.QueryByName(configurationObj.RedisClientFactoryName);
            if (redisClientFactory == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundRedisClientFactoryByName,
                    DefaultFormatting = "找不到名称为{0}的Redis客户端工厂",
                    ReplaceParameters = new List<object>() { configurationObj.RedisClientFactoryName }
                };

                throw new UtilityException((int)Errors.NotFoundRedisClientFactoryByName, fragment);
            }

            var redisClient =await redisClientFactory.GenerateClient();

            List<string> strKeys=new List<string>();
            foreach(var item in keys)
            {
                if (item is string)
                {
                    strKeys.Add(item.ToString());
                }
                else
                {
                    strKeys.Add(JsonSerializerHelper.Serializer(item));
                }
            }

            await redisClient.DelAsync(strKeys.ToArray());
        }

        public void ClearSync<K, V>(string cacheConfiguration, string prefix, K key)
        {
            var configurationObj = JsonSerializerHelper.Deserialize<Configuration>(cacheConfiguration);
            var redisClientFactory = _redisClientFactoryRepositoryCacheProxy.QueryByNameSync(configurationObj.RedisClientFactoryName);
            if (redisClientFactory == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundRedisClientFactoryByName,
                    DefaultFormatting = "找不到名称为{0}的Redis客户端工厂",
                    ReplaceParameters = new List<object>() { configurationObj.RedisClientFactoryName }
                };

                throw new UtilityException((int)Errors.NotFoundRedisClientFactoryByName, fragment);
            }

            var redisClient = redisClientFactory.GenerateClientSync();

            string strKey;
            if (key is string)
            {
                strKey = key.ToString();
            }
            else
            {
                strKey = JsonSerializerHelper.Serializer(key);
            }

            redisClient.Del(strKey);
        }

        public void ClearSync<K, V>(string cacheConfiguration, string prefix, IEnumerable<K> keys)
        {
            var configurationObj = JsonSerializerHelper.Deserialize<Configuration>(cacheConfiguration);
            var redisClientFactory = _redisClientFactoryRepositoryCacheProxy.QueryByNameSync(configurationObj.RedisClientFactoryName);
            if (redisClientFactory == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundRedisClientFactoryByName,
                    DefaultFormatting = "找不到名称为{0}的Redis客户端工厂",
                    ReplaceParameters = new List<object>() { configurationObj.RedisClientFactoryName }
                };

                throw new UtilityException((int)Errors.NotFoundRedisClientFactoryByName, fragment);
            }

            var redisClient = redisClientFactory.GenerateClientSync();
         
            List<string> strKeys = new List<string>();
            foreach (var item in keys)
            {
                if (item is string)
                {
                    strKeys.Add(item.ToString());
                }
                else
                {
                    strKeys.Add(JsonSerializerHelper.Serializer(item));
                }
            }
           
            redisClient.Del(strKeys.ToArray());
        }

        public async Task<(V,bool)> Get<K, V>(string cacheConfiguration, Func<Task<(V,bool)>> creator, string prefix, K key)
        {
            var configurationObj = JsonSerializerHelper.Deserialize<Configuration>(cacheConfiguration);
            var redisClientFactory = await _redisClientFactoryRepositoryCacheProxy.QueryByName(configurationObj.RedisClientFactoryName);
            if (redisClientFactory == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundRedisClientFactoryByName,
                    DefaultFormatting = "找不到名称为{0}的Redis客户端工厂",
                    ReplaceParameters = new List<object>() { configurationObj.RedisClientFactoryName }
                };

                throw new UtilityException((int)Errors.NotFoundRedisClientFactoryByName, fragment);
            }

            var redisClient =await redisClientFactory.GenerateClient();

            string strKey;
            if (key is string)
            {
                strKey = key.ToString();
            }
            else
            {
                strKey = JsonSerializerHelper.Serializer(key);
            }

            var strValue=await redisClient.GetAsync(strKey);
            if (strValue!=null)
            {
                return (JsonSerializerHelper.Deserialize<V>(strValue),true);
            }
            else
            {
                var (value,isCache) = await creator();
                if (isCache)
                {
                    strValue = JsonSerializerHelper.Serializer(value);
                    if (configurationObj.ExpireSeconds > 0)
                    {
                        await redisClient.SetAsync(strKey, strValue, configurationObj.ExpireSeconds, RedisExistence.Nx);
                    }
                    else
                    {
                        await redisClient.SetNxAsync(strKey, strValue);
                    }
                    return (value,true);
                }
                else
                {
                    return (value, false);
                }
            }
        }

        public async Task<(V, bool)> GetHash<K, V>(string cacheConfiguration, Func<Task<(V, bool)>> creator, string prefix, K key, string hashKey)
        {
            var configurationObj = JsonSerializerHelper.Deserialize<Configuration>(cacheConfiguration);
            var redisClientFactory = await _redisClientFactoryRepositoryCacheProxy.QueryByName(configurationObj.RedisClientFactoryName);
            if (redisClientFactory == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundRedisClientFactoryByName,
                    DefaultFormatting = "找不到名称为{0}的Redis客户端工厂",
                    ReplaceParameters = new List<object>() { configurationObj.RedisClientFactoryName }
                };

                throw new UtilityException((int)Errors.NotFoundRedisClientFactoryByName, fragment);
            }

            var redisClient = await redisClientFactory.GenerateClient();

            string strKey;
            if (key is string)
            {
                strKey = key.ToString();
            }
            else
            {
                strKey = JsonSerializerHelper.Serializer(key);
            }

            var strValue = await redisClient.HGetAsync(strKey,hashKey);
            if (strValue != null)
            {
                return (JsonSerializerHelper.Deserialize<V>(strValue), true);
            }
            else
            {
                var (value, isCache) = await creator();
                if (isCache)
                {
                    strValue = JsonSerializerHelper.Serializer(value); 
                    await redisClient.HSetAsync(strKey,hashKey, strValue);

                    if (configurationObj.ExpireSeconds > 0)
                    {
                        var ttlResult=await redisClient.TtlAsync(strKey);
                        if (ttlResult<0)
                        {
                            await redisClient.ExpireAsync(strKey, configurationObj.ExpireSeconds);
                        }
                    }

                    return (value, true);
                }
                else
                {
                    return (value, false);
                }
            }
        }

        public (V, bool) GetHashSync<K, V>(string cacheConfiguration, Func<(V, bool)> creator, string prefix, K key, string hashKey)
        {
            var configurationObj = JsonSerializerHelper.Deserialize<Configuration>(cacheConfiguration);
            var redisClientFactory =  _redisClientFactoryRepositoryCacheProxy.QueryByNameSync(configurationObj.RedisClientFactoryName);
            if (redisClientFactory == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundRedisClientFactoryByName,
                    DefaultFormatting = "找不到名称为{0}的Redis客户端工厂",
                    ReplaceParameters = new List<object>() { configurationObj.RedisClientFactoryName }
                };

                throw new UtilityException((int)Errors.NotFoundRedisClientFactoryByName, fragment);
            }

            var redisClient =  redisClientFactory.GenerateClientSync();

            string strKey;
            if (key is string)
            {
                strKey = key.ToString();
            }
            else
            {
                strKey = JsonSerializerHelper.Serializer(key);
            }

            var strValue =  redisClient.HGet(strKey, hashKey);
            if (strValue != null)
            {
                return (JsonSerializerHelper.Deserialize<V>(strValue), true);
            }
            else
            {
                var (value, isCache) =  creator();
                if (isCache)
                {
                    strValue = JsonSerializerHelper.Serializer(value);
                     redisClient.HSet(strKey, hashKey, strValue);

                    if (configurationObj.ExpireSeconds > 0)
                    {
                        var ttlResult =  redisClient.Ttl(strKey);
                        if (ttlResult < 0)
                        {
                             redisClient.Expire(strKey, configurationObj.ExpireSeconds);
                        }
                    }

                    return (value, true);
                }
                else
                {
                    return (value, false);
                }
            }
        }

        public (V,bool) GetSync<K, V>(string cacheConfiguration, Func<(V,bool)> creator, string prefix, K key)
        {
            var configurationObj = JsonSerializerHelper.Deserialize<Configuration>(cacheConfiguration);
            var redisClientFactory = _redisClientFactoryRepositoryCacheProxy.QueryByNameSync(configurationObj.RedisClientFactoryName);
            if (redisClientFactory == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundRedisClientFactoryByName,
                    DefaultFormatting = "找不到名称为{0}的Redis客户端工厂",
                    ReplaceParameters = new List<object>() { configurationObj.RedisClientFactoryName }
                };

                throw new UtilityException((int)Errors.NotFoundRedisClientFactoryByName, fragment);
            }

            var redisClient = redisClientFactory.GenerateClientSync();

            string strKey;
            if (key is string)
            {
                strKey = key.ToString();
            }
            else
            {
                strKey = JsonSerializerHelper.Serializer(key);
            }

            var strValue =  redisClient.Get(strKey);
            if (strValue != null)
            {
                return (JsonSerializerHelper.Deserialize<V>(strValue),true);
            }
            else
            {
                var (value,isCache) =  creator();
                if (isCache)
                {
                    strValue = JsonSerializerHelper.Serializer(value);
                    if (configurationObj.ExpireSeconds > 0)
                    {
                        redisClient.Set(strKey, strValue, configurationObj.ExpireSeconds, RedisExistence.Nx);
                    }
                    else
                    {
                        redisClient.SetNx(strKey, strValue);
                    }
                    return (value, true);
                }
                else
                {
                    return (value, false);
                }
            }
        }

        public async Task Set<K, V>(string cacheConfiguration, string prefix, K key, V value)
        {
            var configurationObj = JsonSerializerHelper.Deserialize<Configuration>(cacheConfiguration);
            var redisClientFactory = await _redisClientFactoryRepositoryCacheProxy.QueryByName(configurationObj.RedisClientFactoryName);
            if (redisClientFactory == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundRedisClientFactoryByName,
                    DefaultFormatting = "找不到名称为{0}的Redis客户端工厂",
                    ReplaceParameters = new List<object>() { configurationObj.RedisClientFactoryName }
                };

                throw new UtilityException((int)Errors.NotFoundRedisClientFactoryByName, fragment);
            }

            var redisClient =await redisClientFactory.GenerateClient();

            string strKey;
            if (key is string)
            {
                strKey = key.ToString();
            }
            else
            {
                strKey = JsonSerializerHelper.Serializer(key);
            }

            var strValue = JsonSerializerHelper.Serializer(value);
            if (configurationObj.ExpireSeconds > 0)
            {
                await redisClient.SetAsync(strKey, strValue, configurationObj.ExpireSeconds, RedisExistence.Nx);
            }
            else
            {
                await redisClient.SetNxAsync(strKey, strValue);
            }

        }

        public async Task SetHash<K, V>(string cacheConfiguration, string prefix, K key, IDictionary<string, V> values)
        {
            var configurationObj = JsonSerializerHelper.Deserialize<Configuration>(cacheConfiguration);
            var redisClientFactory = await _redisClientFactoryRepositoryCacheProxy.QueryByName(configurationObj.RedisClientFactoryName);
            if (redisClientFactory == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundRedisClientFactoryByName,
                    DefaultFormatting = "找不到名称为{0}的Redis客户端工厂",
                    ReplaceParameters = new List<object>() { configurationObj.RedisClientFactoryName }
                };

                throw new UtilityException((int)Errors.NotFoundRedisClientFactoryByName, fragment);
            }

            var redisClient = await redisClientFactory.GenerateClient();

            string strKey;
            if (key is string)
            {
                strKey = key.ToString();
            }
            else
            {
                strKey = JsonSerializerHelper.Serializer(key);
            }

            List<string[]> valueObjs = new List<string[]>();

            foreach(var item in values)
            {
                valueObjs.Add(new string[] { item.Key, JsonSerializerHelper.Serializer(item.Value) });
            }

            await redisClient.HMSetAsync(strKey, valueObjs);


            if (configurationObj.ExpireSeconds > 0)
            {
                var ttlResult = await redisClient.TtlAsync(strKey);
                if (ttlResult < 0)
                {
                    await redisClient.ExpireAsync(strKey, configurationObj.ExpireSeconds);
                }
            }
        }

        public void SetHashSync<K, V>(string cacheConfiguration, string prefix, K key, IDictionary<string, V> values)
        {
            var configurationObj = JsonSerializerHelper.Deserialize<Configuration>(cacheConfiguration);
            var redisClientFactory =  _redisClientFactoryRepositoryCacheProxy.QueryByNameSync(configurationObj.RedisClientFactoryName);
            if (redisClientFactory == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundRedisClientFactoryByName,
                    DefaultFormatting = "找不到名称为{0}的Redis客户端工厂",
                    ReplaceParameters = new List<object>() { configurationObj.RedisClientFactoryName }
                };

                throw new UtilityException((int)Errors.NotFoundRedisClientFactoryByName, fragment);
            }

            var redisClient =  redisClientFactory.GenerateClientSync();

            string strKey;
            if (key is string)
            {
                strKey = key.ToString();
            }
            else
            {
                strKey = JsonSerializerHelper.Serializer(key);
            }

            List<string[]> valueObjs = new List<string[]>();

            foreach (var item in values)
            {
                valueObjs.Add(new string[] { item.Key, JsonSerializerHelper.Serializer(item.Value) });
            }

             redisClient.HMSet(strKey, valueObjs);


            if (configurationObj.ExpireSeconds > 0)
            {
                var ttlResult =  redisClient.Ttl(strKey);
                if (ttlResult < 0)
                {
                     redisClient.Expire(strKey, configurationObj.ExpireSeconds);
                }
            }
        }

        public void SetSync<K, V>(string cacheConfiguration, string prefix, K key, V value)
        {
            var configurationObj = JsonSerializerHelper.Deserialize<Configuration>(cacheConfiguration);
            var redisClientFactory =  _redisClientFactoryRepositoryCacheProxy.QueryByNameSync(configurationObj.RedisClientFactoryName);
            if (redisClientFactory == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundRedisClientFactoryByName,
                    DefaultFormatting = "找不到名称为{0}的Redis客户端工厂",
                    ReplaceParameters = new List<object>() { configurationObj.RedisClientFactoryName }
                };

                throw new UtilityException((int)Errors.NotFoundRedisClientFactoryByName, fragment);
            }

            var redisClient = redisClientFactory.GenerateClientSync();

            string strKey;
            if (key is string)
            {
                strKey = key.ToString();
            }
            else
            {
                strKey = JsonSerializerHelper.Serializer(key);
            }

            var strValue = JsonSerializerHelper.Serializer(value);
            if (configurationObj.ExpireSeconds > 0)
            {
                 redisClient.Set(strKey, strValue, configurationObj.ExpireSeconds, RedisExistence.Nx);
            }
            else
            {
                 redisClient.SetNx(strKey, strValue);
            }
        }

        [DataContract]
        private class Configuration
        {
            [DataMember]
            public string RedisClientFactoryName { get; set; }

            [DataMember]
            public int ExpireSeconds { get; set; }
        }
    }
}
