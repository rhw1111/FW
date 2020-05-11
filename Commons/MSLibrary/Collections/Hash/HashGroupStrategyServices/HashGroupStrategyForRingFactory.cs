using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Collections.Hash.HashGroupStrategyServices
{
    [Injection(InterfaceType = typeof(HashGroupStrategyForRingFactory), Scope = InjectionScope.Singleton)]
    public class HashGroupStrategyForRingFactory : IFactory<IHashGroupStrategyService>
    {
        private HashGroupStrategyForRing _hashGroupStrategyForRing;
        public HashGroupStrategyForRingFactory(HashGroupStrategyForRing hashGroupStrategyForRing)
        {
            _hashGroupStrategyForRing = hashGroupStrategyForRing;
        }
        public IHashGroupStrategyService Create()
        {
            return _hashGroupStrategyForRing;
        }
    }
}
