using MSLibrary;
using MSLibrary.DI;
using System;
using System.Collections.Generic;
using System.Text;

namespace FW.TestPlatform.Main.Code.GenerateAdditionFuncServices
{
    [Injection(InterfaceType = typeof(GenerateAdditionFuncServiceForLocustGetJsonRowDataFactory), Scope = InjectionScope.Singleton)]
    public class GenerateAdditionFuncServiceForLocustGetJsonRowDataFactory : IFactory<IGenerateAdditionFuncService>
    {
        private readonly GenerateAdditionFuncServiceForLocustGetJsonRowData _generateAdditionFuncServiceForLocustGetJsonRowData;

        public GenerateAdditionFuncServiceForLocustGetJsonRowDataFactory(GenerateAdditionFuncServiceForLocustGetJsonRowData generateAdditionFuncServiceForLocustGetJsonRowData)
        {
            _generateAdditionFuncServiceForLocustGetJsonRowData = generateAdditionFuncServiceForLocustGetJsonRowData;
        }

        public IGenerateAdditionFuncService Create()
        {
            return _generateAdditionFuncServiceForLocustGetJsonRowData;
        }
    }
}
