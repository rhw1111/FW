using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Security.Jwt.JwtValidateParameterBuildServices
{
    [Injection(InterfaceType = typeof(JwtValidateParameterBuildServiceForAudienceFactory), Scope = InjectionScope.Singleton)]
    public class JwtValidateParameterBuildServiceForAudienceFactory : IFactory<IJwtValidateParameterBuildService>
    {
        private JwtValidateParameterBuildServiceForAudience _jwtValidateParameterBuildServiceForAudience;

        public JwtValidateParameterBuildServiceForAudienceFactory(JwtValidateParameterBuildServiceForAudience jwtValidateParameterBuildServiceForAudience)
        {
            _jwtValidateParameterBuildServiceForAudience = jwtValidateParameterBuildServiceForAudience;
        }
        public IJwtValidateParameterBuildService Create()
        {
            return _jwtValidateParameterBuildServiceForAudience;
        }
    }
}
