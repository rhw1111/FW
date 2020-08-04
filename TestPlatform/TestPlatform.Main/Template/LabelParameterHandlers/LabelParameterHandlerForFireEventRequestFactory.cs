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
    [Injection(InterfaceType = typeof(LabelParameterHandlerForFireEventRequestFactory), Scope = InjectionScope.Singleton)]
    public class LabelParameterHandlerForFireEventRequestFactory : IFactory<ILabelParameterHandler>
    {
        private readonly LabelParameterHandlerForFireEventRequest _labelParameterHandlerForFireEventRequest;

        public LabelParameterHandlerForFireEventRequestFactory(LabelParameterHandlerForFireEventRequest labelParameterHandlerForFireEventRequest)
        {
            _labelParameterHandlerForFireEventRequest = labelParameterHandlerForFireEventRequest;
        }

        public ILabelParameterHandler Create()
        {
            return _labelParameterHandlerForFireEventRequest;
        }
    }
}
