using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.Grpc.Interceptors;
using MSLibrary.Grpc.Interceptors.Application;
using IdentityCenter.Main.Logger;

namespace IdentityCenter.Main.Grpc.Interceptors.Application
{
    [Injection(InterfaceType = typeof(IAppGrpcLogContextConvert), Scope = InjectionScope.Singleton)]
    public class AppGrpcLogContextConvert : IAppGrpcLogContextConvert
    {
        public async Task<object> Convert(GrpcLogContextData contextData)
        {
            LoggerContent content = new LoggerContent()
            {
                ActionName = contextData.RequestPath,
                Duration = contextData.Duration,
                Message = string.Empty,
                RequestBody = string.Empty,
                ResponseBody = string.Empty,
                RequestUri = contextData.RequestUri

            };
            content.Tags.Add(contextData.RequestUri);
            return await Task.FromResult(content);
        }
    }
}
