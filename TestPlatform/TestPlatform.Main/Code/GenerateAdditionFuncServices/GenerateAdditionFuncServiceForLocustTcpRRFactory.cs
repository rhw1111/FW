using MSLibrary;
using MSLibrary.DI;
using System;
using System.Collections.Generic;
using System.Text;

namespace FW.TestPlatform.Main.Code.GenerateAdditionFuncServices
{
    [Injection(InterfaceType = typeof(GenerateAdditionFuncServiceForLocustTcpRRFactory), Scope = InjectionScope.Singleton)]
    public class GenerateAdditionFuncServiceForLocustTcpRRFactory : IFactory<IGenerateAdditionFuncService>
    {
        private readonly GenerateAdditionFuncServiceForLocustTcpRR _generateAdditionFuncServiceForLocustTcpRR;

        public GenerateAdditionFuncServiceForLocustTcpRRFactory(GenerateAdditionFuncServiceForLocustTcpRR generateAdditionFuncServiceForLocustTcpRR)
        {
            _generateAdditionFuncServiceForLocustTcpRR = generateAdditionFuncServiceForLocustTcpRR;
        }

        public IGenerateAdditionFuncService Create()
        {
            return _generateAdditionFuncServiceForLocustTcpRR;
        }
    }
}
