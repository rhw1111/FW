using MSLibrary;
using MSLibrary.DI;
using System;
using System.Collections.Generic;
using System.Text;

namespace FW.TestPlatform.Main.Code.GenerateAdditionFuncServices
{
    [Injection(InterfaceType = typeof(GenerateAdditionFuncServiceForLocustHttpPostWithConnectFactory), Scope = InjectionScope.Singleton)]
    public class GenerateAdditionFuncServiceForLocustHttpPostWithConnectFactory : IFactory<IGenerateAdditionFuncService>
    {
        private readonly GenerateAdditionFuncServiceForLocustHttpPostWithConnect _generateAdditionFuncServiceForLocustHttpPostWithConnect;

        public GenerateAdditionFuncServiceForLocustHttpPostWithConnectFactory(GenerateAdditionFuncServiceForLocustHttpPostWithConnect generateAdditionFuncServiceForLocustHttpPostWithConnect)
        {
            _generateAdditionFuncServiceForLocustHttpPostWithConnect = generateAdditionFuncServiceForLocustHttpPostWithConnect;
        }

        public IGenerateAdditionFuncService Create()
        {
            return _generateAdditionFuncServiceForLocustHttpPostWithConnect;
        }
    }
}
