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
    [Injection(InterfaceType = typeof(LabelParameterHandlerForFilterJsonDataInvokeFactory), Scope = InjectionScope.Singleton)]
    public class LabelParameterHandlerForFilterJsonDataInvokeFactory : IFactory<ILabelParameterHandler>
    {
        private readonly LabelParameterHandlerForFilterJsonDataInvoke _labelParameterHandlerForFilterJsonDataInvoke;

        public LabelParameterHandlerForFilterJsonDataInvokeFactory(LabelParameterHandlerForFilterJsonDataInvoke labelParameterHandlerForFilterJsonDataInvoke)
        {
            _labelParameterHandlerForFilterJsonDataInvoke = labelParameterHandlerForFilterJsonDataInvoke;
        }

        public ILabelParameterHandler Create()
        {
            return _labelParameterHandlerForFilterJsonDataInvoke;
        }
    }
}
