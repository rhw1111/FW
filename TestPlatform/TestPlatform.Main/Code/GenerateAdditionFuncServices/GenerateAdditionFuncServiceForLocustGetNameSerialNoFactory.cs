using MSLibrary;
using MSLibrary.DI;
using System;
using System.Collections.Generic;
using System.Text;

namespace FW.TestPlatform.Main.Code.GenerateAdditionFuncServices
{
    [Injection(InterfaceType = typeof(GenerateAdditionFuncServiceForLocustGetNameSerialNoFactory), Scope = InjectionScope.Singleton)]
    public class GenerateAdditionFuncServiceForLocustGetNameSerialNoFactory : IFactory<IGenerateAdditionFuncService>
    {
        private readonly GenerateAdditionFuncServiceForLocustGetNameSerialNo _generateAdditionFuncServiceForLocustGetNameSerialNo;

        public GenerateAdditionFuncServiceForLocustGetNameSerialNoFactory(GenerateAdditionFuncServiceForLocustGetNameSerialNo generateAdditionFuncServiceForLocustGetNameSerialNo)
        {
            _generateAdditionFuncServiceForLocustGetNameSerialNo = generateAdditionFuncServiceForLocustGetNameSerialNo;
        }

        public IGenerateAdditionFuncService Create()
        {
            return _generateAdditionFuncServiceForLocustGetNameSerialNo;
        }
    }
}
