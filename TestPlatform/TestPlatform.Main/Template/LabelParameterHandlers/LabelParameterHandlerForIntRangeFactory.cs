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
    [Injection(InterfaceType = typeof(LabelParameterHandlerForIntRangeFactory), Scope = InjectionScope.Singleton)]
    public class LabelParameterHandlerForIntRangeFactory : IFactory<ILabelParameterHandler>
    {
        private readonly LabelParameterHandlerForIntRange _labelParameterHandlerForIntRange;

        public LabelParameterHandlerForIntRangeFactory(LabelParameterHandlerForIntRange labelParameterHandlerForIntRange)
        {
            _labelParameterHandlerForIntRange = labelParameterHandlerForIntRange;
        }

        public ILabelParameterHandler Create()
        {
            return _labelParameterHandlerForIntRange;
        }
    }
}
