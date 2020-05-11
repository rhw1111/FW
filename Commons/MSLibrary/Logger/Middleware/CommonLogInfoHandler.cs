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
using MSLibrary.Logger.Application;

namespace MSLibrary.Logger.Middleware
{
    /// <summary>
    /// 通用日志信息处理中间件
    /// </summary>
    public class CommonLogInfoHandler
    {
        private RequestDelegate _nextMiddleware;
        private string _categoryName;
        private ILoggerFactory _loggerFactory;
        private IAppCommonLogInfoHttpHandle _appCommonLogInfoHttpHandle;
        private IAppCreateCommonLogRootContentByHttpContext _appCreateCommonLogRootContentByHttpContext;
        private IAppCreateCommonLogContentByHttpContext _appCreateCommonLogContentByHttpContext;

        public CommonLogInfoHandler(RequestDelegate nextMiddleware,string categoryName, ILoggerFactory loggerFactory, IAppCommonLogInfoHttpHandle appCommonLogInfoHttpHandle, IAppCreateCommonLogRootContentByHttpContext appCreateCommonLogRootContentByHttpContext, IAppCreateCommonLogContentByHttpContext appCreateCommonLogContentByHttpContext)
        {
            _nextMiddleware = nextMiddleware;
            _categoryName = categoryName;
            _loggerFactory = loggerFactory;
            _appCommonLogInfoHttpHandle = appCommonLogInfoHttpHandle;
            _appCreateCommonLogRootContentByHttpContext = appCreateCommonLogRootContentByHttpContext;
            _appCreateCommonLogContentByHttpContext = appCreateCommonLogContentByHttpContext;
        }


        public async Task Invoke(HttpContext context)
        {
            var logger = _loggerFactory.CreateLogger(_categoryName);

            await HttpErrorHelper.ExecuteByHttpContextAsync(context, logger, async () =>
            {
                //执行通用日志信息处理
                var needCreatRootLog=await _appCommonLogInfoHttpHandle.Do(context);
                //写入一条日志作为初始日志
                if (needCreatRootLog)
                {
                    var rootContent = await _appCreateCommonLogRootContentByHttpContext.Do(context);
                    LoggerHelper.LogInformation(_categoryName, rootContent);
                }
                else
                {
                    var content = await _appCreateCommonLogContentByHttpContext.Do(context);
                    LoggerHelper.LogInformation(_categoryName, content);
                }
                await _nextMiddleware(context);
            });


        }
    }
}
