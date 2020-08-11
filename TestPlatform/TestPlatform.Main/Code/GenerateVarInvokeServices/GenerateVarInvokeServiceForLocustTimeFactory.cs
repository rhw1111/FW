using MSLibrary;
using MSLibrary.DI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FW.TestPlatform.Main.Code.GenerateVarInvokeServices
{
    [Injection(InterfaceType = typeof(GenerateVarInvokeServiceForLocustTimeFactory), Scope = InjectionScope.Singleton)]
    public class GenerateVarInvokeServiceForLocustTimeFactory : IFactory<IGenerateVarInvokeService>
    {
        private readonly GenerateVarInvokeServiceForLocustTime _generateVarInvokeServiceForLocustTime;

        public GenerateVarInvokeServiceForLocustTimeFactory(GenerateVarInvokeServiceForLocustTime generateVarInvokeServiceForLocustTime)
        {
            _generateVarInvokeServiceForLocustTime = generateVarInvokeServiceForLocustTime;
        }

        public IGenerateVarInvokeService Create()
        {
            return _generateVarInvokeServiceForLocustTime;
        }
    }
}
