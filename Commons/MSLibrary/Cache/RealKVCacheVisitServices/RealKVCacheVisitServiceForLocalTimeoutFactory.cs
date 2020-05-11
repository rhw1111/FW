using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Cache.RealKVCacheVisitServices
{
    [Injection(InterfaceType = typeof(RealKVCacheVisitServiceForLocalTimeoutFactory), Scope = InjectionScope.Singleton)]
    public class RealKVCacheVisitServiceForLocalTimeoutFactory : IFactory<IRealKVCacheVisitService>
    {
        private RealKVCacheVisitServiceForLocalTimeout _realKVCacheVisitServiceForLocalTimeout;

        public RealKVCacheVisitServiceForLocalTimeoutFactory(RealKVCacheVisitServiceForLocalTimeout realKVCacheVisitServiceForLocalTimeout)
        {
            _realKVCacheVisitServiceForLocalTimeout = realKVCacheVisitServiceForLocalTimeout;
        }
        public IRealKVCacheVisitService Create()
        {
            return _realKVCacheVisitServiceForLocalTimeout;
        }
    }
}
