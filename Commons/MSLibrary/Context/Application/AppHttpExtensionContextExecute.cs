using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MSLibrary.DI;

namespace MSLibrary.Context.Application
{
    [Injection(InterfaceType = typeof(IAppHttpExtensionContextExecute), Scope = InjectionScope.Singleton)]
    public class AppHttpExtensionContextExecute : IAppHttpExtensionContextExecute
    {

        private IHttpExtensionContextHandleServiceFactorySelector _httpExtensionContextHandleServiceFactorySelector;

        public AppHttpExtensionContextExecute(IHttpExtensionContextHandleServiceFactorySelector httpExtensionContextHandleServiceFactorySelector)
        {
            _httpExtensionContextHandleServiceFactorySelector = httpExtensionContextHandleServiceFactorySelector;
        }

        public async Task<IHttpExtensionContextInit> Do(HttpRequest request, string name)
        {
            var httpExtensionContextHandleService = _httpExtensionContextHandleServiceFactorySelector.Choose(name).Create();
            var info = await httpExtensionContextHandleService.GetInfo(request);

            return new HttpExtensionContextInit(httpExtensionContextHandleService, info);
        }

        private class HttpExtensionContextInit : IHttpExtensionContextInit
        {
            private IHttpExtensionContextHandleService _httpExtensionContextHandleService;
            private object _info;
            public HttpExtensionContextInit(IHttpExtensionContextHandleService httpExtensionContextHandleService, object info)
            {
                _httpExtensionContextHandleService = httpExtensionContextHandleService;
                _info = info;
            }
            public void Execute()
            {
                _httpExtensionContextHandleService.GenerateContext(_info);
            }
        }


    }
}
