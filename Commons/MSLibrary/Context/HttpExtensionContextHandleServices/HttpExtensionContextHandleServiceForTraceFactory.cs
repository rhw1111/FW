using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Context.HttpExtensionContextHandleServices
{
    [Injection(InterfaceType = typeof(HttpExtensionContextHandleServiceForTraceFactory), Scope = InjectionScope.Singleton)]
    public class HttpExtensionContextHandleServiceForTraceFactory : IFactory<IHttpExtensionContextHandleService>
    {
        private readonly HttpExtensionContextHandleServiceForTrace _httpExtensionContextHandleServiceForTrace;

        public HttpExtensionContextHandleServiceForTraceFactory(HttpExtensionContextHandleServiceForTrace httpExtensionContextHandleServiceForTrace)
        {
            _httpExtensionContextHandleServiceForTrace = httpExtensionContextHandleServiceForTrace;
        }


        public IHttpExtensionContextHandleService Create()
        {
            return _httpExtensionContextHandleServiceForTrace;
        }
    }
}
