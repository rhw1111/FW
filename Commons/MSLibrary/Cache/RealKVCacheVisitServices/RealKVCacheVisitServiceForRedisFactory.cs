using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Cache.RealKVCacheVisitServices
{
    [Injection(InterfaceType = typeof(RealKVCacheVisitServiceForRedisFactory), Scope = InjectionScope.Singleton)]
    public class RealKVCacheVisitServiceForRedisFactory : IFactory<IRealKVCacheVisitService>
    {
        private RealKVCacheVisitServiceForRedis _realKVCacheVisitServiceForRedis;

        public RealKVCacheVisitServiceForRedisFactory(RealKVCacheVisitServiceForRedis realKVCacheVisitServiceForRedis)
        {
            _realKVCacheVisitServiceForRedis = realKVCacheVisitServiceForRedis;
        }
        public IRealKVCacheVisitService Create()
        {
            return _realKVCacheVisitServiceForRedis;
        }
    }
}
