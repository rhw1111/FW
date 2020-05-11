using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using MSLibrary.DI;

namespace MSLibrary.Security.Jwt.JwtValidateParameterBuildServices
{
    /// <summary>
    /// 针对Lifetime的验证
    /// 参数Type：Lifetime
    /// 参数Value：无意义，任何值都一样，不用赋值
    /// </summary>
    [Injection(InterfaceType = typeof(JwtValidateParameterBuildServiceForLifetime), Scope = InjectionScope.Singleton)]
    public class JwtValidateParameterBuildServiceForLifetime : IJwtValidateParameterBuildService
    {
        public async Task Build(TokenValidationParameters tokenParameter, JwtValidateParameter parameter)
        {
            tokenParameter.ValidateLifetime = true;
            await Task.FromResult(0);
        }
    }
}
