using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Cache.RealKVCacheVisitServices
{
    [Injection(InterfaceType = typeof(RealKVCacheVisitServiceForLocalVersionFactory), Scope = InjectionScope.Singleton)]
    public class RealKVCacheVisitServiceForLocalVersionFactory : IFactory<IRealKVCacheVisitService>
    {
        private RealKVCacheVisitServiceForLocalVersion _realKVCacheVisitServiceForLocalVersion;

        public RealKVCacheVisitServiceForLocalVersionFactory(RealKVCacheVisitServiceForLocalVersion realKVCacheVisitServiceForLocalVersion)
        {
            _realKVCacheVisitServiceForLocalVersion = realKVCacheVisitServiceForLocalVersion;
        }

        public IRealKVCacheVisitService Create()
        {
            return _realKVCacheVisitServiceForLocalVersion;
        }
    }
}
