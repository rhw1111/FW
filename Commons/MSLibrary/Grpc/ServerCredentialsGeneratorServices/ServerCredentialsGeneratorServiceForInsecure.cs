using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;
using MSLibrary.DI;

namespace MSLibrary.Grpc.ServerCredentialsGeneratorServices
{
    /// <summary>
    /// 针对非安全的服务端凭证
    /// </summary>
    [Injection(InterfaceType = typeof(ServerCredentialsGeneratorServiceForInsecure), Scope = InjectionScope.Singleton)]
    public class ServerCredentialsGeneratorServiceForInsecure:IServerCredentialsGeneratorService
    {
        public async Task<ServerCredentials> Generate(string type, string configuration)
        {
            return await Task.FromResult(ServerCredentials.Insecure);
        }
    }
}
