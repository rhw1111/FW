using MSLibrary;
using MSLibrary.DI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FW.TestPlatform.Main.Code.GenerateVarInvokeServices
{
    [Injection(InterfaceType = typeof(GenerateVarInvokeServiceForLocustSleepFactory), Scope = InjectionScope.Singleton)]
    public class GenerateVarInvokeServiceForLocustSleepFactory : IFactory<IGenerateVarInvokeService>
    {
        private readonly GenerateVarInvokeServiceForLocustSleep _generateVarInvokeServiceForLocustSleep;

        public GenerateVarInvokeServiceForLocustSleepFactory(GenerateVarInvokeServiceForLocustSleep generateVarInvokeServiceForLocustSleep)
        {
            _generateVarInvokeServiceForLocustSleep = generateVarInvokeServiceForLocustSleep;
        }

        public IGenerateVarInvokeService Create()
        {
            return _generateVarInvokeServiceForLocustSleep;
        }
    }
}
