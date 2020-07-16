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
    [Injection(InterfaceType = typeof(LabelParameterHandlerForGetJsonDataInvokeFactory), Scope = InjectionScope.Singleton)]
    public class LabelParameterHandlerForGetJsonDataInvokeFactory : IFactory<ILabelParameterHandler>
    {
        private readonly LabelParameterHandlerForGetJsonDataInvoke _labelParameterHandlerForGetJsonDataInvoke;

        public LabelParameterHandlerForGetJsonDataInvokeFactory(LabelParameterHandlerForGetJsonDataInvoke labelParameterHandlerForGetJsonDataInvoke)
        {
            _labelParameterHandlerForGetJsonDataInvoke = labelParameterHandlerForGetJsonDataInvoke;
        }

        public ILabelParameterHandler Create()
        {
            return _labelParameterHandlerForGetJsonDataInvoke;
        }
    }
}
