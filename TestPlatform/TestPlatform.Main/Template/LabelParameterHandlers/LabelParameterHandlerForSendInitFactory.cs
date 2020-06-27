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
    [Injection(InterfaceType = typeof(LabelParameterHandlerForSendInitFactory), Scope = InjectionScope.Singleton)]
    public class LabelParameterHandlerForSendInitFactory : IFactory<ILabelParameterHandler>
    {
        private readonly LabelParameterHandlerForSendInit _labelParameterHandlerForSendInit;

        public LabelParameterHandlerForSendInitFactory(LabelParameterHandlerForSendInit labelParameterHandlerForSendInit)
        {
            _labelParameterHandlerForSendInit = labelParameterHandlerForSendInit;
        }

        public ILabelParameterHandler Create()
        {
            return _labelParameterHandlerForSendInit;
        }
    }
}
