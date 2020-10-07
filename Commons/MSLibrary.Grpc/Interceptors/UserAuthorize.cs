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
using Grpc.Net.Client;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.Context.Application;
using MSLibrary.LanguageTranslate;
using MSLibrary.Logger;
using MSLibrary.AspNet.Middleware.Application;
using MSLibrary.Grpc.Interceptors.Application;
using MSLibrary.Grpc.Context.Application;

namespace MSLibrary.Grpc.Interceptors
{
    /// <summary>
    /// 用户授权及建立应用上下文的拦截器
    /// </summary>
    public class UserAuthorize : Interceptor
    {
        private readonly string _matchPath;
        private readonly bool _allowAnonymous;
        private readonly string _userGeneratorName;
        private readonly string _anonymousGeneratorName;
        private readonly string _directGeneratorName;
        private readonly IAppUserAuthorize _appUserAuthorize;

        public UserAuthorize(string matchPath, bool allowAnonymous, string userGeneratorName, string anonymousGeneratorName, string directGeneratorName, IAppUserAuthorize appUserAuthorize)
        {
            _matchPath = matchPath;
            _allowAnonymous = allowAnonymous;
            _userGeneratorName = userGeneratorName;
            _anonymousGeneratorName = anonymousGeneratorName;
            _directGeneratorName = directGeneratorName;
            _appUserAuthorize = appUserAuthorize;
        }

        public async override Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
        {
            TResponse response = null!;

            await auth(context, async () =>
            {
                response = await continuation(request, context);
            });

            return response;


        }

        public async override Task<TResponse> ClientStreamingServerHandler<TRequest, TResponse>(IAsyncStreamReader<TRequest> requestStream, ServerCallContext context, ClientStreamingServerMethod<TRequest, TResponse> continuation)
        {
            TResponse response = null!;

            await auth(context, async () =>
            {
                response = await continuation(requestStream, context);
            });

            return response;
        }

        public async override Task ServerStreamingServerHandler<TRequest, TResponse>(TRequest request, IServerStreamWriter<TResponse> responseStream, ServerCallContext context, ServerStreamingServerMethod<TRequest, TResponse> continuation)
        {
            await auth(context, async () =>
            {
                await continuation(request, responseStream, context);
            });
        }

        public async override Task DuplexStreamingServerHandler<TRequest, TResponse>(IAsyncStreamReader<TRequest> requestStream, IServerStreamWriter<TResponse> responseStream, ServerCallContext context, DuplexStreamingServerMethod<TRequest, TResponse> continuation)
        {
            await auth(context, async () =>
            {
                await continuation(requestStream, responseStream, context);
            });
        }



        private async Task auth(ServerCallContext context,Func<Task> action)
        {            
            IAppUserAuthorizeResult? authorizeResult = null;
            var httpContext = context.GetHttpContext();

            await context.OnceRun("UserAuthorize",
                async () =>
                {
                    if (_matchPath != null && httpContext.Request.Path.HasValue)
                    {
                        //检查当前请求路径是否匹配
                        Regex regex = new Regex(_matchPath, RegexOptions.IgnoreCase);

                        if (!regex.IsMatch(httpContext.Request.Path.Value))
                        {
                            return;
                        }
                    }



                    //如果_directGeneratorName不为空，则直接使用该生成器名称
                    if (!string.IsNullOrEmpty(_directGeneratorName))
                    {
                        authorizeResult = await _appUserAuthorize.Do(null, _directGeneratorName);
                        //存储到state上下文中
                        context.UserState["AuthorizeResult"] = authorizeResult;
                        return;
                    }

                    //判断是否已经通过验证
                    if (httpContext.User != null && httpContext.User.Identity != null && httpContext.User.Identity.IsAuthenticated && httpContext.User.Claims != null)
                    {
                        authorizeResult = await _appUserAuthorize.Do(httpContext.User.Claims, _userGeneratorName);

                        //存储到state上下文中
                        context.UserState["AuthorizeResult"] = authorizeResult;
                        //authorizeResult.Execute();
                    }
                    else
                    {
                        if (_allowAnonymous)
                        {
                            //未通过验证，但允许匿名，则调用匿名上下文生成
                            authorizeResult = await _appUserAuthorize.Do(null, _anonymousGeneratorName);
                            //存储到state上下文中
                            context.UserState["AuthorizeResult"] = authorizeResult;
                            //authorizeResult.Execute();
                        }
                        else
                        {
                            //返回错误响应

                            var fragment = new TextFragment()
                            {
                                Code = TextCodes.AuthorizeFail,
                                DefaultFormatting = "用户授权失败，没有找到对应的身份信息",
                                ReplaceParameters = new List<object>() { }
                            };

                            throw new UtilityException((int)Errors.AuthorizeFail, fragment, 1, 1);
                        }
                    }
                }
                );

            if (authorizeResult!=null)
            {
                authorizeResult.Execute();
            }

            await action();
        }

    }
}
