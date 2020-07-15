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
    [Injection(InterfaceType = typeof(LabelParameterHandlerForCalcCheckSumInvokeFactory), Scope = InjectionScope.Singleton)]
    public class LabelParameterHandlerForCalcCheckSumInvokeFactory : IFactory<ILabelParameterHandler>
    {
        private readonly LabelParameterHandlerForCalcCheckSumInvoke _labelParameterHandlerForCalcCheckSumInvoke;

        public LabelParameterHandlerForCalcCheckSumInvokeFactory(LabelParameterHandlerForCalcCheckSumInvoke labelParameterHandlerForCalcCheckSumInvoke)
        {
            _labelParameterHandlerForCalcCheckSumInvoke = labelParameterHandlerForCalcCheckSumInvoke;
        }

        public ILabelParameterHandler Create()
        {
            return _labelParameterHandlerForCalcCheckSumInvoke;
        }
    }
}
