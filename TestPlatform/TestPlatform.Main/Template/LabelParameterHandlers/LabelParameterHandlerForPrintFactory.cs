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
    [Injection(InterfaceType = typeof(LabelParameterHandlerForPrintFactory), Scope = InjectionScope.Singleton)]
    public class LabelParameterHandlerForPrintFactory : IFactory<ILabelParameterHandler>
    {
        private readonly LabelParameterHandlerForPrint _labelParameterHandlerForPrint;

        public LabelParameterHandlerForPrintFactory(LabelParameterHandlerForPrint labelParameterHandlerForPrint)
        {
            _labelParameterHandlerForPrint = labelParameterHandlerForPrint;
        }

        public ILabelParameterHandler Create()
        {
            return _labelParameterHandlerForPrint;
        }
    }
}
