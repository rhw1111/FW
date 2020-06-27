using MSLibrary;
using MSLibrary.DI;
using System;
using System.Collections.Generic;
using System.Text;

namespace FW.TestPlatform.Main.Code.GenerateAdditionFuncServices
{
    [Injection(InterfaceType = typeof(GenerateAdditionFuncServiceForLocustDesSecurityFactory), Scope = InjectionScope.Singleton)]
    public class GenerateAdditionFuncServiceForLocustDesSecurityFactory : IFactory<IGenerateAdditionFuncService>
    {
        private readonly GenerateAdditionFuncServiceForLocustDesSecurity _generateAdditionFuncServiceForLocustDesSecurity;

        public GenerateAdditionFuncServiceForLocustDesSecurityFactory(GenerateAdditionFuncServiceForLocustDesSecurity generateAdditionFuncServiceForLocustDesSecurity)
        {
            _generateAdditionFuncServiceForLocustDesSecurity = generateAdditionFuncServiceForLocustDesSecurity;
        }

        public IGenerateAdditionFuncService Create()
        {
            return _generateAdditionFuncServiceForLocustDesSecurity;
        }
    }
}
