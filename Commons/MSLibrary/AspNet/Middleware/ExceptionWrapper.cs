﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using MSLibrary.Logger;
using MSLibrary.AspNet.Middleware.Application;
using MSLibrary.Context.Application;
using MSLibrary.ExceptionHandle;

namespace MSLibrary.AspNet.Middleware
{
    /// <summary>
    /// 实现对错误的包裹
    /// </summary>
    public class ExceptionWrapper
    {
        private const string _httpContextItemName = "ExtensionContextInits";
        /// <summary>
        /// 异常转换
        /// </summary>
        public static IExceptionConvert ExceptionConvert { set; get; }

        private RequestDelegate _nextMiddleware;
        private string _categoryName;
        private IAppExceptionHttpContextLogConvert _appExceptionHttpContextLogConvert;
        private bool _isDebug = false;
        private bool _isInnerService = false;

        public ExceptionWrapper(RequestDelegate nextMiddleware, string categoryName, bool isDebug,bool isInnerService, IAppExceptionHttpContextLogConvert appExceptionHttpContextLogConvert) : base()
        {
            _nextMiddleware = nextMiddleware;
            _categoryName = categoryName;
            _appExceptionHttpContextLogConvert = appExceptionHttpContextLogConvert;
            _isDebug = isDebug;
            _isInnerService = isInnerService;
        }

        public async Task Invoke(HttpContext context)
        {
            //context.Features.Get<IEndpointFeature>
            try
            {
                await _nextMiddleware.Invoke(context);
            }
            catch (Exception ex)
            {
                if (ex is UtilityException && (((UtilityException)ex).Level > 0 || _isInnerService))
                {
                    var utilityException = (UtilityException)ex;


                    object errorMessage = new ErrorMessage()
                    {
                        Level= utilityException.Level,
                        Type= utilityException.Type,
                        Code = utilityException.Code,
                        Message = await utilityException.GetCurrentLcidMessage()
                    };
                    var errorType = typeof(ErrorMessage);

                    if (ExceptionConvert != null)
                    {
                        (errorType, errorMessage) = await ExceptionConvert.Convert(utilityException);
                    }

   
                    if (!UtilityExceptionTypeStatusCodeMappings.Mappings.TryGetValue(utilityException.Type, out int statusCode))
                    {
                        statusCode = 500;
                    }

                    await context.Response.WriteJson(statusCode, errorType, errorMessage);
                }
                else
                {
                    string message;
                    if (_isDebug)
                    {
                        message = ex.ToStackTraceString();
                    }
                    else
                    {
                        message = string.Format(StringLanguageTranslate.Translate(TextCodes.InnerError, "系统内部错误，请查看系统日志"));
                    }


                    object errorMessage = new ErrorMessage()
                    {
                        Code = -1,
                        Message = message
                    };
                    var errorType = typeof(ErrorMessage);

                    if (ExceptionConvert != null)
                    {
                        (errorType, errorMessage) = await ExceptionConvert.Convert(ex);
                    }

                    await context.Response.WriteJson(StatusCodes.Status500InternalServerError, errorType, errorMessage);
                }


                //从Http上下文中获取上下文生成结果
                if (context.Items.TryGetValue("AuthorizeResult", out object objResult))
                {

                    ((IAppUserAuthorizeResult)objResult).Execute();
                }

                //从Http上下文中获取Http请求扩展上下文初始化对象
                if (context.Items.TryGetValue(_httpContextItemName, out object objInit))
                {

                    var inits = (Dictionary<string, IHttpExtensionContextInit>)objInit;
                    foreach (var item in inits)
                    {
                        item.Value.Execute();
                    }
                }





                //将异常存储在上下文的Item中
                context.Items.Add("ExecuteException", ex);

                //加到日志中
                var logObj = await _appExceptionHttpContextLogConvert.Convert(context);

                LoggerHelper.LogError(_categoryName, logObj);

                //var logger = _loggerFactory.CreateLogger(_categoryName);
                //logger.LogError($"Unhandle Error,\nmessage:{ex.Message},\nstacktrace:{ex.StackTrace}");
            }


        }

    }
}
