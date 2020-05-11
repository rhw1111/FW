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
    /// 国际化处理中间件
    /// 为相关上下文准备处理对象
    /// </summary>
    public class Internationalization
    {
        private RequestDelegate _nextMiddleware;
        private string _name;
        private string _errorCatalogName;
        private ILoggerFactory _loggerFactory;
        private IAppInternationalizationExecute _appInternationalizationExecute;

        /// <summary>
        /// 传入指定的国家化处理服务名称
        /// </summary>
        /// <param name="next"></param>
        /// <param name="name">声明生成器名称</param>
        /// <param name="loggerFactory">日志工厂</param>
        /// <param name="appInternationalizationExecute"></param>
        public Internationalization(RequestDelegate next, string name, string errorCatalogName, ILoggerFactory loggerFactory, IAppInternationalizationExecute appInternationalizationExecute)
        {
            _nextMiddleware = next;
            _name = name;
            _loggerFactory = loggerFactory;
            _appInternationalizationExecute = appInternationalizationExecute;
            _errorCatalogName = errorCatalogName;
        }

        public async Task Invoke(HttpContext context)
        {
            ILogger logger = null;

            using (var diContainer = DIContainerContainer.CreateContainer())
            {
                var loggerFactory = diContainer.Get<ILoggerFactory>();
                logger = loggerFactory.CreateLogger(_errorCatalogName);
            }


            await HttpErrorHelper.ExecuteByHttpContextAsync(context, logger, async () =>
            {
                var contextInit = await _appInternationalizationExecute.Do(context.Request,_name);

                context.Items["InternationalizationContextInit"] = contextInit;

                await _nextMiddleware(context);
            });



        }
    }



   
}
