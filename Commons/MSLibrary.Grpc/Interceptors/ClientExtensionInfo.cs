using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Grpc.Core;
using Grpc.Net.Client;
using Grpc.Core.Interceptors;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using MSLibrary.Logger;
using MSLibrary.AspNet.Middleware.Application;
using MSLibrary.Context.Application;
using MSLibrary.ExceptionHandle;
using MSLibrary.Serializer;
using MSLibrary.RemoteService;
using MSLibrary.Grpc.Interceptors.Application;

namespace MSLibrary.Grpc.Interceptors
{
    /// <summary>
    /// 客户端发送时附属信息处理拦截器
    /// </summary>
    public class ClientExtensionInfo: Interceptor, IExtensionInfoinject
    {
        private readonly string _name;
        private readonly IAppClientExtensionInfoGenerate _appClientExtensionInfoGenerate;

        private object _state = string.Empty;

        public ClientExtensionInfo(string name, IAppClientExtensionInfoGenerate appClientExtensionInfoGenerate)
        {
            _name = name;
            _appClientExtensionInfoGenerate = appClientExtensionInfoGenerate;
        }

        public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(TRequest request, ClientInterceptorContext<TRequest, TResponse> context, AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
        {
            executeInfo(context);
            return continuation(request, context);
        }

        public override AsyncServerStreamingCall<TResponse> AsyncServerStreamingCall<TRequest, TResponse>(TRequest request, ClientInterceptorContext<TRequest, TResponse> context, AsyncServerStreamingCallContinuation<TRequest, TResponse> continuation)
        {
            executeInfo(context);
            return continuation(request, context);
        }

        public override AsyncClientStreamingCall<TRequest, TResponse> AsyncClientStreamingCall<TRequest, TResponse>(ClientInterceptorContext<TRequest, TResponse> context, AsyncClientStreamingCallContinuation<TRequest, TResponse> continuation)
        {
          
            executeInfo(context);
            return continuation(context);
        }

        public override AsyncDuplexStreamingCall<TRequest, TResponse> AsyncDuplexStreamingCall<TRequest, TResponse>(ClientInterceptorContext<TRequest, TResponse> context, AsyncDuplexStreamingCallContinuation<TRequest, TResponse> continuation)
        {
            executeInfo(context);
            return continuation(context);
        }

        public override TResponse BlockingUnaryCall<TRequest, TResponse>(TRequest request, ClientInterceptorContext<TRequest, TResponse> context, BlockingUnaryCallContinuation<TRequest, TResponse> continuation)
        {
            executeInfo(context);
            return continuation(request,context);
        }



        private void executeInfo<TRequest, TResponse>(ClientInterceptorContext<TRequest, TResponse> context)
            where TRequest : class
            where TResponse : class
        {
            var infos = _appClientExtensionInfoGenerate.Do(_name,_state);
            foreach (var item in infos)
            {
                context.Options.Headers.Add(item.Key, item.Value);
            }


        }

        public async Task SetData(object state)
        {
            _state = state;
            await Task.CompletedTask;
        }
    }
}
