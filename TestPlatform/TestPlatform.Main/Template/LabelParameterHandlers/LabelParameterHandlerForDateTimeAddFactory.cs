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
    [Injection(InterfaceType = typeof(LabelParameterHandlerForDateTimeAddFactory), Scope = InjectionScope.Singleton)]
    public class LabelParameterHandlerForDateTimeAddFactory : IFactory<ILabelParameterHandler>
    {
        private readonly LabelParameterHandlerForDateTimeAdd _labelParameterHandlerForDateTimeAdd;

        public LabelParameterHandlerForDateTimeAddFactory(LabelParameterHandlerForDateTimeAdd labelParameterHandlerForDateTimeAdd)
        {
            _labelParameterHandlerForDateTimeAdd = labelParameterHandlerForDateTimeAdd;
        }

        public ILabelParameterHandler Create()
        {
            return _labelParameterHandlerForDateTimeAdd;
        }
    }
}
