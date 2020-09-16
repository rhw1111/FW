using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Grpc.Core;
using Grpc.Core.Utils;
using Grpc.Core.Interceptors;
using MSLibrary;
using MSLibrary.LanguageTranslate;
using MSLibrary.Serializer;

namespace MSLibrary.Grpc
{
    public class GrpcErrorHelper
    {


        /// <summary>
        /// 基于ServerCall上下文异步处理，
        /// 如果异常为UtilityException，则抛出message为该异常message的RpcException
        /// 如果异常为RpcException，则直接抛出原样异常
        /// 如果为其他Exception,则抛出message为内部错误的RpcException
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static async Task ExecuteByServerCallContextAsync(ServerCallContext callContext, ILogger logger, Func<Task> action)
        {
            
            try
            {
                await action();
            }
            catch (UtilityException ex)
            {
                if (!UtilityExceptionTypeGrpcStatusCodeMappings.Mappings.TryGetValue(ex.Type, out StatusCode statusCode))
                {
                    statusCode = StatusCode.Internal;
                }



                ErrorMessage errorMessage = new ErrorMessage()
                {
                    Level = ex.Level,
                    Type = ex.Type,
                    Code = ex.Code,
                    Message = await ex.GetCurrentLcidMessage()
                };
            
                throw new RpcException(new Status(statusCode, JsonSerializerHelper.Serializer(errorMessage)), errorMessage.Message);
            }
            catch(RpcException)
            {
                throw;
            }
            catch (Exception ex)
            {

                var errorMessage = string.Format(StringLanguageTranslate.Translate(TextCodes.InnerError, "系统内部错误，请查看系统日志"));

                var httpContext = callContext.GetHttpContext();

                //加入日志
                logger.Log<string>(LogLevel.Error, new EventId(), string.Empty, null, (obj, e) => { return $"http request {httpContext.Request.Path} error,message:{ex.Message},stacktrace:{ex.StackTrace}"; });

                throw new RpcException(new Status(StatusCode.Internal, errorMessage), errorMessage);
            }
        }



    }
}
