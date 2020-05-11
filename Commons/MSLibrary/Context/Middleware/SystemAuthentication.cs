using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MSLibrary;
using MSLibrary.AspNet;
using MSLibrary.LanguageTranslate;
using MSLibrary.Context.Application;
using MSLibrary.DI;

namespace MSLibrary.Context.Middleware
{
    /// <summary>
    /// 系统身份认中间件
    /// 根据传入的指定声明生成器名称执行指定的生成器，
    /// 将结果赋值Http上下文中的User属性
    /// </summary>
    public class SystemAuthentication
    {
        private RequestDelegate _nextMiddleware;
        private string _generatorName;
        private string _errorCatalogName;
        private ILoggerFactory _loggerFactory;
        private IAppSystemAuthentication _appSystemAuthentication;

        /// <summary>
        /// 传入指定的声明生成器名称
        /// 为Http上下文中的User属性赋值
        /// </summary>
        /// <param name="next"></param>
        /// <param name="generatorName">声明生成器名称</param>
        /// <param name="loggerFactory">日志工厂</param>
        /// <param name="appSystemAuthentication"></param>
        public SystemAuthentication(RequestDelegate next,string generatorName, string errorCatalogName, ILoggerFactory loggerFactory, IAppSystemAuthentication appSystemAuthentication)
        {
            _nextMiddleware = next;
            _generatorName = generatorName;
            _loggerFactory = loggerFactory;
            _appSystemAuthentication = appSystemAuthentication;
            _errorCatalogName = errorCatalogName;
        }

        public async Task Invoke(HttpContext context)
        {


            ILogger logger = null ;


            using (var diContainer = DIContainerContainer.CreateContainer())
            {
                var loggerFactory = diContainer.Get<ILoggerFactory>();
                logger = loggerFactory.CreateLogger(_errorCatalogName);
            }


            await HttpErrorHelper.ExecuteByHttpContextAsync(context,logger, async () =>
             {
                 var claimsIdentity = await _appSystemAuthentication.Do(context, _generatorName);


                 if (claimsIdentity == null)
                 {
                     context.User = null;
                 }
                 else
                 {
                     context.User = new ClaimsPrincipal(claimsIdentity);
                 }

                 await _nextMiddleware(context);
             });

          
        }
    }
}
