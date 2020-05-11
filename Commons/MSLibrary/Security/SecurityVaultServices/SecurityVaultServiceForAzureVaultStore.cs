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
using MSLibrary.Azure;

namespace MSLibrary.Security.SecurityVaultServices
{
    /// <summary>
    /// 使用Azure Vault的机密数据服务(令牌凭据来自于令牌凭据生成器)
    /// 配置格式为
    /// {
    ///     
    /// }
    /// </summary>
    [Injection(InterfaceType = typeof(SecurityVaultServiceForAzureVaultStore), Scope = InjectionScope.Singleton)]
    public class SecurityVaultServiceForAzureVaultStore : ISecurityVaultService
    {
        private ITokenCredentialGeneratorRepositoryCacheProxy _tokenCredentialGeneratorRepositoryCacheProxy;

        public SecurityVaultServiceForAzureVaultStore(ITokenCredentialGeneratorRepositoryCacheProxy tokenCredentialGeneratorRepositoryCacheProxy)
        {
            _tokenCredentialGeneratorRepositoryCacheProxy = tokenCredentialGeneratorRepositoryCacheProxy;
        }
        public async Task<string> GetData(string configuration, string name)
        {
            var configurationObj = JsonSerializerHelper.Deserialize<Configuration>(configuration);

            var credentialGenerator=await _tokenCredentialGeneratorRepositoryCacheProxy.QueryByName(configurationObj.CredentialName);
            if (credentialGenerator==null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundTokenCredentialGeneratorByName,
                    DefaultFormatting = "找不到名称为{0}的Azure令牌凭据生成器",
                    ReplaceParameters = new List<object>() { configurationObj.CredentialName }
                };

                throw new UtilityException((int)Errors.NotFoundTokenCredentialGeneratorByName, fragment);
            }

            var credential=await credentialGenerator.Generate();

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

            var client = new SecretClient(new Uri($"https://{configurationObj.VaultName}{configurationObj.VaultUriSuffix}"), credential, options);

            var result = await client.GetSecretAsync(name);

            return result.Value.Value;
        }

        [DataContract]
        private class Configuration
        {
            [DataMember]
            public string CredentialName { get; set; }
            [DataMember]
            public string VaultName { get; set; }
            [DataMember]
            public string VaultUriSuffix { get; set; }
        }
    }
}
