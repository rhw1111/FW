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
    [Injection(InterfaceType = typeof(LabelParameterHandlerForVarKVFactory), Scope = InjectionScope.Singleton)]
    public class LabelParameterHandlerForVarKVFactory : IFactory<ILabelParameterHandler>
    {
        private readonly LabelParameterHandlerForVarKV _labelParameterHandlerForVarKV;

        public LabelParameterHandlerForVarKVFactory(LabelParameterHandlerForVarKV labelParameterHandlerForVarKV)
        {
            _labelParameterHandlerForVarKV = labelParameterHandlerForVarKV;
        }

        public ILabelParameterHandler Create()
        {
            return _labelParameterHandlerForVarKV;
        }
    }
}
