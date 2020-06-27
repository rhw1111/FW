using MSLibrary;
using MSLibrary.DI;
using System;
using System.Collections.Generic;
using System.Text;

namespace FW.TestPlatform.Main.Code.GenerateAdditionFuncServices
{
    [Injection(InterfaceType = typeof(GenerateAdditionFuncServiceForLocustTcpRRWithConnectFactory), Scope = InjectionScope.Singleton)]
    public class GenerateAdditionFuncServiceForLocustTcpRRWithConnectFactory : IFactory<IGenerateAdditionFuncService>
    {
        private readonly GenerateAdditionFuncServiceForLocustTcpRRWithConnect _generateAdditionFuncServiceForLocustTcpRRWithConnect;

        public GenerateAdditionFuncServiceForLocustTcpRRWithConnectFactory(GenerateAdditionFuncServiceForLocustTcpRRWithConnect generateAdditionFuncServiceForLocustTcpRRWithConnect)
        {
            _generateAdditionFuncServiceForLocustTcpRRWithConnect = generateAdditionFuncServiceForLocustTcpRRWithConnect;
        }

        public IGenerateAdditionFuncService Create()
        {
            return _generateAdditionFuncServiceForLocustTcpRRWithConnect;
        }
    }
}
