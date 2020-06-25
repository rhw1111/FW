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
    [Injection(InterfaceType = typeof(LabelParameterHandlerForDataVarDeclareInitFactory), Scope = InjectionScope.Singleton)]
    public class LabelParameterHandlerForDataVarDeclareInitFactory : IFactory<ILabelParameterHandler>
    {
        private readonly LabelParameterHandlerForDataVarDeclareInit _labelParameterHandlerForDataVarDeclareInit;

        public LabelParameterHandlerForDataVarDeclareInitFactory(LabelParameterHandlerForDataVarDeclareInit labelParameterHandlerForDataVarDeclareInit)
        {
            _labelParameterHandlerForDataVarDeclareInit = labelParameterHandlerForDataVarDeclareInit;
        }

        public ILabelParameterHandler Create()
        {
            return _labelParameterHandlerForDataVarDeclareInit;
        }
    }
}
