using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime;
using Newtonsoft.Json.Linq;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.Template;

namespace FW.TestPlatform.Main.Template.LabelParameterHandlers
{
    [Injection(InterfaceType = typeof(LabelParameterHandlerForWebSocketWithConnectInvokeFactory), Scope = InjectionScope.Singleton)]
    public class LabelParameterHandlerForWebSocketWithConnectInvokeFactory : IFactory<ILabelParameterHandler>
    {
        private readonly LabelParameterHandlerForWebSocketWithConnectInvoke _labelParameterHandlerForWebSocketWithConnectInvoke;

        public LabelParameterHandlerForWebSocketWithConnectInvokeFactory(LabelParameterHandlerForWebSocketWithConnectInvoke labelParameterHandlerForWebSocketWithConnectInvoke)
        {
            _labelParameterHandlerForWebSocketWithConnectInvoke = labelParameterHandlerForWebSocketWithConnectInvoke;
        }

        public ILabelParameterHandler Create()
        {
            return _labelParameterHandlerForWebSocketWithConnectInvoke;
        }
    }
}
