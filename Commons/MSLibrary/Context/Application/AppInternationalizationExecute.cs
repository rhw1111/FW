using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MSLibrary.DI;

namespace MSLibrary.Context.Application
{
    [Injection(InterfaceType = typeof(IAppInternationalizationExecute), Scope = InjectionScope.Singleton)]
    public class AppInternationalizationExecute : IAppInternationalizationExecute
    {
        private IInternationalizationHandleServiceFactorySelector _internationalizationHandleServiceFactorySelector;

        public AppInternationalizationExecute(IInternationalizationHandleServiceFactorySelector internationalizationHandleServiceFactorySelector)
        {
            _internationalizationHandleServiceFactorySelector = internationalizationHandleServiceFactorySelector;
        }

        public async Task<IInternationalizationContextInit> Do(HttpRequest request,string name)
        {
            var internationalizationHandleService = _internationalizationHandleServiceFactorySelector.Choose(name).Create();
            var internationalizationInfo=await internationalizationHandleService.GetInternationalizationInfo(request);

            return new InternationalizationContextInit(internationalizationHandleService, internationalizationInfo);
        }

        private class InternationalizationContextInit : IInternationalizationContextInit
        {
            private IInternationalizationHandleService _internationalizationHandleService;
            private object _info;
            public InternationalizationContextInit(IInternationalizationHandleService internationalizationHandleService,object info)
            {
                _internationalizationHandleService = internationalizationHandleService;
                _info = info;
            }
            public void Execute()
            {
                _internationalizationHandleService.GenerateContext(_info);
            }
        }
    }
}
