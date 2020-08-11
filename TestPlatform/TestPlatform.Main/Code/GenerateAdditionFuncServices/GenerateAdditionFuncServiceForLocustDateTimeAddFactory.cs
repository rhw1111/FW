using MSLibrary;
using MSLibrary.DI;
using System;
using System.Collections.Generic;
using System.Text;

namespace FW.TestPlatform.Main.Code.GenerateAdditionFuncServices
{
    [Injection(InterfaceType = typeof(GenerateAdditionFuncServiceForLocustDateTimeAddFactory), Scope = InjectionScope.Singleton)]
    public class GenerateAdditionFuncServiceForLocustDateTimeAddFactory : IFactory<IGenerateAdditionFuncService>
    {
        private readonly GenerateAdditionFuncServiceForLocustDateTimeAdd _generateAdditionFuncServiceForLocustDateTimeAdd;

        public GenerateAdditionFuncServiceForLocustDateTimeAddFactory(GenerateAdditionFuncServiceForLocustDateTimeAdd generateAdditionFuncServiceForLocustDateTimeAdd)
        {
            _generateAdditionFuncServiceForLocustDateTimeAdd = generateAdditionFuncServiceForLocustDateTimeAdd;
        }

        public IGenerateAdditionFuncService Create()
        {
            return _generateAdditionFuncServiceForLocustDateTimeAdd;
        }
    }
}
