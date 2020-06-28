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
    [Injection(InterfaceType = typeof(LabelParameterHandlerForDecimalRangeFactory), Scope = InjectionScope.Singleton)]
    public class LabelParameterHandlerForDecimalRangeFactory : IFactory<ILabelParameterHandler>
    {
        private readonly LabelParameterHandlerForDecimalRange _labelParameterHandlerForDecimalRange;

        public LabelParameterHandlerForDecimalRangeFactory(LabelParameterHandlerForDecimalRange labelParameterHandlerForDecimalRange)
        {
            _labelParameterHandlerForDecimalRange = labelParameterHandlerForDecimalRange;
        }

        public ILabelParameterHandler Create()
        {
            return _labelParameterHandlerForDecimalRange;
        }
    }
}
