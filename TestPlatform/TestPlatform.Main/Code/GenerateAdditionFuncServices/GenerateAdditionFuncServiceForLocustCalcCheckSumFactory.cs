using MSLibrary;
using MSLibrary.DI;
using System;
using System.Collections.Generic;
using System.Text;

namespace FW.TestPlatform.Main.Code.GenerateAdditionFuncServices
{
    [Injection(InterfaceType = typeof(GenerateAdditionFuncServiceForLocustCalcCheckSumFactory), Scope = InjectionScope.Singleton)]
    public class GenerateAdditionFuncServiceForLocustCalcCheckSumFactory : IFactory<IGenerateAdditionFuncService>
    {
        private readonly GenerateAdditionFuncServiceForLocustCalcCheckSum _generateAdditionFuncServiceForLocustCalcCheckSum;

        public GenerateAdditionFuncServiceForLocustCalcCheckSumFactory(GenerateAdditionFuncServiceForLocustCalcCheckSum generateAdditionFuncServiceForLocustCalcCheckSum)
        {
            _generateAdditionFuncServiceForLocustCalcCheckSum = generateAdditionFuncServiceForLocustCalcCheckSum;
        }

        public IGenerateAdditionFuncService Create()
        {
            return _generateAdditionFuncServiceForLocustCalcCheckSum;
        }
    }
}
