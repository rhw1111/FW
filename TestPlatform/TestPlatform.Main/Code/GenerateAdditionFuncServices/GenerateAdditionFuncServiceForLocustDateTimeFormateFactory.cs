using MSLibrary;
using MSLibrary.DI;
using System;
using System.Collections.Generic;
using System.Text;

namespace FW.TestPlatform.Main.Code.GenerateAdditionFuncServices
{
    [Injection(InterfaceType = typeof(GenerateAdditionFuncServiceForLocustDateTimeFormateFactory), Scope = InjectionScope.Singleton)]
    public class GenerateAdditionFuncServiceForLocustDateTimeFormateFactory : IFactory<IGenerateAdditionFuncService>
    {
        private readonly GenerateAdditionFuncServiceForLocustDateTimeFormate _generateAdditionFuncServiceForLocustDateTimeFormate;

        public GenerateAdditionFuncServiceForLocustDateTimeFormateFactory(GenerateAdditionFuncServiceForLocustDateTimeFormate generateAdditionFuncServiceForLocustDateTimeFormate)
        {
            _generateAdditionFuncServiceForLocustDateTimeFormate = generateAdditionFuncServiceForLocustDateTimeFormate;
        }

        public IGenerateAdditionFuncService Create()
        {
            return _generateAdditionFuncServiceForLocustDateTimeFormate;
        }
    }
}
