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
    [Injection(InterfaceType = typeof(LabelParameterHandlerForHttpGetWithConnectInvokeFactory), Scope = InjectionScope.Singleton)]
    public class LabelParameterHandlerForHttpGetWithConnectInvokeFactory : IFactory<ILabelParameterHandler>
    {
        private readonly LabelParameterHandlerForHttpGetWithConnectInvoke _labelParameterHandlerForHttpGetWithConnectInvoke;

        public LabelParameterHandlerForHttpGetWithConnectInvokeFactory(LabelParameterHandlerForHttpGetWithConnectInvoke labelParameterHandlerForHttpGetWithConnectInvoke)
        {
            _labelParameterHandlerForHttpGetWithConnectInvoke = labelParameterHandlerForHttpGetWithConnectInvoke;
        }

        public ILabelParameterHandler Create()
        {
            return _labelParameterHandlerForHttpGetWithConnectInvoke;
        }
    }
}
