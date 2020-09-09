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

            var redisClient =await redisClientFactory.GenerateClient();

            string strKey = JsonSerializerHelper.Serializer(key);
            await redisClient.DelAsync(strKey);
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

            string strKey = JsonSerializerHelper.Serializer(key);
            redisClient.Del(strKey);
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

            string strKey=JsonSerializerHelper.Serializer(key);

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

            string strKey = JsonSerializerHelper.Serializer(key);

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

            string strKey = JsonSerializerHelper.Serializer(key);

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

            string strKey = JsonSerializerHelper.Serializer(key);

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
