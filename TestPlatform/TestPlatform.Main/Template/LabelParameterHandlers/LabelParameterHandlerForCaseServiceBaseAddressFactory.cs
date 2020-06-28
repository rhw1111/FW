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
    [Injection(InterfaceType = typeof(LabelParameterHandlerForCaseServiceBaseAddressFactory), Scope = InjectionScope.Singleton)]
    public class LabelParameterHandlerForCaseServiceBaseAddressFactory : IFactory<ILabelParameterHandler>
    {
        private readonly LabelParameterHandlerForCaseServiceBaseAddress _labelParameterHandlerForCaseServiceBaseAddress;

        public LabelParameterHandlerForCaseServiceBaseAddressFactory(LabelParameterHandlerForCaseServiceBaseAddress labelParameterHandlerForCaseServiceBaseAddress)
        {
            _labelParameterHandlerForCaseServiceBaseAddress = labelParameterHandlerForCaseServiceBaseAddress;
        }

        public ILabelParameterHandler Create()
        {
            return _labelParameterHandlerForCaseServiceBaseAddress;
        }
    }
}
