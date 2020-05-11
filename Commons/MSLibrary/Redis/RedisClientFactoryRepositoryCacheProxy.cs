using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.Cache;

namespace MSLibrary.Redis
{
    [Injection(InterfaceType = typeof(IRedisClientFactoryRepositoryCacheProxy), Scope = InjectionScope.Singleton)]
    public class RedisClientFactoryRepositoryCacheProxy : IRedisClientFactoryRepositoryCacheProxy
    {
        public static KVCacheVisitorSetting KVCacheVisitorSetting = new KVCacheVisitorSetting()
        {
            Name = "_RedisClientFactoryRepository",
            ExpireSeconds = -1,
            MaxLength = 1000
        };

        private IRedisClientFactoryRepository _redisClientFactoryRepository;

        private KVCacheVisitor _kvcacheVisitor;

        public RedisClientFactoryRepositoryCacheProxy(IRedisClientFactoryRepository redisClientFactoryRepository)
        {
            _redisClientFactoryRepository = redisClientFactoryRepository;
            _kvcacheVisitor = CacheInnerHelper.CreateKVCacheVisitor(KVCacheVisitorSetting);
        }


        public async Task<RedisClientFactory> QueryByName(string name)
        {
            return await _kvcacheVisitor.Get(
                async (k) =>
                {
                    return await _redisClientFactoryRepository.QueryByName(name);
                },
                name
                );
        }

        public RedisClientFactory QueryByNameSync(string name)
        {
            return _kvcacheVisitor.GetSync(
               (k) =>
              {
                  return  _redisClientFactoryRepository.QueryByNameSync(name);
              },
              name
              );
        }
    }
}
