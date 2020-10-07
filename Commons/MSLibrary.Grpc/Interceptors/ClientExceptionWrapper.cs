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


namespace MSLibrary.Grpc.Interceptors
{
    /// <summary>
    /// 客户端接收异常处理拦截器
    /// </summary>
    public class ClientExceptionWrapper: Interceptor
    {
        public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(TRequest request,ClientInterceptorContext<TRequest, TResponse> context,AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
        {
            AsyncUnaryCall<TResponse> response=null!;

            wrapper(() =>
            {
                response = continuation(request, context);
            });

            return response;
        }

        public override AsyncServerStreamingCall<TResponse> AsyncServerStreamingCall<TRequest, TResponse>(TRequest request, ClientInterceptorContext<TRequest, TResponse> context, AsyncServerStreamingCallContinuation<TRequest, TResponse> continuation)
        {
            AsyncServerStreamingCall<TResponse> response = null!;

            wrapper(() =>
            {
                response = continuation(request, context);
            });

            return response;
        }

        public override AsyncClientStreamingCall<TRequest, TResponse> AsyncClientStreamingCall<TRequest, TResponse>(ClientInterceptorContext<TRequest, TResponse> context, AsyncClientStreamingCallContinuation<TRequest, TResponse> continuation)
        {
            AsyncClientStreamingCall<TRequest, TResponse> response = null!;

            wrapper(() =>
            {
                response = continuation(context);
            });

            return response;
        }

        public override AsyncDuplexStreamingCall<TRequest, TResponse> AsyncDuplexStreamingCall<TRequest, TResponse>(ClientInterceptorContext<TRequest, TResponse> context, AsyncDuplexStreamingCallContinuation<TRequest, TResponse> continuation)
        {
            AsyncDuplexStreamingCall<TRequest, TResponse> response = null!;

            wrapper(() =>
            {
                response = continuation(context);
            });

            return response;
        }

        public override TResponse BlockingUnaryCall<TRequest, TResponse>(TRequest request, ClientInterceptorContext<TRequest, TResponse> context, BlockingUnaryCallContinuation<TRequest, TResponse> continuation)
        {
            TResponse response = null!;

            wrapper(() =>
            {
                response = continuation(request,context);
            });

            return response;
        }

        private void wrapper(Action action)
        {
            try
            {
                action();
            }
            catch (RpcException ex)
            {
                var errorMessage = JsonSerializerHelper.Deserialize<ErrorMessage>(ex.Status.Detail);
                if (errorMessage != null && errorMessage.Message != null)
                {
                    throw new UtilityException(errorMessage.Code, errorMessage.Message, errorMessage.Level, errorMessage.Type);
                }
                throw ex;
            }
        }
    }
}
