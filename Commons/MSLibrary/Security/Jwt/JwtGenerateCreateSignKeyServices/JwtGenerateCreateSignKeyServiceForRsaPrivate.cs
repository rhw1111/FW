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

namespace MSLibrary.Security.Jwt.JwtGenerateCreateSignKeyServices
{
    /// <summary>
    /// 基于非对称私钥的签名
    /// 需要的配置信息内容为
    /// {
    ///     "Key":"Rsa中私钥，与验证时的公钥相对",
    ///     "Alg":"加密算法"
    /// }
    /// </summary>
    [Injection(InterfaceType = typeof(JwtGenerateCreateSignKeyServiceForRsaPrivate), Scope = InjectionScope.Singleton)]
    public class JwtGenerateCreateSignKeyServiceForRsaPrivate : IJwtGenerateCreateSignKeyService
    {
        public async Task<SigningCredentials> Generate(JwtEnpoint endpoint, string type, string configuration)
        {
            var configurationObj=JsonSerializerHelper.Deserialize<RsaConfiguration>(configuration);
            SigningCredentials signingCredentials = null;
            using (RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider(2048))
            {
               
                rsaProvider.FromXmlString(configurationObj.Key);
                //var strkey= rsaProvider.ToXmlString(true);
                var rasParameters = rsaProvider.ExportParameters(true);
             
                RsaSecurityKey securityKey = new RsaSecurityKey(rasParameters);
                signingCredentials = new SigningCredentials(securityKey, configurationObj.Alg);

            }

            return await Task.FromResult(signingCredentials);
        }

        [DataContract]
        private class RsaConfiguration
        {
            [DataMember]
            public string Key { get; set; }
            [DataMember]
            public string Alg { get; set; }
        }
    }
}
