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
    [Injection(InterfaceType = typeof(LabelParameterHandlerForCurConnectIDFactory), Scope = InjectionScope.Singleton)]
    public class LabelParameterHandlerForCurConnectIDFactory : IFactory<ILabelParameterHandler>
    {
        private readonly LabelParameterHandlerForCurConnectID _labelParameterHandlerForCurConnectID;

        public LabelParameterHandlerForCurConnectIDFactory(LabelParameterHandlerForCurConnectID labelParameterHandlerForCurConnectID)
        {
            _labelParameterHandlerForCurConnectID = labelParameterHandlerForCurConnectID;
        }

        public ILabelParameterHandler Create()
        {
            return _labelParameterHandlerForCurConnectID;
        }
    }
}
