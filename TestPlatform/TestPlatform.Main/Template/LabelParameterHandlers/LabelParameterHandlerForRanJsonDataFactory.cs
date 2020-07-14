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
    [Injection(InterfaceType = typeof(LabelParameterHandlerForRanJsonDataFactory), Scope = InjectionScope.Singleton)]
    public class LabelParameterHandlerForRanJsonDataFactory : IFactory<ILabelParameterHandler>
    {
        private readonly LabelParameterHandlerForRanJsonData _labelParameterHandlerForRanJsonData;

        public LabelParameterHandlerForRanJsonDataFactory(LabelParameterHandlerForRanJsonData labelParameterHandlerForRanJsonData)
        {
            _labelParameterHandlerForRanJsonData = labelParameterHandlerForRanJsonData;
        }

        public ILabelParameterHandler Create()
        {
            return _labelParameterHandlerForRanJsonData;
        }
    }
}
