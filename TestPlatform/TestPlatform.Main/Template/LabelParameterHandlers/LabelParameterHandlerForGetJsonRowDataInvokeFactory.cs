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
    [Injection(InterfaceType = typeof(LabelParameterHandlerForGetJsonRowDataInvokeFactory), Scope = InjectionScope.Singleton)]
    public class LabelParameterHandlerForGetJsonRowDataInvokeFactory : IFactory<ILabelParameterHandler>
    {
        private readonly LabelParameterHandlerForGetJsonRowDataInvoke _labelParameterHandlerForGetJsonRowDataInvoke;

        public LabelParameterHandlerForGetJsonRowDataInvokeFactory(LabelParameterHandlerForGetJsonRowDataInvoke labelParameterHandlerForGetJsonRowDataInvoke)
        {
            _labelParameterHandlerForGetJsonRowDataInvoke = labelParameterHandlerForGetJsonRowDataInvoke;
        }

        public ILabelParameterHandler Create()
        {
            return _labelParameterHandlerForGetJsonRowDataInvoke;
        }
    }
}
