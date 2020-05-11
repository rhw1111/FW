using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Builder;

namespace MSLibrary.Security.WhitelistPolicy.Middleware
{
    /// <summary>
    /// 中间件扩展方法
    /// </summary>
    public static class MiddlewareExtensions
    {
        /// <summary>
        ///  注册白名单身份验证中间件
        ///  只有匹配起始地址的请求才执行该中间件
        /// </summary>
        /// <param name="app"></param>
        /// <param name="startPath"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseWhitelistAuthorization(this IApplicationBuilder app,string startPath)
        {
            return app.UseWhen(context => context.Request.Path.StartsWithSegments(startPath), appBuilder =>
            {
                appBuilder.UseMiddleware<WhitelistAuthorization>();
            });
        }
    }
}
