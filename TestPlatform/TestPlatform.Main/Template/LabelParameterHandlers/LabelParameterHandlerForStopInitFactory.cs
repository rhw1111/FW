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
    [Injection(InterfaceType = typeof(LabelParameterHandlerForStopInitFactory), Scope = InjectionScope.Singleton)]
    public class LabelParameterHandlerForStopInitFactory : IFactory<ILabelParameterHandler>
    {
        private readonly LabelParameterHandlerForStopInit _labelParameterHandlerForStopInit;

        public LabelParameterHandlerForStopInitFactory(LabelParameterHandlerForStopInit labelParameterHandlerForStopInit)
        {
            _labelParameterHandlerForStopInit = labelParameterHandlerForStopInit;
        }

        public ILabelParameterHandler Create()
        {
            return _labelParameterHandlerForStopInit;
        }
    }
}
