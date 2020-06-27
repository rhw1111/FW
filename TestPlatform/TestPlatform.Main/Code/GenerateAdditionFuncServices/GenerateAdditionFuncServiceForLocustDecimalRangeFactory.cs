using MSLibrary;
using MSLibrary.DI;
using System;
using System.Collections.Generic;
using System.Text;

namespace FW.TestPlatform.Main.Code.GenerateAdditionFuncServices
{
    [Injection(InterfaceType = typeof(GenerateAdditionFuncServiceForLocustDecimalRangeFactory), Scope = InjectionScope.Singleton)]
    public class GenerateAdditionFuncServiceForLocustDecimalRangeFactory : IFactory<IGenerateAdditionFuncService>
    {
        private readonly GenerateAdditionFuncServiceForLocustDecimalRange _generateAdditionFuncServiceForLocustDecimalRange;

        public GenerateAdditionFuncServiceForLocustDecimalRangeFactory(GenerateAdditionFuncServiceForLocustDecimalRange generateAdditionFuncServiceForLocustDecimalRange)
        {
            _generateAdditionFuncServiceForLocustDecimalRange = generateAdditionFuncServiceForLocustDecimalRange;
        }

        public IGenerateAdditionFuncService Create()
        {
            return _generateAdditionFuncServiceForLocustDecimalRange;
        }
    }
}
