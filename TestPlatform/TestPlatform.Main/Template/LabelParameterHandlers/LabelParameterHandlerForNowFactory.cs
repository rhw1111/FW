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
    [Injection(InterfaceType = typeof(LabelParameterHandlerForNowFactory), Scope = InjectionScope.Singleton)]
    public class LabelParameterHandlerForNowFactory : IFactory<ILabelParameterHandler>
    {
        private readonly LabelParameterHandlerForNow _labelParameterHandlerForNow;

        public LabelParameterHandlerForNowFactory(LabelParameterHandlerForNow labelParameterHandlerForNow)
        {
            _labelParameterHandlerForNow = labelParameterHandlerForNow;
        }

        public ILabelParameterHandler Create()
        {
            return _labelParameterHandlerForNow;
        }
    }
}
