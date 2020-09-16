using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using MSLibrary.LanguageTranslate;
using MSLibrary.DI;
using MSLibrary.ExceptionHandle;

namespace MSLibrary.AspNet
{
    /// <summary>
    /// Http异常处理帮助
    /// </summary>
    public static class HttpErrorHelper
    {

        /// <summary>
        /// 异常转换
        /// </summary>
        public static IExceptionConvert ExceptionConvert { set; get; }


        /// <summary>
        /// 基于Http上下文异步处理
        /// 捕获action中的异常，
        /// 如果异常为UtilityException，则Http上下文的Response响应类型为由异常转换后的ErrorMessage的Json信息
        /// 如果不为UtilityException,则Http上下文的Response响应类型为Code=-1，Message为内部错误的ErrorMessage的Json信息
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static async Task ExecuteByHttpContextAsync(HttpContext httpContext,ILogger logger,Func<Task> action)
        {
            try
            {
                await action();
            }
            catch(UtilityException ex)
            {
                if (!UtilityExceptionTypeStatusCodeMappings.Mappings.TryGetValue(ex.Type, out int statusCode))
                {
                    statusCode = 500;
                }

                object errorMessage = new ErrorMessage()
                {
                     Type=ex.Type,
                     Level=ex.Level,
                    Code = ex.Code,
                    Message = await ex.GetCurrentLcidMessage()
                };

                var errorType = typeof(ErrorMessage);

                if (ExceptionConvert != null)
                {
                    (errorType, errorMessage) = await ExceptionConvert.Convert(ex);
                }


                await httpContext.Response.WriteJson(statusCode, errorType, errorMessage);
            }
            catch(Exception ex)
            {
                object errorMessage = new ErrorMessage()
                {
                    Code = -1,
                    Message = string.Format(StringLanguageTranslate.Translate(TextCodes.InnerError, "系统内部错误，请查看系统日志"))
                };

                var errorType = typeof(ErrorMessage);

                if (ExceptionConvert != null)
                {
                    (errorType, errorMessage) = await ExceptionConvert.Convert(ex);
                }

                await httpContext.Response.WriteJson(StatusCodes.Status500InternalServerError, errorType, errorMessage);

             
                //加入日志
                logger.Log<string>(LogLevel.Error, new EventId(), $"http request {httpContext.Request.Path} error,message:{ex.Message},stacktrace:{ex.StackTrace}",null, (obj, e) => { return string.Empty; });
            }
        }

        /// <summary>
        /// 基于授权过滤器上下文异步处理
        /// 捕获action中的异常，
        /// 如果异常为UtilityException，则Http上下文的Response响应类型为由异常转换后的ErrorMessage的Json信息
        /// 如果不为UtilityException,则Http上下文的Response响应类型为Code=-1，Message为内部错误的ErrorMessage的Json信息
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static async Task ExecuteByAuthorizationFilterContextAsync(AuthorizationFilterContext context, ILogger logger, Func<Task> action)
        {
            try
            {
                await action();
            }
            catch (UtilityException ex)
            {
                if (!UtilityExceptionTypeStatusCodeMappings.Mappings.TryGetValue(ex.Type, out int statusCode))
                {
                    statusCode = 500;
                }

                object errorMessage = new ErrorMessage()
                {
                     Type=ex.Type,
                    Level=ex.Level,
                    Code = ex.Code,
                    Message = await ex.GetCurrentLcidMessage()
                };

                var errorType = typeof(ErrorMessage);

                if (ExceptionConvert != null)
                {
                    (errorType, errorMessage) = await ExceptionConvert.Convert(ex);
                }

                context.Result= new JsonContentResult(statusCode, errorType, errorMessage);
            }
            catch (Exception ex)
            {
                string message;
            
                message = string.Format(StringLanguageTranslate.Translate(TextCodes.InnerError, "系统内部错误，请查看系统日志"));
                

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

                context.Result = new JsonContentResult(StatusCodes.Status500InternalServerError, errorType, errorMessage);
                //加入日志
                logger.Log<string>(LogLevel.Error, new EventId(), $"AuthorizationFilter in action {context.ActionDescriptor.DisplayName} error,message:{ex.Message},stacktrace:{ex.StackTrace}", null, (obj, e) => { return string.Empty; });


            }
        }



        /// <summary>
        /// 基于动作过滤器上下文异步处理
        /// 捕获action中的异常，
        /// 如果异常为UtilityException，则Http上下文的Response响应类型为由异常转换后的ErrorMessage的Json信息
        /// 如果不为UtilityException,则Http上下文的Response响应类型为Code=-1，Message为内部错误的ErrorMessage的Json信息
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static async Task ExecuteByActionExecutingFilterContextAsync(ActionExecutingContext context, ILogger logger, Func<Task> action)
        {
            try
            {
                await action();
            }
            catch (UtilityException ex)
            {
                if (!UtilityExceptionTypeStatusCodeMappings.Mappings.TryGetValue(ex.Type, out int statusCode))
                {
                    statusCode = 500;
                }

                object errorMessage = new ErrorMessage()
                {
                    Type=ex.Type,
                    Level=ex.Level,
                    Code = ex.Code,
                    Message = await ex.GetCurrentLcidMessage()
                };

                var errorType = typeof(ErrorMessage);

                if (ExceptionConvert != null)
                {
                    (errorType, errorMessage) = await ExceptionConvert.Convert(ex);
                }

                context.Result = new JsonContentResult(statusCode, errorType, errorMessage);
            }
            catch (Exception ex)
            {
                string message;

                message = string.Format(StringLanguageTranslate.Translate(TextCodes.InnerError, "系统内部错误，请查看系统日志"));


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

                context.Result = new JsonContentResult(StatusCodes.Status500InternalServerError, errorType, errorMessage);
                //加入日志
                logger.Log<string>(LogLevel.Error, new EventId(), $"ActionExecutingFilter in action {context.ActionDescriptor.DisplayName} error,message:{ex.Message},stacktrace:{ex.StackTrace}", null, (obj, e) => { return string.Empty; });
            }
        }

    }
}
