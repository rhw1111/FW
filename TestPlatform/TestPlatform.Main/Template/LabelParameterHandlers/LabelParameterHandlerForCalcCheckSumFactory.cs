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
    [Injection(InterfaceType = typeof(LabelParameterHandlerForCalcCheckSumFactory), Scope = InjectionScope.Singleton)]
    public class LabelParameterHandlerForCalcCheckSumFactory : IFactory<ILabelParameterHandler>
    {
        private readonly LabelParameterHandlerForCalcCheckSum _labelParameterHandlerForCalcCheckSum;

        public LabelParameterHandlerForCalcCheckSumFactory(LabelParameterHandlerForCalcCheckSum labelParameterHandlerForCalcCheckSum)
        {
            _labelParameterHandlerForCalcCheckSum = labelParameterHandlerForCalcCheckSum;
        }

        public ILabelParameterHandler Create()
        {
            return _labelParameterHandlerForCalcCheckSum;
        }
    }
}
