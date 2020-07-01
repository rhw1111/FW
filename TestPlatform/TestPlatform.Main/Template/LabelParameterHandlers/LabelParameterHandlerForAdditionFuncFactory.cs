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
    [Injection(InterfaceType = typeof(LabelParameterHandlerForAdditionFuncFactory), Scope = InjectionScope.Singleton)]
    public class LabelParameterHandlerForAdditionFuncFactory : IFactory<ILabelParameterHandler>
    {
        private readonly LabelParameterHandlerForAdditionFunc _labelParameterHandlerForAdditionFunc;

        public LabelParameterHandlerForAdditionFuncFactory(LabelParameterHandlerForAdditionFunc labelParameterHandlerForAdditionFunc)
        {
            _labelParameterHandlerForAdditionFunc = labelParameterHandlerForAdditionFunc;
        }

        public ILabelParameterHandler Create()
        {
            return _labelParameterHandlerForAdditionFunc;
        }
    }
}
