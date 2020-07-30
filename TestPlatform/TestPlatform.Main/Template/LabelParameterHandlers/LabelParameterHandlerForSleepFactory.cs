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
    [Injection(InterfaceType = typeof(LabelParameterHandlerForSleepFactory), Scope = InjectionScope.Singleton)]
    public class LabelParameterHandlerForSleepFactory : IFactory<ILabelParameterHandler>
    {
        private readonly LabelParameterHandlerForSleep _labelParameterHandlerForSleep;

        public LabelParameterHandlerForSleepFactory(LabelParameterHandlerForSleep labelParameterHandlerForSleep)
        {
            _labelParameterHandlerForSleep = labelParameterHandlerForSleep;
        }

        public ILabelParameterHandler Create()
        {
            return _labelParameterHandlerForSleep;
        }
    }
}
