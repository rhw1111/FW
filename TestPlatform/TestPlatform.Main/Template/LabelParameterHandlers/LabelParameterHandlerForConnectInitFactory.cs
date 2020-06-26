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
    [Injection(InterfaceType = typeof(LabelParameterHandlerForConnectInitFactory), Scope = InjectionScope.Singleton)]
    public class LabelParameterHandlerForConnectInitFactory : IFactory<ILabelParameterHandler>
    {
        private readonly LabelParameterHandlerForConnectInit _labelParameterHandlerForConnectInit;

        public LabelParameterHandlerForConnectInitFactory(LabelParameterHandlerForConnectInit labelParameterHandlerForConnectInit)
        {
            _labelParameterHandlerForConnectInit = labelParameterHandlerForConnectInit;
        }

        public ILabelParameterHandler Create()
        {
            return _labelParameterHandlerForConnectInit;
        }
    }
}
