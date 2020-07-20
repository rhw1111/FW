using MSLibrary;
using MSLibrary.DI;
using System;
using System.Collections.Generic;
using System.Text;

namespace FW.TestPlatform.Main.Code.GenerateAdditionFuncServices
{
    [Injection(InterfaceType = typeof(GenerateAdditionFuncServiceForLocustSplitJsonDataFactory), Scope = InjectionScope.Singleton)]
    public class GenerateAdditionFuncServiceForLocustSplitJsonDataFactory : IFactory<IGenerateAdditionFuncService>
    {
        private readonly GenerateAdditionFuncServiceForLocustSplitJsonData _generateAdditionFuncServiceForLocustSplitJsonData;

        public GenerateAdditionFuncServiceForLocustSplitJsonDataFactory(GenerateAdditionFuncServiceForLocustSplitJsonData generateAdditionFuncServiceForLocustSplitJsonData)
        {
            _generateAdditionFuncServiceForLocustSplitJsonData = generateAdditionFuncServiceForLocustSplitJsonData;
        }

        public IGenerateAdditionFuncService Create()
        {
            return _generateAdditionFuncServiceForLocustSplitJsonData;
        }
    }
}
