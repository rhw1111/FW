using MSLibrary;
using MSLibrary.DI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FW.TestPlatform.Main.Code.GenerateVarInvokeServices
{
    [Injection(InterfaceType = typeof(GenerateVarInvokeServiceForLocustFactory), Scope = InjectionScope.Singleton)]
    public class GenerateVarInvokeServiceForLocustFactory : IFactory<IGenerateVarInvokeService>
    {
        private readonly GenerateVarInvokeServiceForLocust _generateVarInvokeServiceForLocust;

        public GenerateVarInvokeServiceForLocustFactory(GenerateVarInvokeServiceForLocust generateVarInvokeServiceForLocust)
        {
            _generateVarInvokeServiceForLocust = generateVarInvokeServiceForLocust;
        }

        public IGenerateVarInvokeService Create()
        {
            return _generateVarInvokeServiceForLocust;
        }
    }
}
