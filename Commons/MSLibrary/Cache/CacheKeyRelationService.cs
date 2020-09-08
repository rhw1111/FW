using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using MSLibrary.Redis;
using CSRedis;

namespace MSLibrary.Cache
{
    [Injection(InterfaceType = typeof(ICacheKeyRelationService), Scope = InjectionScope.Singleton)]
    public class CacheKeyRelationService : ICacheKeyRelationService
    {
        public static string? RedisClientFactoryName { get; set; } = null;
        private static string _relationNameFormat = "{0}_{1}";

        private readonly IRedisClientFactoryRepositoryCacheProxy _redisClientFactoryRepositoryCacheProxy;

        public CacheKeyRelationService(IRedisClientFactoryRepositoryCacheProxy redisClientFactoryRepositoryCacheProxy)
        {
            _redisClientFactoryRepositoryCacheProxy = redisClientFactoryRepositoryCacheProxy;
        }

        public async Task AddOTN(string relationName, string oKey, string nKey)
        {
            if (RedisClientFactoryName!=null)
            {
                var redisClient = await getRedisClient(RedisClientFactoryName);
                await redisClient.SAddAsync(string.Format(_relationNameFormat, relationName, oKey), nKey);
            }
          
        }

        public async Task Delete(string relationName,string oKey)
        {
            if (RedisClientFactoryName != null)
            {
                var redisClient = await getRedisClient(RedisClientFactoryName);
                await redisClient.DelAsync(string.Format(_relationNameFormat, relationName, oKey));
            }
        }

        public async Task<IList<string>> GetNKeys(string relationName, string oKey)
        {
            List<string> result = null;
            if (RedisClientFactoryName != null)
            {
                var redisClient = await getRedisClient(RedisClientFactoryName);
                result=(await redisClient.SMembersAsync(string.Format(_relationNameFormat, relationName, oKey))).ToList();
             
            }
            if (result == null)
            {
                return new List<string>();
            }

            return result;
        }

        private async Task<CSRedisClient> getRedisClient(string name)
        {
            var clientFactory = await _redisClientFactoryRepositoryCacheProxy.QueryByName(name);
            if (clientFactory == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundRedisClientFactoryByName,
                    DefaultFormatting = "找不到名称为{0}的Redis客户端工厂",
                    ReplaceParameters = new List<object>() { name }
                };

                throw new UtilityException((int)Errors.NotFoundRedisClientFactoryByName, fragment);
            }

            return await clientFactory.GenerateClient();
        }

    }
}
