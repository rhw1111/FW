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
    /// 基于Http请求来生成扩展上下文的中间件
    /// </summary>
    public class HttpExtensionContext
    {

        private const string _httpContextItemName = "ExtensionContextInits";

        private RequestDelegate _nextMiddleware;
        private string _name;
        private string _errorCatalogName;
        private ILoggerFactory _loggerFactory;
        private IAppHttpExtensionContextExecute _appHttpExtensionContextExecute;

        /// <summary>
        /// 传入指定的Http请求扩展上下文服务名称
        /// </summary>
        /// <param name="next"></param>
        /// <param name="name">声明生成器名称</param>
        /// <param name="loggerFactory">日志工厂</param>
        /// <param name="appHttpExtensionContextExecute"></param>
        public HttpExtensionContext(RequestDelegate next, string name, string errorCatalogName, ILoggerFactory loggerFactory, IAppHttpExtensionContextExecute appHttpExtensionContextExecute)
        {
            _nextMiddleware = next;
            _name = name;
            _loggerFactory = loggerFactory;
            _appHttpExtensionContextExecute = appHttpExtensionContextExecute;
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
                var contextInit = await _appHttpExtensionContextExecute.Do(context.Request, _name);

                Dictionary<string, IHttpExtensionContextInit> contextInits;
                if (!context.Items.TryGetValue(_httpContextItemName, out object contextInitsObj))
                {
                    contextInits = new Dictionary<string, IHttpExtensionContextInit>();
                    context.Items[_httpContextItemName] = contextInits;
                }
                else
                {
                    contextInits = (Dictionary<string, IHttpExtensionContextInit>)contextInitsObj;
                }

                contextInits[_name] = contextInit;

                await _nextMiddleware(context);
            });



        }
    }
}
