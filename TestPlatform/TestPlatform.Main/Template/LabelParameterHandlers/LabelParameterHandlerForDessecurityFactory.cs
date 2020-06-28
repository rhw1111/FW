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
    [Injection(InterfaceType = typeof(LabelParameterHandlerForDessecurityFactory), Scope = InjectionScope.Singleton)]
    public class LabelParameterHandlerForDessecurityFactory : IFactory<ILabelParameterHandler>
    {
        private readonly LabelParameterHandlerForDessecurity _labelParameterHandlerForDessecurity;

        public LabelParameterHandlerForDessecurityFactory(LabelParameterHandlerForDessecurity labelParameterHandlerForDessecurity)
        {
            _labelParameterHandlerForDessecurity = labelParameterHandlerForDessecurity;
        }

        public ILabelParameterHandler Create()
        {
            return _labelParameterHandlerForDessecurity;
        }
    }
}
