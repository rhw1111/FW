using MSLibrary;
using MSLibrary.DI;
using System;
using System.Collections.Generic;
using System.Text;

namespace FW.TestPlatform.Main.Code.GenerateAdditionFuncServices
{
    [Injection(InterfaceType = typeof(GenerateAdditionFuncServiceForLocustNumberFillFactory), Scope = InjectionScope.Singleton)]
    public class GenerateAdditionFuncServiceForLocustNumberFillFactory : IFactory<IGenerateAdditionFuncService>
    {
        private readonly GenerateAdditionFuncServiceForLocustNumberFill _generateAdditionFuncServiceForLocustNumberFill;

        public GenerateAdditionFuncServiceForLocustNumberFillFactory(GenerateAdditionFuncServiceForLocustNumberFill generateAdditionFuncServiceForLocustNumberFill)
        {
            _generateAdditionFuncServiceForLocustNumberFill = generateAdditionFuncServiceForLocustNumberFill;
        }

        public IGenerateAdditionFuncService Create()
        {
            return _generateAdditionFuncServiceForLocustNumberFill;
        }
    }
}
