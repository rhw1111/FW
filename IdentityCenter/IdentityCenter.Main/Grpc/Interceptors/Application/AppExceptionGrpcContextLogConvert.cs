using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.Grpc.Interceptors;
using MSLibrary.Grpc.Interceptors.Application;
using IdentityCenter.Main.Logger;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Http;

namespace IdentityCenter.Main.Grpc.Interceptors.Application
{
    [Injection(InterfaceType = typeof(IAppExceptionGrpcContextLogConvert), Scope = InjectionScope.Singleton)]
    public class AppExceptionGrpcContextLogConvert : IAppExceptionGrpcContextLogConvert
    {
        public async Task<object> Convert(ServerCallContext context)
        {
            //取出存储在上下文Item中的异常
            var ex = (Exception)context.UserState["ExecuteException"];

            HttpContext httpContext = context.GetHttpContext();

            LoggerContent content = new LoggerContent() { RequestUri = httpContext.Request != null ? httpContext.Request.Path.Value : string.Empty, ActionName = "", Message = $"Unhandle Error,\nmessage:{ex.Message},\nstacktrace:{ex.StackTrace}", RequestBody =  string.Empty, ResponseBody = string.Empty };

            if (httpContext.Request != null)
            {
                content.Tags.Add(httpContext.Request.PathBase.Value);
            }
            return await Task.FromResult(content);
        }
    }
}
