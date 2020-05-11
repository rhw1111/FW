using Azure.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Azure.Identity;
using MSLibrary.DI;
using MSLibrary.Serializer;

namespace MSLibrary.Azure.TokenCredentialGeneratorServices
{
    /// <summary>
    /// 针对托管标识的令牌凭据生成器服务
    /// 配置格式为
    /// {
    ///     "ClientId":"ClientId",
    ///     "LoginUri":"Azure登录Uri"
    /// }
    [Injection(InterfaceType = typeof(TokenCredentialGeneratorServiceForManagedIdentity), Scope = InjectionScope.Singleton)]
    public class TokenCredentialGeneratorServiceForManagedIdentity : ITokenCredentialGeneratorService
    {
        public async Task<TokenCredential> Generate(string configuration)
        {
            var configurationObj = JsonSerializerHelper.Deserialize<Configuration>(configuration);
            var credentialOption = new TokenCredentialOptions();
            credentialOption.AuthorityHost = new Uri(configurationObj.LoginUri);

            string clientID = null;
            if (!string.IsNullOrEmpty(configurationObj.ClientId))
            {
                clientID = configurationObj.ClientId;
            }
            var credential = new ManagedIdentityCredential(clientID);

            return await Task.FromResult(credential);
        }

        [DataContract]
        private class Configuration
        {
            [DataMember]
            public string LoginUri { get; set; }
            [DataMember]
            public string ClientId { get; set; }
        }
    }
}
