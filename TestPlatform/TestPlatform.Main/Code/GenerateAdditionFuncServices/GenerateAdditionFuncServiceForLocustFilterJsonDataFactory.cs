using MSLibrary;
using MSLibrary.DI;
using System;
using System.Collections.Generic;
using System.Text;

namespace FW.TestPlatform.Main.Code.GenerateAdditionFuncServices
{
    [Injection(InterfaceType = typeof(GenerateAdditionFuncServiceForLocustFilterJsonDataFactory), Scope = InjectionScope.Singleton)]
    public class GenerateAdditionFuncServiceForLocustFilterJsonDataFactory : IFactory<IGenerateAdditionFuncService>
    {
        private readonly GenerateAdditionFuncServiceForLocustFilterJsonData _generateAdditionFuncServiceForLocustFilterJsonData;

        public GenerateAdditionFuncServiceForLocustFilterJsonDataFactory(GenerateAdditionFuncServiceForLocustFilterJsonData generateAdditionFuncServiceForLocustFilterJsonData)
        {
            _generateAdditionFuncServiceForLocustFilterJsonData = generateAdditionFuncServiceForLocustFilterJsonData;
        }

        public IGenerateAdditionFuncService Create()
        {
            return _generateAdditionFuncServiceForLocustFilterJsonData;
        }
    }
}
