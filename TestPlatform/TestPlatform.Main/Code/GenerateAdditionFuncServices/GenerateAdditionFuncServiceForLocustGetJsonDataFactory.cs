using MSLibrary;
using MSLibrary.DI;
using System;
using System.Collections.Generic;
using System.Text;

namespace FW.TestPlatform.Main.Code.GenerateAdditionFuncServices
{
    [Injection(InterfaceType = typeof(GenerateAdditionFuncServiceForLocustGetJsonDataFactory), Scope = InjectionScope.Singleton)]
    public class GenerateAdditionFuncServiceForLocustGetJsonDataFactory : IFactory<IGenerateAdditionFuncService>
    {
        private readonly GenerateAdditionFuncServiceForLocustGetJsonData _generateAdditionFuncServiceForLocustGetJsonData;

        public GenerateAdditionFuncServiceForLocustGetJsonDataFactory(GenerateAdditionFuncServiceForLocustGetJsonData generateAdditionFuncServiceForLocustGetJsonData)
        {
            _generateAdditionFuncServiceForLocustGetJsonData = generateAdditionFuncServiceForLocustGetJsonData;
        }

        public IGenerateAdditionFuncService Create()
        {
            return _generateAdditionFuncServiceForLocustGetJsonData;
        }
    }
}
