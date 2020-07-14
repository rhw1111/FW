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
    [Injection(InterfaceType = typeof(LabelParameterHandlerForFilterJsonDataFactory), Scope = InjectionScope.Singleton)]
    public class LabelParameterHandlerForFilterJsonDataFactory : IFactory<ILabelParameterHandler>
    {
        private readonly LabelParameterHandlerForFilterJsonData _labelParameterHandlerForFilterJsonData;

        public LabelParameterHandlerForFilterJsonDataFactory(LabelParameterHandlerForFilterJsonData labelParameterHandlerForFilterJsonData)
        {
            _labelParameterHandlerForFilterJsonData = labelParameterHandlerForFilterJsonData;
        }

        public ILabelParameterHandler Create()
        {
            return _labelParameterHandlerForFilterJsonData;
        }
    }
}
