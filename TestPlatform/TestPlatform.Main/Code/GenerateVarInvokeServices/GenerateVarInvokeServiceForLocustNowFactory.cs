using MSLibrary;
using MSLibrary.DI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FW.TestPlatform.Main.Code.GenerateVarInvokeServices
{
    [Injection(InterfaceType = typeof(GenerateVarInvokeServiceForLocustNowFactory), Scope = InjectionScope.Singleton)]
    public class GenerateVarInvokeServiceForLocustNowFactory : IFactory<IGenerateVarInvokeService>
    {
        private readonly GenerateVarInvokeServiceForLocustNow _generateVarInvokeServiceForLocustNow;

        public GenerateVarInvokeServiceForLocustNowFactory(GenerateVarInvokeServiceForLocustNow generateVarInvokeServiceForLocustNow)
        {
            _generateVarInvokeServiceForLocustNow = generateVarInvokeServiceForLocustNow;
        }

        public IGenerateVarInvokeService Create()
        {
            return _generateVarInvokeServiceForLocustNow;
        }
    }
}
