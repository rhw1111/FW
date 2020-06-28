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
    [Injection(InterfaceType = typeof(LabelParameterHandlerForCurConnectFactory), Scope = InjectionScope.Singleton)]
    public class LabelParameterHandlerForCurConnectFactory : IFactory<ILabelParameterHandler>
    {
        private readonly LabelParameterHandlerForCurConnect _labelParameterHandlerForCurConnect;

        public LabelParameterHandlerForCurConnectFactory(LabelParameterHandlerForCurConnect labelParameterHandlerForCurConnect)
        {
            _labelParameterHandlerForCurConnect = labelParameterHandlerForCurConnect;
        }

        public ILabelParameterHandler Create()
        {
            return _labelParameterHandlerForCurConnect;
        }
    }
}
