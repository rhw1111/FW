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
    [Injection(InterfaceType = typeof(LabelParameterHandlerForUserNameFactory), Scope = InjectionScope.Singleton)]
    public class LabelParameterHandlerForUserNameFactory : IFactory<ILabelParameterHandler>
    {
        private readonly LabelParameterHandlerForUserName _labelParameterHandlerForUserName;

        public LabelParameterHandlerForUserNameFactory(LabelParameterHandlerForUserName labelParameterHandlerForUserName)
        {
            _labelParameterHandlerForUserName = labelParameterHandlerForUserName;
        }

        public ILabelParameterHandler Create()
        {
            return _labelParameterHandlerForUserName;
        }
    }
}
