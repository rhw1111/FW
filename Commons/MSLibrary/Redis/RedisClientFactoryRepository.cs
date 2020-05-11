using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using MSLibrary.DI;
using MSLibrary.Redis.DAL;

namespace MSLibrary.Redis
{
    [Injection(InterfaceType = typeof(IRedisClientFactoryRepository), Scope = InjectionScope.Singleton)]
    public class RedisClientFactoryRepository : IRedisClientFactoryRepository
    {
        private IRedisClientFactoryStore _redisClientFactoryStore;

        public RedisClientFactoryRepository(IRedisClientFactoryStore redisClientFactoryStore)
        {
            _redisClientFactoryStore = redisClientFactoryStore;
        }
        public async Task<RedisClientFactory> QueryByName(string name)
        {
            return await _redisClientFactoryStore.QueryByName(name);
        }

        public RedisClientFactory QueryByNameSync(string name)
        {
            return _redisClientFactoryStore.QueryByNameSync(name);
        }
    }
}
