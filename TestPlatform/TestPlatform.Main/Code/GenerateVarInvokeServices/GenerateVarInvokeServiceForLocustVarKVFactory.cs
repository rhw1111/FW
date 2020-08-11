using MSLibrary;
using MSLibrary.DI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FW.TestPlatform.Main.Code.GenerateVarInvokeServices
{
    [Injection(InterfaceType = typeof(GenerateVarInvokeServiceForLocustVarKVFactory), Scope = InjectionScope.Singleton)]
    public class GenerateVarInvokeServiceForLocustVarKVFactory : IFactory<IGenerateVarInvokeService>
    {
        private readonly GenerateVarInvokeServiceForLocustVarKV _generateVarInvokeServiceForLocustVarKV;

        public GenerateVarInvokeServiceForLocustVarKVFactory(GenerateVarInvokeServiceForLocustVarKV generateVarInvokeServiceForLocustVarKV)
        {
            _generateVarInvokeServiceForLocustVarKV = generateVarInvokeServiceForLocustVarKV;
        }

        public IGenerateVarInvokeService Create()
        {
            return _generateVarInvokeServiceForLocustVarKV;
        }
    }
}
