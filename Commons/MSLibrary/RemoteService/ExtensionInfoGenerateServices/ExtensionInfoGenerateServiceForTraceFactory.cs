using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.RemoteService.ExtensionInfoGenerateServices
{
    [Injection(InterfaceType = typeof(ExtensionInfoGenerateServiceForTraceFactory), Scope = InjectionScope.Singleton)]
    public class ExtensionInfoGenerateServiceForTraceFactory : IFactory<IExtensionInfoGenerateService>
    {
        private readonly ExtensionInfoGenerateServiceForTrace _extensionInfoGenerateServiceForTrace;

        public ExtensionInfoGenerateServiceForTraceFactory(ExtensionInfoGenerateServiceForTrace extensionInfoGenerateServiceForTrace)
        {
            _extensionInfoGenerateServiceForTrace = extensionInfoGenerateServiceForTrace;
        }
        public IExtensionInfoGenerateService Create()
        {
            return _extensionInfoGenerateServiceForTrace;
        }
    }
}
