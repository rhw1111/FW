using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Grpc.Core;
using Grpc.Core.Interceptors;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using MSLibrary.Logger;
using MSLibrary.AspNet.Middleware.Application;
using MSLibrary.Context.Application;
using MSLibrary.ExceptionHandle;
using MSLibrary.Serializer;
using MSLibrary.Grpc.Interceptors.Application;
using MSLibrary.Grpc.Context.Application;

namespace MSLibrary.Grpc.Interceptors
{
    public class ExceptionWrapper: Interceptor
    {
        private const string _httpContextItemName = "ExtensionContextInits";


        private string _categoryName;
        private IAppExceptionGrpcContextLogConvert _appExceptionGrpcContextLogConvert;
        
        private bool _isDebug = false;
        private bool _isInnerService = false;

        public ExceptionWrapper( string categoryName, bool isDebug, bool isInnerService, IAppExceptionGrpcContextLogConvert appExceptionGrpcContextLogConvert) : base()
        {
        
            _categoryName = categoryName;
            _appExceptionGrpcContextLogConvert = appExceptionGrpcContextLogConvert;
            _isDebug = isDebug;
            _isInnerService = isInnerService;
        }


        public async override Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
        {
            TResponse response = null!;

            await wrapper(context, async () =>
             {
                 response = await continuation(request,context);
             });

            return response;


        }

        public async override Task<TResponse> ClientStreamingServerHandler<TRequest, TResponse>(IAsyncStreamReader<TRequest> requestStream, ServerCallContext context, ClientStreamingServerMethod<TRequest, TResponse> continuation)
        {
            TResponse response = null!;

            await wrapper(context, async () =>
            {
                response = await continuation(requestStream, context);
            });

            return response;
        }

        public async override Task ServerStreamingServerHandler<TRequest, TResponse>(TRequest request, IServerStreamWriter<TResponse> responseStream, ServerCallContext context, ServerStreamingServerMethod<TRequest, TResponse> continuation)
        {
            await wrapper(context, async () =>
            {
                await continuation(request, responseStream, context);
            });
        }

        public async override Task DuplexStreamingServerHandler<TRequest, TResponse>(IAsyncStreamReader<TRequest> requestStream, IServerStreamWriter<TResponse> responseStream, ServerCallContext context, DuplexStreamingServerMethod<TRequest, TResponse> continuation)
        {
            await wrapper(context, async () =>
            {
                await continuation(requestStream, responseStream, context);
            });
        }



        public async Task wrapper(ServerCallContext context,Func<Task> action)
        {
            try
            {
                await action();
            }
            catch (Exception ex)
            {
                RpcException filnalEx=null!;
                if (ex is RpcException)
                {
                    filnalEx = (RpcException)ex;
                }
                else
                {
                    if (ex is UtilityException && (((UtilityException)ex).Level > 0 || _isInnerService))
                    {
                        var utilityException = (UtilityException)ex;


                        if (!UtilityExceptionTypeGrpcStatusCodeMappings.Mappings.TryGetValue(utilityException.Type, out StatusCode statusCode))
                        {
                            statusCode = StatusCode.Internal;
                        }



                        ErrorMessage errorMessage = new ErrorMessage()
                        {
                            Level = utilityException.Level,
                            Type = utilityException.Type,
                            Code = utilityException.Code,
                            Message = await ex.GetCurrentLcidMessage()
                        };

                        filnalEx = new RpcException(new Status(statusCode, JsonSerializerHelper.Serializer(errorMessage)), errorMessage.Message);
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


                        filnalEx = new RpcException(new Status(StatusCode.Internal, message), message);
                    }
                }



                //从Http上下文中获取上下文生成结果
                if (context.UserState.TryGetValue("AuthorizeResult", out object? objResult))
                {

                    ((IAppUserAuthorizeResult)objResult).Execute();
                }

                //从Http上下文中获取Http请求扩展上下文初始化对象
                if (context.UserState.TryGetValue(_httpContextItemName, out object? objInit))
                {

                    var inits = (Dictionary<string, IGrpcExtensionContextInit>)objInit;
                    foreach (var item in inits)
                    {
                        item.Value.Execute();
                    }
                }


                //将异常存储在上下文的UserState中
                context.UserState.Add("ExecuteException", ex);

                //加到日志中
                var logObj = await _appExceptionGrpcContextLogConvert.Convert(context);

                LoggerHelper.LogError(_categoryName, logObj);

                throw filnalEx;
                //var logger = _loggerFactory.CreateLogger(_categoryName);
                //logger.LogError($"Unhandle Error,\nmessage:{ex.Message},\nstacktrace:{ex.StackTrace}");
            }
        }
    }
}
