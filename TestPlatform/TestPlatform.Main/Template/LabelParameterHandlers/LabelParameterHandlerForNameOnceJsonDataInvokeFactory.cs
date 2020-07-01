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
    [Injection(InterfaceType = typeof(LabelParameterHandlerForNameOnceJsonDataInvokeFactory), Scope = InjectionScope.Singleton)]
    public class LabelParameterHandlerForNameOnceJsonDataInvokeFactory : IFactory<ILabelParameterHandler>
    {
        private readonly LabelParameterHandlerForNameOnceJsonDataInvoke _labelParameterHandlerForNameOnceJsonDataInvoke;

        public LabelParameterHandlerForNameOnceJsonDataInvokeFactory(LabelParameterHandlerForNameOnceJsonDataInvoke labelParameterHandlerForNameOnceJsonDataInvoke)
        {
            _labelParameterHandlerForNameOnceJsonDataInvoke = labelParameterHandlerForNameOnceJsonDataInvoke;
        }

        public ILabelParameterHandler Create()
        {
            return _labelParameterHandlerForNameOnceJsonDataInvoke;
        }
    }
}
