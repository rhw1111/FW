using MSLibrary;
using MSLibrary.DI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FW.TestPlatform.Main.Code.GenerateFuncInvokeServices
{
    [Injection(InterfaceType = typeof(GenerateFuncInvokeServiceForLocustFactory), Scope = InjectionScope.Singleton)]
    public class GenerateFuncInvokeServiceForLocustFactory : IFactory<IGenerateFuncInvokeService>
    {
        private readonly GenerateFuncInvokeServiceForLocust _generateFuncInvokeServiceForLocust;

        public GenerateFuncInvokeServiceForLocustFactory(GenerateFuncInvokeServiceForLocust generateFuncInvokeServiceForLocust)
        {
            _generateFuncInvokeServiceForLocust = generateFuncInvokeServiceForLocust;
        }

        public IGenerateFuncInvokeService Create()
        {
            return _generateFuncInvokeServiceForLocust;
        }
    }
}
