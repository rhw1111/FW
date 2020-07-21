using MSLibrary;
using MSLibrary.DI;
using System;
using System.Collections.Generic;
using System.Text;

namespace FW.TestPlatform.Main.Code.GenerateAdditionFuncServices
{
    [Injection(InterfaceType = typeof(GenerateAdditionFuncServiceForLocustPrintFactory), Scope = InjectionScope.Singleton)]
    public class GenerateAdditionFuncServiceForLocustPrintFactory : IFactory<IGenerateAdditionFuncService>
    {
        private readonly GenerateAdditionFuncServiceForLocustPrint _generateAdditionFuncServiceForLocustPrint;

        public GenerateAdditionFuncServiceForLocustPrintFactory(GenerateAdditionFuncServiceForLocustPrint generateAdditionFuncServiceForLocustPrint)
        {
            _generateAdditionFuncServiceForLocustPrint = generateAdditionFuncServiceForLocustPrint;
        }

        public IGenerateAdditionFuncService Create()
        {
            return _generateAdditionFuncServiceForLocustPrint;
        }
    }
}
