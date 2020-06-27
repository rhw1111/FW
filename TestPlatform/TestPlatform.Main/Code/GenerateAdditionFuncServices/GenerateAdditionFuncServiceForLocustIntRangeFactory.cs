using MSLibrary;
using MSLibrary.DI;
using System;
using System.Collections.Generic;
using System.Text;

namespace FW.TestPlatform.Main.Code.GenerateAdditionFuncServices
{
    [Injection(InterfaceType = typeof(GenerateAdditionFuncServiceForLocustIntRangeFactory), Scope = InjectionScope.Singleton)]
    public class GenerateAdditionFuncServiceForLocustIntRangeFactory : IFactory<IGenerateAdditionFuncService>
    {
        private readonly GenerateAdditionFuncServiceForLocustIntRange _generateAdditionFuncServiceForLocustIntRange;

        public GenerateAdditionFuncServiceForLocustIntRangeFactory(GenerateAdditionFuncServiceForLocustIntRange generateAdditionFuncServiceForLocustIntRange)
        {
            _generateAdditionFuncServiceForLocustIntRange = generateAdditionFuncServiceForLocustIntRange;
        }

        public IGenerateAdditionFuncService Create()
        {
            return _generateAdditionFuncServiceForLocustIntRange;
        }
    }
}
