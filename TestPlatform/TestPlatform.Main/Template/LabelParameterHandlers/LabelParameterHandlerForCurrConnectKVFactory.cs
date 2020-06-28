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
    [Injection(InterfaceType = typeof(LabelParameterHandlerForCurrConnectKVFactory), Scope = InjectionScope.Singleton)]
    public class LabelParameterHandlerForCurrConnectKVFactory : IFactory<ILabelParameterHandler>
    {
        private readonly LabelParameterHandlerForCurrConnectKV _labelParameterHandlerForCurrConnectKV;

        public LabelParameterHandlerForCurrConnectKVFactory(LabelParameterHandlerForCurrConnectKV labelParameterHandlerForCurrConnectKV)
        {
            _labelParameterHandlerForCurrConnectKV = labelParameterHandlerForCurrConnectKV;
        }

        public ILabelParameterHandler Create()
        {
            return _labelParameterHandlerForCurrConnectKV;
        }
    }
}
