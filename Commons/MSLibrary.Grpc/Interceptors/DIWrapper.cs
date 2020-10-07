using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http.Extensions;
using Grpc.Core;
using Grpc.Core.Interceptors;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.Context.Application;
using MSLibrary.Logger;
using MSLibrary.AspNet.Middleware.Application;
using MSLibrary.Grpc.Interceptors.Application;
using MSLibrary.Grpc.Context.Application;

namespace MSLibrary.Grpc.Interceptors
{
    /// <summary>
    /// DI包装拦截器
    /// </summary>
    public class DIWrapper : Interceptor
    {
        private readonly string _categoryName;
        private readonly ILoggerFactory _loggerFactory;
        private IAppGrpcLogContextConvert _appGrpcLogContextConvert;
        private IAppGetLogExcludePaths _appGetLogExcludePaths;


        public DIWrapper(string categoryName, ILoggerFactory loggerFactory, IAppGrpcLogContextConvert appGrpcLogContextConvert, IAppGetLogExcludePaths appGetLogExcludePaths)
        {
            _categoryName = categoryName;
            _loggerFactory = loggerFactory;
            _appGrpcLogContextConvert = appGrpcLogContextConvert;
            _appGetLogExcludePaths = appGetLogExcludePaths;
        }

        public async override Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
        {
            TResponse response = null;

            await wrapper(context, async () =>
             {
                 response=await continuation(request, context);
             });

            if (response == null)
            {
                throw new Exception($"Response of Request {context.GetHttpContext().Request.Path} is null");
            }
            return response;


        }

        public async override Task<TResponse> ClientStreamingServerHandler<TRequest, TResponse>(IAsyncStreamReader<TRequest> requestStream, ServerCallContext context, ClientStreamingServerMethod<TRequest, TResponse> continuation)
        {
            TResponse response = null;

            await wrapper(context, async () =>
            {
                response = await continuation(requestStream, context);
            });

            if (response == null)
            {
                throw new Exception($"Response of Request {context.GetHttpContext().Request.Path} is null");
            }
            return response;
        }

        public async override Task ServerStreamingServerHandler<TRequest, TResponse>(TRequest request, IServerStreamWriter<TResponse> responseStream, ServerCallContext context, ServerStreamingServerMethod<TRequest, TResponse> continuation)
        {
            await wrapper(context, async () =>
            {
                await continuation(request,responseStream, context);
            });
        }

        public async override Task DuplexStreamingServerHandler<TRequest, TResponse>(IAsyncStreamReader<TRequest> requestStream, IServerStreamWriter<TResponse> responseStream, ServerCallContext context, DuplexStreamingServerMethod<TRequest, TResponse> continuation)
        {    
            await wrapper(context, async () =>
            {
                await continuation(requestStream, responseStream, context);
            });
        }

        private async Task wrapper(ServerCallContext context,Func<Task> action)
        {
            var logger = _loggerFactory.CreateLogger(_categoryName);
            await GrpcErrorHelper.ExecuteByServerCallContextAsync(context, logger, async () =>
            {
                using (var diContainer = DIContainerContainer.CreateContainer())
                {
                    ContextContainer.SetValue<IDIContainer>(ContextTypes.DI, diContainer);


                    Stopwatch watch = new Stopwatch();
                    watch.Start();

                    List<string> excludePaths;
                    try
                    {
                        await action();
                        excludePaths = await _appGetLogExcludePaths.Do();
                    }
                    finally
                    {
                        watch.Stop();
                    }

                    var httpContext = context.GetHttpContext();
                    bool needDo = true;
                    if (httpContext.Request.Path.HasValue)
                    {
                        //检查当前请求路径是否匹配
                        foreach (var item in excludePaths)
                        {
                            Regex regex = new Regex(item, RegexOptions.IgnoreCase);
                            if (regex.IsMatch(httpContext.Request.Path.Value))
                            {
                                needDo = false;
                                break;
                            }
                        }
                    }

                    if (needDo)
                    {
                        GrpcLogContextData data = new GrpcLogContextData(httpContext.Request.GetDisplayUrl(), httpContext.Request.Path.Value, httpContext.Request.PathBase.Value, watch.ElapsedMilliseconds);

                        //从上下文中获取上下文生成结果
                        if (context.UserState.TryGetValue("AuthorizeResult", out object? objResult))
                        {
                        }

                        //从上下文中获取请求上下文初始化对象列表
                        if (context.UserState.TryGetValue("ExtensionContextInits", out object? objInit))
                        {
                        }

                        _ = Task.Run(async () =>
                        {
                            if (objResult != null)
                            {
                                ((IAppUserAuthorizeResult)objResult).Execute();
                            }

                            if (objInit != null)
                            {
                                var inits = (Dictionary<string, IGrpcExtensionContextInit>)objInit;
                                foreach (var item in inits)
                                {
                                    item.Value.Execute();
                                }
                            }

                            var logObj = await _appGrpcLogContextConvert.Convert(data);

                            LoggerHelper.LogInformation(_categoryName, logObj);

                        });
                    }

                }

            });

        }

    }
}
