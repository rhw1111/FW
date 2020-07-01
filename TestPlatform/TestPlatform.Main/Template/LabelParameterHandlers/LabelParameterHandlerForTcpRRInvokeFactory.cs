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
    [Injection(InterfaceType = typeof(LabelParameterHandlerForTcpRRInvokeFactory), Scope = InjectionScope.Singleton)]
    public class LabelParameterHandlerForTcpRRInvokeFactory : IFactory<ILabelParameterHandler>
    {
        private readonly LabelParameterHandlerForTcpRRInvoke _labelParameterHandlerForTcpRRInvoke;

        public LabelParameterHandlerForTcpRRInvokeFactory(LabelParameterHandlerForTcpRRInvoke labelParameterHandlerForTcpRRInvoke)
        {
            _labelParameterHandlerForTcpRRInvoke = labelParameterHandlerForTcpRRInvoke;
        }

        public ILabelParameterHandler Create()
        {
            return _labelParameterHandlerForTcpRRInvoke;
        }
    }
}
