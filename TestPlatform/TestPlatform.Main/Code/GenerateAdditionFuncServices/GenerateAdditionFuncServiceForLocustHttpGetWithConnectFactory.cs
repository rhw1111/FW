using MSLibrary;
using MSLibrary.DI;
using System;
using System.Collections.Generic;
using System.Text;

namespace FW.TestPlatform.Main.Code.GenerateAdditionFuncServices
{
    [Injection(InterfaceType = typeof(GenerateAdditionFuncServiceForLocustHttpGetWithConnectFactory), Scope = InjectionScope.Singleton)]
    public class GenerateAdditionFuncServiceForLocustHttpGetWithConnectFactory : IFactory<IGenerateAdditionFuncService>
    {
        private readonly GenerateAdditionFuncServiceForLocustHttpGetWithConnect _generateAdditionFuncServiceForLocustHttpGetWithConnect;

        public GenerateAdditionFuncServiceForLocustHttpGetWithConnectFactory(GenerateAdditionFuncServiceForLocustHttpGetWithConnect generateAdditionFuncServiceForLocustHttpGetWithConnect)
        {
            _generateAdditionFuncServiceForLocustHttpGetWithConnect = generateAdditionFuncServiceForLocustHttpGetWithConnect;
        }

        public IGenerateAdditionFuncService Create()
        {
            return _generateAdditionFuncServiceForLocustHttpGetWithConnect;
        }
    }
}
