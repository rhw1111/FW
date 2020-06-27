using MSLibrary;
using MSLibrary.DI;
using System;
using System.Collections.Generic;
using System.Text;

namespace FW.TestPlatform.Main.Code.GenerateAdditionFuncServices
{
    [Injection(InterfaceType = typeof(GenerateAdditionFuncServiceForLocustNameOnceJsonDataFactory), Scope = InjectionScope.Singleton)]
    public class GenerateAdditionFuncServiceForLocustNameOnceJsonDataFactory : IFactory<IGenerateAdditionFuncService>
    {
        private readonly GenerateAdditionFuncServiceForLocustNameOnceJsonData _generateAdditionFuncServiceForLocustNameOnceJsonData;

        public GenerateAdditionFuncServiceForLocustNameOnceJsonDataFactory(GenerateAdditionFuncServiceForLocustNameOnceJsonData generateAdditionFuncServiceForLocustNameOnceJsonData)
        {
            _generateAdditionFuncServiceForLocustNameOnceJsonData = generateAdditionFuncServiceForLocustNameOnceJsonData;
        }

        public IGenerateAdditionFuncService Create()
        {
            return _generateAdditionFuncServiceForLocustNameOnceJsonData;
        }
    }
}
