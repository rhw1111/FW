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
    [Injection(InterfaceType = typeof(LabelParameterHandlerForSendDataFactory), Scope = InjectionScope.Singleton)]
    public class LabelParameterHandlerForSendDataFactory : IFactory<ILabelParameterHandler>
    {
        private readonly LabelParameterHandlerForSendData _labelParameterHandlerForSendData;

        public LabelParameterHandlerForSendDataFactory(LabelParameterHandlerForSendData labelParameterHandlerForSendData)
        {
            _labelParameterHandlerForSendData = labelParameterHandlerForSendData;
        }

        public ILabelParameterHandler Create()
        {
            return _labelParameterHandlerForSendData;
        }
    }
}
