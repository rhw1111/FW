using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Microsoft.Azure.Services.AppAuthentication;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure.Core;
using MSLibrary.DI;
using MSLibrary.Serializer;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.Security.SecurityVaultServices
{
    /// <summary>
    /// 使用Azure Vault的机密数据服务
    /// </summary>
    [Injection(InterfaceType = typeof(SecurityVaultServiceForAzureVault), Scope = InjectionScope.Singleton)]
    public class SecurityVaultServiceForAzureVault : ISecurityVaultService
    {
        public async Task<string> GetData(string configuration, string name)
        {
            var configurationObj = JsonSerializerHelper.Deserialize<Configuration>(configuration);

            var credentialOption = new TokenCredentialOptions();
            credentialOption.AuthorityHost = new Uri(configurationObj.AzureLoginUri);

            TokenCredential credential;

            switch(configurationObj.Type)
            {
                case 1:
                    //ClientSecret
                    credential = new ClientSecretCredential(configurationObj.TenantId, configurationObj.ClientId, configurationObj.ClientSecret, credentialOption);
                    break;
                case 2:
                    //UsernamePassword
                    credential = new UsernamePasswordCredential(configurationObj.UserName, configurationObj.Password, configurationObj.TenantId,configurationObj.ClientId, credentialOption);                 
                    break;
                case 3:
                    //ManagedIdentity
                    string clientID = null;
                    if (!string.IsNullOrEmpty(configurationObj.ClientId))
                    {
                        clientID = configurationObj.ClientId;
                    }
                    credential = new ManagedIdentityCredential(clientID);
                    break;
                default:
                    var fragment = new TextFragment()
                    {
                        Code = TextCodes.NotSupportAzureVaultAuthType,
                        DefaultFormatting = "不支持方式为{0}的AzureVault验证方式，发生位置为{1}",
                        ReplaceParameters = new List<object>() { configurationObj.Type.ToString(),$"{this.GetType().FullName}.GetData" }
                    };
                    throw new UtilityException((int)Errors.NotSupportAzureVaultAuthType, fragment);
            }

            SecretClientOptions options = new SecretClientOptions()
            {
                Retry =
                {
                    Delay= TimeSpan.FromSeconds(2),
                    MaxDelay = TimeSpan.FromSeconds(16),
                    MaxRetries = 5,
                    Mode = RetryMode.Exponential
                }
            };

            var client = new SecretClient(new Uri($"https://{configurationObj.VaultName}{configurationObj.VaultUriSuffix}"), credential,options);

            var result=await client.GetSecretAsync(name);

            return result.Value.Value;
        }

        [DataContract]
        private class Configuration
        {
            /// <summary>
            /// 支持三种方式
            /// 1：ClientSecret
            /// 2：UsernamePassword
            /// 3：ManagedIdentity
            /// </summary>
            [DataMember]
            public int Type { get; set; }
            [DataMember]
            public string TenantId { get; set; }
            [DataMember]
            public string ClientId { get; set; }
            [DataMember]
            public string ClientSecret { get; set; }
            [DataMember]
            public string UserName { get; set; }
            [DataMember]
            public string Password { get; set; }
            [DataMember]
            public string VaultName { get; set; }
            [DataMember]
            public string AzureLoginUri { get; set; }
            [DataMember]
            public string VaultUriSuffix { get; set; }
        }
    }
}
