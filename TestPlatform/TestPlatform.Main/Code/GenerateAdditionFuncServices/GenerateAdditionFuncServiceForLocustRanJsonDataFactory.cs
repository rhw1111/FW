using MSLibrary;
using MSLibrary.DI;
using System;
using System.Collections.Generic;
using System.Text;

namespace FW.TestPlatform.Main.Code.GenerateAdditionFuncServices
{
    [Injection(InterfaceType = typeof(GenerateAdditionFuncServiceForLocustRanJsonDataFactory), Scope = InjectionScope.Singleton)]
    public class GenerateAdditionFuncServiceForLocustRanJsonDataFactory : IFactory<IGenerateAdditionFuncService>
    {
        private readonly GenerateAdditionFuncServiceForLocustRanJsonData _generateAdditionFuncServiceForLocustRanJsonData;

        public GenerateAdditionFuncServiceForLocustRanJsonDataFactory(GenerateAdditionFuncServiceForLocustRanJsonData generateAdditionFuncServiceForLocustRanJsonData)
        {
            _generateAdditionFuncServiceForLocustRanJsonData = generateAdditionFuncServiceForLocustRanJsonData;
        }

        public IGenerateAdditionFuncService Create()
        {
            return _generateAdditionFuncServiceForLocustRanJsonData;
        }
    }
}
