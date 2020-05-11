using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;

namespace MSLibrary.AspNet.Middleware
{
    /// <summary>
    /// 中间件扩展方法
    /// </summary>
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseDIWrapper(this IApplicationBuilder app, string diContainerContextName, string categoryName)
        {
            return app.UseMiddleware<DIWrapper>(diContainerContextName, categoryName);
        }

        public static IApplicationBuilder UseExceptionWrapper(this IApplicationBuilder app, string categoryName, bool isDebug)
        {
            return app.UseMiddleware<ExceptionWrapper>(categoryName, isDebug);
        }


    }
}
