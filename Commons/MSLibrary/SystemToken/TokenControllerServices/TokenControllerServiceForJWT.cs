using MSLibrary.SystemToken;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.Serialization;
using Microsoft.IdentityModel.Tokens;
using MSLibrary.DI;
using MSLibrary.Serializer;


namespace MSLibrary.SystemToken.TokenControllerServices
{
    /// <summary>
    /// 基于标准JWT的令牌控制器服务
    /// 配置格式为
    /// “
    /// {
    ///     "Issuer":"令牌颁发方",
    ///     "Audience":"令牌申请方",
    ///     "ExpireSeconds":"令牌超时时间（秒）",
    ///     "SignKey":"签名密钥"
    /// }
    /// ”
    /// </summary>
    [Injection(InterfaceType = typeof(TokenControllerServiceForJWT), Scope = InjectionScope.Singleton)]
    public class TokenControllerServiceForJWT : ITokenControllerService
    {
        public async Task<string> Generate(string configuration, IEnumerable<Claim> claims)
        {
            var configurationObj=JsonSerializerHelper.Deserialize<Configuration>(configuration);
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configurationObj.SignKey));

            var token = new JwtSecurityToken(
                            issuer: configurationObj.Issuer,
                            audience: configurationObj.Audience,
                            claims: claims,
                            notBefore: DateTime.UtcNow,
                            expires: DateTime.UtcNow.AddSeconds(configurationObj.ExpireSeconds),
                            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                        );
            
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            return await Task.FromResult(jwtToken);
        }

        public async Task<ClaimsPrincipal> Validate(string configuration, string token)
        {
            var configurationObj = JsonSerializerHelper.Deserialize<Configuration>(configuration);
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configurationObj.SignKey));
            var jwtHandler = new JwtSecurityTokenHandler();
            TokenValidationParameters valudateParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidIssuer = configurationObj.Issuer,
                ValidateAudience = false,
                ValidateIssuerSigningKey=true,
                IssuerSigningKey = key,
                ValidateLifetime = true
            };
            var claim=jwtHandler.ValidateToken(token, valudateParameters, out SecurityToken validatedToken);

            return await Task.FromResult(claim);
        }

        [DataContract]
        private class Configuration
        {
            [DataMember]
            public string Issuer { get; set; }
            [DataMember]
            public string Audience { get; set; }
            [DataMember]
            public int ExpireSeconds { get; set; }
            [DataMember]
            public string SignKey { get; set; }
            
        }
    }
}
