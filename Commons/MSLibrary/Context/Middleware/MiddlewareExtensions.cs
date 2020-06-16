using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Builder;

namespace MSLibrary.Context.Middleware
{
    /// <summary>
    /// 中间件扩展方法
    /// </summary>
    public static class MiddlewareExtensions
    {
        /// <summary>
        /// 注册系统认证中间件
        /// 只有匹配起始路径的请求才需要执行该中间件
        /// </summary>
        /// <param name="app"></param>
        /// <param name="startPath">要匹配的起始路径</param>
        /// <param name="generatorName"></param>
        /// <param name="errorCatalogName"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseSystemAuthentication(this IApplicationBuilder app,string startPath, string generatorName, string errorCatalogName)
        {
            return app.UseWhen(context => context.Request.Path.StartsWithSegments(startPath), appBuilder =>
               {
                   appBuilder.UseMiddleware<SystemAuthentication>(generatorName, errorCatalogName);
               });
        }

        /// <summary>
        /// 注册国际化中间件
        /// 只有匹配起始路径的请求才需要执行该中间件
        /// </summary>
        /// <param name="app"></param>
        /// <param name="startPath">要匹配的起始路径</param>
        /// <param name="name"></param>
        /// <param name="errorCatalogName"></param>
        /// <returns></returns>
        public static IApplicationBuilder Internationalization(this IApplicationBuilder app, string startPath, string name, string errorCatalogName)
        {
            return app.UseWhen(context => context.Request.Path.StartsWithSegments(startPath), appBuilder =>
            {
                appBuilder.UseMiddleware<Internationalization>(name, errorCatalogName);
            });
        }

        /// <summary>
        /// 注册Http扩展上下文中间件
        /// </summary>
        /// <param name="app"></param>
        /// <param name="startPath"></param>
        /// <param name="name"></param>
        /// <param name="errorCatalogName"></param>
        /// <returns></returns>
        public static IApplicationBuilder UserHttpExtensionContext(this IApplicationBuilder app, string startPath, string name, string errorCatalogName)
        {
            return app.UseWhen(context => context.Request.Path.StartsWithSegments(startPath), appBuilder =>
            {
                appBuilder.UseMiddleware<HttpExtensionContext>(name, errorCatalogName);
            });
        }

    }
}
