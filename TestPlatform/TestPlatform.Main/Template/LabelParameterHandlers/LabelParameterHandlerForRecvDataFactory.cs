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
    [Injection(InterfaceType = typeof(LabelParameterHandlerForRecvDataFactory), Scope = InjectionScope.Singleton)]
    public class LabelParameterHandlerForRecvDataFactory : IFactory<ILabelParameterHandler>
    {
        private readonly LabelParameterHandlerForRecvData _labelParameterHandlerForRecvData;

        public LabelParameterHandlerForRecvDataFactory(LabelParameterHandlerForRecvData labelParameterHandlerForRecvData)
        {
            _labelParameterHandlerForRecvData = labelParameterHandlerForRecvData;
        }

        public ILabelParameterHandler Create()
        {
            return _labelParameterHandlerForRecvData;
        }
    }
}
