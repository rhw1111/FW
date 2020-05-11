using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Cache.RealKVCacheVisitServices
{
    [Injection(InterfaceType = typeof(RealKVCacheVisitServiceForCombinationFactory), Scope = InjectionScope.Singleton)]
    public class RealKVCacheVisitServiceForCombinationFactory : IFactory<IRealKVCacheVisitService>
    {
        private RealKVCacheVisitServiceForCombination _realKVCacheVisitServiceForCombination;

        public RealKVCacheVisitServiceForCombinationFactory(RealKVCacheVisitServiceForCombination realKVCacheVisitServiceForCombination)
        {
            _realKVCacheVisitServiceForCombination = realKVCacheVisitServiceForCombination;
        }
        public IRealKVCacheVisitService Create()
        {
            return _realKVCacheVisitServiceForCombination;
        }
    }
}
