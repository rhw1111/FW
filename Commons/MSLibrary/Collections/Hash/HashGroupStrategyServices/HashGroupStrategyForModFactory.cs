using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Collections.Hash.HashGroupStrategyServices
{
    [Injection(InterfaceType = typeof(HashGroupStrategyForModFactory), Scope = InjectionScope.Singleton)]
    public class HashGroupStrategyForModFactory : IFactory<IHashGroupStrategyService>
    {
        private HashGroupStrategyForMod _hashGroupStrategyForMod;

        public HashGroupStrategyForModFactory(HashGroupStrategyForMod hashGroupStrategyForMod)
        {
            _hashGroupStrategyForMod = hashGroupStrategyForMod;
        }
        public IHashGroupStrategyService Create()
        {
            return _hashGroupStrategyForMod;
        }
    }
}
