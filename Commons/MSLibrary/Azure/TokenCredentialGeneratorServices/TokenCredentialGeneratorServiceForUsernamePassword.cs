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
    /// 针对用户名密码的令牌凭据生成器服务
    /// 配置格式为
    /// {
    ///     "TenantId":"租户ID",
    ///     "ClientId":"ClientId",
    ///     "UserName":"用户名",
    ///     "Password":"用户密码",
    ///     "LoginUri":"Azure登录Uri"
    /// }
    /// </summary>
    [Injection(InterfaceType = typeof(TokenCredentialGeneratorServiceForUsernamePassword), Scope = InjectionScope.Singleton)]
    public class TokenCredentialGeneratorServiceForUsernamePassword : ITokenCredentialGeneratorService
    {
        public async Task<TokenCredential> Generate(string configuration)
        {
            var configurationObj = JsonSerializerHelper.Deserialize<Configuration>(configuration);
            var credentialOption = new TokenCredentialOptions();
            credentialOption.AuthorityHost = new Uri(configurationObj.LoginUri);

            var credential = new UsernamePasswordCredential(configurationObj.UserName, configurationObj.Password, configurationObj.TenantId,configurationObj.ClientId, credentialOption);

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
            public string UserName { get; set; }
            [DataMember]
            public string Password { get; set; }
        }

    }
}
