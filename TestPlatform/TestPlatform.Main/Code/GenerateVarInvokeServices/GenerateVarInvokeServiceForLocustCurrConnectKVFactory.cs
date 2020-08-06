using MSLibrary;
using MSLibrary.DI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FW.TestPlatform.Main.Code.GenerateVarInvokeServices
{
    [Injection(InterfaceType = typeof(GenerateVarInvokeServiceForLocustCurrConnectKVFactory), Scope = InjectionScope.Singleton)]
    public class GenerateVarInvokeServiceForLocustCurrConnectKVFactory : IFactory<IGenerateVarInvokeService>
    {
        private readonly GenerateVarInvokeServiceForLocustCurrConnectKV _generateVarInvokeServiceForLocustCurrConnectKV;

        public GenerateVarInvokeServiceForLocustCurrConnectKVFactory(GenerateVarInvokeServiceForLocustCurrConnectKV generateVarInvokeServiceForLocustCurrConnectKV)
        {
            _generateVarInvokeServiceForLocustCurrConnectKV = generateVarInvokeServiceForLocustCurrConnectKV;
        }

        public IGenerateVarInvokeService Create()
        {
            return _generateVarInvokeServiceForLocustCurrConnectKV;
        }
    }
}
