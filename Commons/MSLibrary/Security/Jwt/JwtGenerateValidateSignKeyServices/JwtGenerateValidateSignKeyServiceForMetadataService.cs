using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using MSLibrary.DI;
using MSLibrary.Serializer;
using MSLibrary.Cache;

namespace MSLibrary.Security.Jwt.JwtGenerateValidateSignKeyServices
{
    /// <summary>
    /// 基于元数据服务的签名
    /// 需要的配置信息内容为
    /// {
    ///     "Uri":"元数据服务地址",
    ///     "Cache":是否需要缓存，true或false
    ///     "Timeout":如果需要缓存，缓存的时间，单位秒，小于0表示无限时
    /// }
    /// </summary>
    [Injection(InterfaceType = typeof(JwtGenerateValidateSignKeyServiceForMetadataService), Scope = InjectionScope.Singleton)]
    public class JwtGenerateValidateSignKeyServiceForMetadataService : IJwtGenerateValidateSignKeyService
    {
        private const string _cacheAttributeName = "ValidateSignKey";
        public async Task<IEnumerable<SecurityKey>> Generate(JwtEnpoint endpoint, string type, string configuration)
        {
            var configurationObj = JsonSerializerHelper.Deserialize<RsaConfiguration>(configuration);
            IEnumerable<SecurityKey> keys=new List<SecurityKey>();
            CacheTimeContainer<IEnumerable<SecurityKey>> cacheItem;
            bool needCreate = true;
            if (configurationObj.Cache)
            {
                if (endpoint.Extensions.TryGetValue(_cacheAttributeName, out Object objValidateSignKeyCache))
                {
                    cacheItem = (CacheTimeContainer<IEnumerable<SecurityKey>>)objValidateSignKeyCache;
                    if (!cacheItem.Expire())
                    {
                        needCreate = false;
                        keys = cacheItem.Value;
                    }

                }

                if (needCreate)
                {
                    keys = await getSecurityKeys(configurationObj.Uri);
                    cacheItem = new CacheTimeContainer<IEnumerable<SecurityKey>>(keys, configurationObj.Timeout);
                    endpoint.SetExtension(_cacheAttributeName, cacheItem);
                }
            }
            else
            {
                keys = await getSecurityKeys(configurationObj.Uri);
            }

            return keys;
        }

        private async Task<IEnumerable<SecurityKey>> getSecurityKeys(string uri)
        {
            IConfigurationManager<OpenIdConnectConfiguration> configurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(uri,
                 new OpenIdConnectConfigurationRetriever());
            OpenIdConnectConfiguration openIdConfig = await configurationManager.GetConfigurationAsync(CancellationToken.None);
            return openIdConfig.SigningKeys;
        }

        [DataContract]
        private class RsaConfiguration
        {
            [DataMember]
            public string Uri { get; set; }

            [DataMember]
            public bool Cache { get; set; }
            [DataMember]
            public int Timeout { get; set; }
        }
    }
}
