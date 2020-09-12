using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Grpc.Core;
using Grpc.Core.Interceptors;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.Context.Application;
using MSLibrary.Logger;

namespace MSLibrary.Grpc.Interceptors
{
    /// <summary>
    /// DI包装拦截器
    /// </summary>
    public class DIWrapper: Interceptor
    {
        private readonly string _categoryName;
        private readonly ILoggerFactory _loggerFactory;

        public DIWrapper(string categoryName, ILoggerFactory loggerFactory)
        {
            _categoryName = categoryName;
            _loggerFactory = loggerFactory;
        }

        public async override Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request,ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
        {
            var logger = _loggerFactory.CreateLogger(_categoryName);
           
            TResponse response=default(TResponse);
            await GrpcErrorHelper.ExecuteByServerCallContextAsync(context, logger, async () =>
            {
                using (var diContainer = DIContainerContainer.CreateContainer())
                {
                    ContextContainer.SetValue<IDIContainer>(ContextTypes.DI, diContainer);


                    response= await continuation(request, context);

                    //从上下文中获取上下文生成结果
                    if (context.UserState.TryGetValue("AuthorizeResult", out object objResult))
                    {
                    }

                    //从上下文中获取请求上下文初始化对象列表
                    if (context.UserState.TryGetValue("ExtensionContextInits", out object objInit))
                    {
                    }

                    Task.Run(async () =>
                    {
                        if (objResult != null)
                        {
                            ((IAppUserAuthorizeResult)objResult).Execute();
                        }

                        if (objInit != null)
                        {
                            var inits = (Dictionary<string, IHttpExtensionContextInit>)objInit;
                            foreach (var item in inits)
                            {
                                item.Value.Execute();
                            }
                        }

                        var logObj = await _appHttpContextLogConvert.Convert(data);

                        LoggerHelper.LogInformation(_categoryName, logObj);

                    });
                }

            });

            return response;


        }

    }
}
