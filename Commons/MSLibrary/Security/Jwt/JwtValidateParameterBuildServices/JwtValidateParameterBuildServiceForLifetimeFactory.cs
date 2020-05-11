using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Security.Jwt.JwtValidateParameterBuildServices
{
    [Injection(InterfaceType = typeof(JwtValidateParameterBuildServiceForLifetimeFactory), Scope = InjectionScope.Singleton)]
    public class JwtValidateParameterBuildServiceForLifetimeFactory : IFactory<IJwtValidateParameterBuildService>
    {
        private JwtValidateParameterBuildServiceForLifetime _jwtValidateParameterBuildServiceForLifetime;

        public JwtValidateParameterBuildServiceForLifetimeFactory(JwtValidateParameterBuildServiceForLifetime jwtValidateParameterBuildServiceForLifetime)
        {
            _jwtValidateParameterBuildServiceForLifetime = jwtValidateParameterBuildServiceForLifetime;
        }
        public IJwtValidateParameterBuildService Create()
        {
            return _jwtValidateParameterBuildServiceForLifetime;
        }
    }
}
