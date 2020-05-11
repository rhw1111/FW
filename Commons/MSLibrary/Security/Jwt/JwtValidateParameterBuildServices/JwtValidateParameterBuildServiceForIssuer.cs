using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using MSLibrary.DI;

namespace MSLibrary.Security.Jwt.JwtValidateParameterBuildServices
{
    /// <summary>
    /// 针对Issuer的验证
    /// 参数Type：Issuer
    /// 参数Value：要验证的Issuer
    /// </summary>
    [Injection(InterfaceType = typeof(JwtValidateParameterBuildServiceForIssuer), Scope = InjectionScope.Singleton)]
    public class JwtValidateParameterBuildServiceForIssuer : IJwtValidateParameterBuildService
    {
        public async Task Build(TokenValidationParameters tokenParameter, JwtValidateParameter parameter)
        {
            tokenParameter.ValidateIssuer = true;
            tokenParameter.ValidIssuer = parameter.Value;
            await Task.FromResult(0);
        }
    }
}
