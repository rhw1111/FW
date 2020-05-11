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
    /// 针对ClientSecret的令牌凭据生成器服务
    /// 配置格式为
    /// {
    ///     "TenantId":"租户ID",
    ///     "ClientId":"ClientId",
    ///     "ClientSecret":"ClientSecret",
    ///     "LoginUri":"Azure登录Uri"
    /// }
    /// </summary>
    [Injection(InterfaceType = typeof(TokenCredentialGeneratorServiceForClientSecret), Scope = InjectionScope.Singleton)]
    public class TokenCredentialGeneratorServiceForClientSecret : ITokenCredentialGeneratorService
    {
        public async Task<TokenCredential> Generate(string configuration)
        {
            var configurationObj=JsonSerializerHelper.Deserialize<Configuration>(configuration);
            var credentialOption = new TokenCredentialOptions();
            credentialOption.AuthorityHost = new Uri(configurationObj.LoginUri);

            var credential = new ClientSecretCredential(configurationObj.TenantId, configurationObj.ClientId, configurationObj.ClientSecret, credentialOption);

            return await Task.FromResult(credential);
        }

        [DataContract]
        private class Configuration
        {
            [DataMember]
            public string LoginUri { get; set; }
            [DataMember]
            public string TenantId { get; set; }
            [DataMember]
            public string ClientId { get; set; }
            [DataMember]
            public string ClientSecret { get; set; }
        }
    }
}
