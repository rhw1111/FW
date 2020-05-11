using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MSLibrary.DI;

namespace MSLibrary.Logger.Application
{
    [Injection(InterfaceType = typeof(IAppCommonLogInfoHttpHandle), Scope = InjectionScope.Singleton)]
    public class AppCommonLogInfoHttpHandle : IAppCommonLogInfoHttpHandle
    {
        private ICommonLogInfoHttpHandleService _commonLogInfoHttpHandleService;

        public AppCommonLogInfoHttpHandle(ICommonLogInfoHttpHandleService commonLogInfoHttpHandleService)
        {
            _commonLogInfoHttpHandleService = commonLogInfoHttpHandleService;
        }

        public async Task<bool> Do(HttpContext context)
        {
            return await _commonLogInfoHttpHandleService.Execute(context);
        }
    }
}
