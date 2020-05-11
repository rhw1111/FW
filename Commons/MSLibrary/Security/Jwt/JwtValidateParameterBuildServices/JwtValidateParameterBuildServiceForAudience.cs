using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using MSLibrary.DI;

namespace MSLibrary.Security.Jwt.JwtValidateParameterBuildServices
{
    /// <summary>
    /// 针对Audience的验证
    /// 参数Type：Audience
    /// 参数Value：要验证的Audience
    /// </summary>
    [Injection(InterfaceType = typeof(JwtValidateParameterBuildServiceForAudience), Scope = InjectionScope.Singleton)]
    public class JwtValidateParameterBuildServiceForAudience : IJwtValidateParameterBuildService
    {
        public async Task Build(TokenValidationParameters tokenParameter, JwtValidateParameter parameter)
        {
            tokenParameter.RequireAudience = true;
            tokenParameter.ValidateAudience = true;
            tokenParameter.ValidAudience = parameter.Value;

            await Task.FromResult(0);
        }
    }
}
