using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Builder;

namespace MSLibrary.Logger.Middleware
{
    /// <summary>
    /// 中间件扩展方法
    /// </summary>
    public static class MiddlewareExtensions
    {
        /// <summary>
        /// 注册通用日志信息中间件
        /// 只有匹配起始路径的请求才需要执行该中间件
        /// </summary>
        /// <param name="app"></param>
        /// <param name="startPath">要匹配的起始路径</param>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseCommonLogInfoHandler(this IApplicationBuilder app, string startPath, string categoryName)
        {
            return app.UseWhen(context => context.Request.Path.StartsWithSegments(startPath), appBuilder =>
            {
                appBuilder.UseMiddleware<CommonLogInfoHandler>(categoryName);
            });
        }
    }
}
