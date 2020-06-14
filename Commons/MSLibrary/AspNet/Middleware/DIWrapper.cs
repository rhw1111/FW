using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http.Extensions;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.Logger;
using MSLibrary.AspNet.Middleware.Application;
using MSLibrary.Context.Application;
using MSLibrary.Context;

namespace MSLibrary.AspNet.Middleware
{
    /// <summary>
    /// 实现DI容器的包裹
    /// 同时实现日志的记录
    /// </summary>
    public class DIWrapper
    {
        private const string _httpContextItemName = "ExtensionContextInits";

        private RequestDelegate _nextMiddleware;
        private string _name;
        private string _categoryName;
        private ILoggerFactory _loggerFactory;
        private IAppHttpContextLogConvert _appHttpContextLogConvert;
        private IAppGetLogExcludePaths _appGetLogExcludePaths;
        private IAppGetOutputStreamReplaceExcludePaths _appGetOutputStreamReplaceExcludePaths;

        /// <summary>
        /// 传入指定上下文容器名称作为DI上下文容器的名称
        /// </summary>
        /// <param name="next"></param>
        /// <param name="name"></param>
        public DIWrapper(RequestDelegate next, string name, string categoryName, ILoggerFactory loggerFactory, IAppHttpContextLogConvert appHttpContextLogConvert, IAppGetLogExcludePaths appGetLogExcludePaths, IAppGetOutputStreamReplaceExcludePaths appGetOutputStreamReplaceExcludePaths)
        {
            _nextMiddleware = next;
            _name = name;
            _categoryName = categoryName;
            _loggerFactory = loggerFactory;
            _appHttpContextLogConvert = appHttpContextLogConvert;
            _appGetLogExcludePaths = appGetLogExcludePaths;
            _appGetOutputStreamReplaceExcludePaths = appGetOutputStreamReplaceExcludePaths;
        }

        public async Task Invoke(HttpContext context)
        {
            var logger = _loggerFactory.CreateLogger(_categoryName);
            await HttpErrorHelper.ExecuteByHttpContextAsync(context, logger, async () =>
            {
                using (var diContainer = DIContainerContainer.CreateContainer())
                {
                    ContextContainer.SetValue<IDIContainer>(_name, diContainer);

                    var replaceExcludePaths = await _appGetOutputStreamReplaceExcludePaths.Do();
                    bool needReplace = true;
                    if (context.Request.Path.HasValue)
                    {
                        //检查当前请求路径是否匹配
                        foreach (var item in replaceExcludePaths)
                        {
                            Regex regex = new Regex(item, RegexOptions.IgnoreCase);
                            if (regex.IsMatch(context.Request.Path.Value))
                            {
                                needReplace = false;
                                break;
                            }
                        }
                    }

                    if (needReplace)
                    {
                        Stream originalBody = context.Response.Body;
                        try
                        {

                            using (var responseStream = new MemoryStream())
                            {
                                context.Response.Body = responseStream;


                                await InnerInvoke(context);


                                responseStream.Position = 0;
                                await responseStream.CopyToAsync(originalBody);

                            }
                        }
                        finally
                        {
                            context.Response.Body = originalBody;
                        }
                    }
                    else
                    {
                        await InnerInvoke(context);
                    }

                }

            });


        }



        private async Task InnerInvoke(HttpContext context)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            await _nextMiddleware.Invoke(context);

            var excludePaths = await _appGetLogExcludePaths.Do();
            watch.Stop();
            //context.Items["RunTime"] = watch.ElapsedMilliseconds;
            bool needDo = true;
            if (context.Request.Path.HasValue)
            {
                //检查当前请求路径是否匹配
                foreach (var item in excludePaths)
                {
                    Regex regex = new Regex(item, RegexOptions.IgnoreCase);
                    if (regex.IsMatch(context.Request.Path.Value))
                    {
                        needDo = false;
                        break;
                    }
                }
            }

            if (needDo)
            {
                MemoryStream requestStream = null;
                MemoryStream responseStream = null;

                try
                {
                    if (context.Request != null && context.Request.Body != null && context.Request.Body.CanRead)
                    {
                        //context.Request.Body.can.Position = 0;
                        requestStream = new MemoryStream();
                        await context.Request.Body.CopyToAsync(requestStream);
                        requestStream.Position = 0;
                    }

                    if (context.Response != null && context.Response.Body != null && context.Response.Body.CanRead)
                    {
                        context.Response.Body.Position = 0;
                        responseStream = new MemoryStream();
                        await context.Response.Body.CopyToAsync(responseStream);
                        responseStream.Position = 0;
                    }

                    HttpContextData data = new HttpContextData(requestStream, responseStream, context.Request.GetDisplayUrl(), context.Request.Path.Value, context.Request.PathBase.Value, watch.ElapsedMilliseconds);



                    //从Http上下文中获取上下文生成结果
                    if (context.Items.TryGetValue("AuthorizeResult", out object objResult))
                    {
                    }

                    //从Http上下文中获取http请求上下文初始化对象列表
                    if (context.Items.TryGetValue(_httpContextItemName, out object objInit))
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
                            foreach(var item in inits)
                            {
                                item.Value.Execute();
                            }
                        }

                        var logObj = await _appHttpContextLogConvert.Convert(data);

                        LoggerHelper.LogInformation(_categoryName, logObj);

                    });



                }
                catch
                {
                    if (requestStream!=null)
                    {
                        try
                        {
                            await requestStream.DisposeAsync();
                        }
                        catch
                        {

                        }
                    }
                    if (responseStream!=null)
                    {
                        try
                        {
                            await responseStream.DisposeAsync();
                        }
                        catch
                        {
                         
                        }
                    }
                    throw;
                }

            }



        }
    }

    public class HttpContextData
    {
        public HttpContextData(Stream request, Stream response, string requestUri, string requestPath, string requestBasePath, long duration)
        {
            Request = request;
            Response = response;
            RequestUri = requestUri;
            RequestPath = requestPath;
            RequestBasePath = requestBasePath;
            Duration = duration;
        }
        public Stream Request { get; private set; }
        public Stream Response { get; private set; }

        public string RequestUri { get; private set; }

        public string RequestPath { get; private set; }
        public string RequestBasePath { get; private set; }

        public long Duration { get; private set; }
    }
}
