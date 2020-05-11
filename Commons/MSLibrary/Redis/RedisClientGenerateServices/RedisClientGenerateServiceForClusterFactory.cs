using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Redis.RedisClientGenerateServices
{
    [Injection(InterfaceType = typeof(RedisClientGenerateServiceForClusterFactory), Scope = InjectionScope.Singleton)]
    public class RedisClientGenerateServiceForClusterFactory : IFactory<IRedisClientGenerateService>
    {
        private RedisClientGenerateServiceForCluster _redisClientGenerateServiceForCluster;

        public RedisClientGenerateServiceForClusterFactory(RedisClientGenerateServiceForCluster redisClientGenerateServiceForCluster)
        {
            _redisClientGenerateServiceForCluster = redisClientGenerateServiceForCluster;
        }
        public IRedisClientGenerateService Create()
        {
            return _redisClientGenerateServiceForCluster;
        }
    }
}
