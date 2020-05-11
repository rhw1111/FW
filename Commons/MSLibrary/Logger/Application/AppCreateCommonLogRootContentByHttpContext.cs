using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MSLibrary.DI;

namespace MSLibrary.Logger.Application
{
    [Injection(InterfaceType = typeof(IAppCreateCommonLogRootContentByHttpContext), Scope = InjectionScope.Singleton)]
    public class AppCreateCommonLogRootContentByHttpContext : IAppCreateCommonLogRootContentByHttpContext
    {
        private ICommonLogRootContentFactory _commonLogRootContentFactory;

        public AppCreateCommonLogRootContentByHttpContext(ICommonLogRootContentFactory commonLogRootContentFactory)
        {
            _commonLogRootContentFactory = commonLogRootContentFactory;
        }
        public async Task<CommonLogRootContent> Do(HttpContext context)
        {
            return await _commonLogRootContentFactory.CreateFromHttpContext(context);
        }
    }
}
