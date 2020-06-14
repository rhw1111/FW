using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.Context;

namespace FW.TestPlatform.Main.Context.HttpExtensionContextHandleServices
{
    [Injection(InterfaceType = typeof(HttpExtensionContextHandleServiceForInternationalizationFactory), Scope = InjectionScope.Singleton)]
    public class HttpExtensionContextHandleServiceForInternationalizationFactory : IFactory<IHttpExtensionContextHandleService>
    {
        private readonly HttpExtensionContextHandleServiceForInternationalization _httpExtensionContextHandleServiceForInternationalization;

        public HttpExtensionContextHandleServiceForInternationalizationFactory(HttpExtensionContextHandleServiceForInternationalization httpExtensionContextHandleServiceForInternationalization)
        {
            _httpExtensionContextHandleServiceForInternationalization  = httpExtensionContextHandleServiceForInternationalization;
        }
        public IHttpExtensionContextHandleService Create()
        {
            return _httpExtensionContextHandleServiceForInternationalization;
        }
    }
}
