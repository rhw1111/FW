using MSLibrary;
using MSLibrary.DI;
using System;
using System.Collections.Generic;
using System.Text;

namespace FW.TestPlatform.Main.Code.GenerateAdditionFuncServices
{
    [Injection(InterfaceType = typeof(GenerateAdditionFuncServiceForLocustWebSocketWithConnectFactory), Scope = InjectionScope.Singleton)]
    public class GenerateAdditionFuncServiceForLocustWebSocketWithConnectFactory : IFactory<IGenerateAdditionFuncService>
    {
        private readonly GenerateAdditionFuncServiceForLocustWebSocketWithConnect _generateAdditionFuncServiceForLocustWebSocketWithConnect;

        public GenerateAdditionFuncServiceForLocustWebSocketWithConnectFactory(GenerateAdditionFuncServiceForLocustWebSocketWithConnect generateAdditionFuncServiceForLocustWebSocketWithConnect)
        {
            _generateAdditionFuncServiceForLocustWebSocketWithConnect = generateAdditionFuncServiceForLocustWebSocketWithConnect;
        }

        public IGenerateAdditionFuncService Create()
        {
            return _generateAdditionFuncServiceForLocustWebSocketWithConnect;
        }
    }
}
