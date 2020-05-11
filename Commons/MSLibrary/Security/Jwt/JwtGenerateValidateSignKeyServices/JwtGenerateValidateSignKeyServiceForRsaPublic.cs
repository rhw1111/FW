using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using MSLibrary.DI;
using MSLibrary.Serializer;

namespace MSLibrary.Security.Jwt.JwtGenerateValidateSignKeyServices
{
    /// <summary>
    /// 基于非对称密钥公钥的签名
    /// 需要的配置信息内容为
    /// {
    ///     "Key":"Rsa中公钥，与生成时的私钥相对",
    /// }
    /// </summary>
    [Injection(InterfaceType = typeof(JwtGenerateValidateSignKeyServiceForRsaPublic), Scope = InjectionScope.Singleton)]
    public class JwtGenerateValidateSignKeyServiceForRsaPublic : IJwtGenerateValidateSignKeyService
    {
        public async Task<IEnumerable<SecurityKey>> Generate(JwtEnpoint endpoint, string type, string configuration)
        {
            var configurationObj = JsonSerializerHelper.Deserialize<RsaConfiguration>(configuration);

            RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider();
            rsaProvider.FromXmlString(configurationObj.Key);
            var rasParameters = rsaProvider.ExportParameters(false);
            RsaSecurityKey securityKey = new RsaSecurityKey(rasParameters);

            return await Task.FromResult(new List<SecurityKey>() { securityKey });
        }

        [DataContract]
        private class RsaConfiguration
        {
            [DataMember]
            public string Key { get; set; }
        }
    }
}
