using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;
using MSLibrary.DI;

namespace MSLibrary.Grpc.ChannelCredentialsGeneratorServices
{
    /// <summary>
    /// 针对非安全的通道凭证
    /// </summary>
    [Injection(InterfaceType = typeof(ChannelCredentialsGeneratorServiceForInsecure), Scope = InjectionScope.Singleton)]
    public class ChannelCredentialsGeneratorServiceForInsecure:IChannelCredentialsGeneratorService
    {
        public async Task<ChannelCredentials> Generate(string type, string configuration)
        {
            return await Task.FromResult(ChannelCredentials.Insecure);
        }
    }
}
