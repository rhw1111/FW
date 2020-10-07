using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.RemoteService;

namespace MSLibrary.Grpc.Interceptors.Application
{
    [Injection(InterfaceType = typeof(IAppClientExtensionInfoGenerate), Scope = InjectionScope.Singleton)]
    public class AppClientExtensionInfoGenerate : IAppClientExtensionInfoGenerate
    {
        private readonly IExtensionInfoGenerateService _extensionInfoGenerateService;

        public AppClientExtensionInfoGenerate(IExtensionInfoGenerateService extensionInfoGenerateService)
        {
            _extensionInfoGenerateService = extensionInfoGenerateService;
        }
        public IDictionary<string, string> Do(string name, object state)
        {
            return  _extensionInfoGenerateService.GenerateSync(name,state);
        }
    }
}
