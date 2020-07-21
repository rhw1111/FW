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
    [Injection(InterfaceType = typeof(LabelParameterHandlerForPrintInvokeFactory), Scope = InjectionScope.Singleton)]
    public class LabelParameterHandlerForPrintInvokeFactory : IFactory<ILabelParameterHandler>
    {
        private readonly LabelParameterHandlerForPrintInvoke _labelParameterHandlerForPrintInvoke;

        public LabelParameterHandlerForPrintInvokeFactory(LabelParameterHandlerForPrintInvoke labelParameterHandlerForPrintInvoke)
        {
            _labelParameterHandlerForPrintInvoke = labelParameterHandlerForPrintInvoke;
        }

        public ILabelParameterHandler Create()
        {
            return _labelParameterHandlerForPrintInvoke;
        }
    }
}
