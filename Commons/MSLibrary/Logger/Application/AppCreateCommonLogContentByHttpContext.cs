using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MSLibrary.DI;

namespace MSLibrary.Logger.Application
{
    [Injection(InterfaceType = typeof(IAppCreateCommonLogContentByHttpContext), Scope = InjectionScope.Singleton)]
    public class AppCreateCommonLogContentByHttpContext : IAppCreateCommonLogContentByHttpContext
    {
        private ICommonLogContentFactory _commonLogContentFactory;

        public AppCreateCommonLogContentByHttpContext(ICommonLogContentFactory commonLogContentFactory)
        {
            _commonLogContentFactory = commonLogContentFactory;
        }
        public async Task<CommonLogContent> Do(HttpContext context)
        {
            return await _commonLogContentFactory.CreateFromHttpContext(context);
        }
    }
}
