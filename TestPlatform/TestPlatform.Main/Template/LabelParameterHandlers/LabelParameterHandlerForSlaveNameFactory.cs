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
    [Injection(InterfaceType = typeof(LabelParameterHandlerForSlaveNameFactory), Scope = InjectionScope.Singleton)]
    public class LabelParameterHandlerForSlaveNameFactory : IFactory<ILabelParameterHandler>
    {
        private readonly LabelParameterHandlerForSlaveName _labelParameterHandlerForSlaveName;

        public LabelParameterHandlerForSlaveNameFactory(LabelParameterHandlerForSlaveName labelParameterHandlerForSlaveName)
        {
            _labelParameterHandlerForSlaveName = labelParameterHandlerForSlaveName;
        }

        public ILabelParameterHandler Create()
        {
            return _labelParameterHandlerForSlaveName;
        }
    }
}
