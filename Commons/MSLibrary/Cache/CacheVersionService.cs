using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.Redis;
using MSLibrary.LanguageTranslate;
using CSRedis;

namespace MSLibrary.Cache
{
    [Injection(InterfaceType = typeof(ICacheVersionService), Scope = InjectionScope.Singleton)]
    public class CacheVersionService : ICacheVersionService
    {
        public static string? RedisClientFactoryName { get; set; } = null;
        public static string VersionNamePrefix { get; set; } = "Version_";
        private static string _versionNameFormat = "{0}{1}";

        private readonly IRedisClientFactoryRepositoryCacheProxy _redisClientFactoryRepositoryCacheProxy;

        public CacheVersionService(IRedisClientFactoryRepositoryCacheProxy redisClientFactoryRepositoryCacheProxy)
        {
            _redisClientFactoryRepositoryCacheProxy = redisClientFactoryRepositoryCacheProxy;
        }

        public async Task Execute(string name, Func<string, Task<string>> versionQueryAction, Func<Task> changedAction)
        {
            if (RedisClientFactoryName==null)
            {
                await changedAction();
                return;
            }

            var redisClient = await getRedisClient(RedisClientFactoryName);
            var version=await redisClient.GetAsync(string.Format(_versionNameFormat, VersionNamePrefix, name));
            if (version==null)
            {
                await changedAction();
                var newVersion = await versionQueryAction(name);
                await redisClient.SetAsync(string.Format(_versionNameFormat, VersionNamePrefix, name),newVersion);
            }
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

        public async Task Clear(string name)
        {
            if (RedisClientFactoryName != null)
            {
                var redisClient = await getRedisClient(RedisClientFactoryName);
                await redisClient.DelAsync(string.Format(_versionNameFormat, VersionNamePrefix, name));
            }
        }
    }
}
