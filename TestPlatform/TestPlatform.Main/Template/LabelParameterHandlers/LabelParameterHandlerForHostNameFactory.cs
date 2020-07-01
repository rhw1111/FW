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
    [Injection(InterfaceType = typeof(LabelParameterHandlerForHostNameFactory), Scope = InjectionScope.Singleton)]
    public class LabelParameterHandlerForHostNameFactory : IFactory<ILabelParameterHandler>
    {
        private readonly LabelParameterHandlerForHostName _labelParameterHandlerForHostName;

        public LabelParameterHandlerForHostNameFactory(LabelParameterHandlerForHostName labelParameterHandlerForHostName)
        {
            _labelParameterHandlerForHostName = labelParameterHandlerForHostName;
        }

        public ILabelParameterHandler Create()
        {
            return _labelParameterHandlerForHostName;
        }
    }
}
