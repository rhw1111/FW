using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using MSLibrary.Logger;
using MSLibrary.AspNet.Filter.Application;
using MSLibrary.Context.Application;
using MSLibrary.ExceptionHandle;

namespace MSLibrary.AspNet.Filter
{
    /// <summary>
    /// 异常处理过滤器
    /// 将错误转换成ErrorMessage的json格式返回
    /// </summary>
    [Injection(InterfaceType = typeof(ExceptionFilter), Scope = InjectionScope.Transient)]
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        private const string _httpContextItemName = "ExtensionContextInits";
        /// <summary>
        /// 异常转换
        /// </summary>
        public static IExceptionConvert ExceptionConvert { set; get; }

        private string _categoryName;
        private bool _isDebug;
        private bool _isInnerService=false;
        private IAppExceptionContextLogConvert _appExceptionContextLogConvert;

        public ExceptionFilter(string categoryName, bool isDebug, bool isInnerService, IAppExceptionContextLogConvert appExceptionContextLogConvert) : base()
        {
            _categoryName = categoryName;
            _isDebug = isDebug;
            _isInnerService = isInnerService;
            _appExceptionContextLogConvert = appExceptionContextLogConvert;
        }

        public override async Task OnExceptionAsync(ExceptionContext context)
        {
            if (context.Exception != null)
            {
                if (context.HttpContext.Items.TryGetValue("AuthorizeResult", out object objResult))
                {
                    ((IAppUserAuthorizeResult)objResult).Execute();
                }


                //从Http上下文中获取http请求上下文初始化对象列表
                if (context.HttpContext.Items.TryGetValue(_httpContextItemName, out object objInit))
                {
                    var inits = (Dictionary<string, IHttpExtensionContextInit>)objInit;
                    foreach (var item in inits)
                    {
                        item.Value.Execute();
                    }
                }


                if (context.Exception is UtilityException && (((UtilityException)context.Exception).Level > 0 || _isInnerService))
                {
                    var utilityException = (UtilityException)context.Exception;
                    object errorMessage = new ErrorMessage()
                    {
                        Level= utilityException.Level,
                         Type= utilityException.Type,
                        Code = utilityException.Code,
                        Message = await utilityException.GetCurrentLcidMessage()
                    };
                    var errorType = typeof(ErrorMessage);

                    if (ExceptionConvert!=null)
                    {
                        (errorType,errorMessage) =await ExceptionConvert.Convert(utilityException);
                    }

                    /*ErrorMessage errorMessage = new ErrorMessage()
                    {
                        Code = utilityException.Code,
                        Message = await utilityException.GetCurrentLcidMessage()
                    };*/

                    if (!UtilityExceptionTypeStatusCodeMappings.Mappings.TryGetValue(utilityException.Type,out int statusCode))
                    {
                        statusCode = 500;
                    }

                    context.Result = new JsonContentResult(statusCode, errorType, errorMessage);
                }
                else
                {
                    string message;
                    if (_isDebug)
                    {
                        message = context.Exception.ToStackTraceString();
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
                        (errorType, errorMessage) = await ExceptionConvert.Convert(context.Exception);
                    }


            
                    context.Result = new JsonContentResult(StatusCodes.Status500InternalServerError, errorType, errorMessage);
                }

                //加到日志中


                var logObj = await _appExceptionContextLogConvert.Do(context);


                LoggerHelper.LogError(_categoryName, logObj);


                //logger.Log(LogLevel.Error,new EventId(),logObj, context.Exception,)

                //logger.LogError($"Action {context.ActionDescriptor.DisplayName} error,\nmessage:{context.Exception.Message},\nstacktrace:{context.Exception.StackTrace}");
            }
        }



    }
}
