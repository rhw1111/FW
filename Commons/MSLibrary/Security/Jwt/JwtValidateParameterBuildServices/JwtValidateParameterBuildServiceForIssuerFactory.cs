using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Security.Jwt.JwtValidateParameterBuildServices
{
    [Injection(InterfaceType = typeof(JwtValidateParameterBuildServiceForIssuerFactory), Scope = InjectionScope.Singleton)]
    public class JwtValidateParameterBuildServiceForIssuerFactory : IFactory<IJwtValidateParameterBuildService>
    {
        private JwtValidateParameterBuildServiceForIssuer _jwtValidateParameterBuildServiceForIssuer;

        public JwtValidateParameterBuildServiceForIssuerFactory(JwtValidateParameterBuildServiceForIssuer jwtValidateParameterBuildServiceForIssuer)
        {
            _jwtValidateParameterBuildServiceForIssuer = jwtValidateParameterBuildServiceForIssuer;
        }
        public IJwtValidateParameterBuildService Create()
        {
            return _jwtValidateParameterBuildServiceForIssuer;
        }
    }
}
