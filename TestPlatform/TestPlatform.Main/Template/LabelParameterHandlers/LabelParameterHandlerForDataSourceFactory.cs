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
    [Injection(InterfaceType = typeof(LabelParameterHandlerForDataSourceFactory), Scope = InjectionScope.Singleton)]
    public class LabelParameterHandlerForDataSourceFactory : IFactory<ILabelParameterHandler>
    {
        private readonly LabelParameterHandlerForDataSource _labelParameterHandlerForDataSource;

        public LabelParameterHandlerForDataSourceFactory(LabelParameterHandlerForDataSource labelParameterHandlerForDataSource)
        {
            _labelParameterHandlerForDataSource = labelParameterHandlerForDataSource;
        }

        public ILabelParameterHandler Create()
        {
            return _labelParameterHandlerForDataSource;
        }
    }
}
