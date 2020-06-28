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
    [Injection(InterfaceType = typeof(LabelParameterHandlerForCaseIDFactory), Scope = InjectionScope.Singleton)]
    public class LabelParameterHandlerForCaseIDFactory : IFactory<ILabelParameterHandler>
    {
        private readonly LabelParameterHandlerForCaseID _labelParameterHandlerForCaseID;

        public LabelParameterHandlerForCaseIDFactory(LabelParameterHandlerForCaseID labelParameterHandlerForCaseID)
        {
            _labelParameterHandlerForCaseID = labelParameterHandlerForCaseID;
        }

        public ILabelParameterHandler Create()
        {
            return _labelParameterHandlerForCaseID;
        }
    }
}
