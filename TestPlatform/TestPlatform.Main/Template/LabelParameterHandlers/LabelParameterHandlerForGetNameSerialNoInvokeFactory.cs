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
    [Injection(InterfaceType = typeof(LabelParameterHandlerForGetNameSerialNoInvokeFactory), Scope = InjectionScope.Singleton)]
    public class LabelParameterHandlerForGetNameSerialNoInvokeFactory : IFactory<ILabelParameterHandler>
    {
        private readonly LabelParameterHandlerForGetNameSerialNoInvoke _labelParameterHandlerForGetNameSerialNoInvoke;

        public LabelParameterHandlerForGetNameSerialNoInvokeFactory(LabelParameterHandlerForGetNameSerialNoInvoke labelParameterHandlerForGetNameSerialNoInvoke)
        {
            _labelParameterHandlerForGetNameSerialNoInvoke = labelParameterHandlerForGetNameSerialNoInvoke;
        }

        public ILabelParameterHandler Create()
        {
            return _labelParameterHandlerForGetNameSerialNoInvoke;
        }
    }
}
