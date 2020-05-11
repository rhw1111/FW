using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Security.Jwt.JwtGenerateCreateSignKeyServices
{
    [Injection(InterfaceType = typeof(JwtGenerateCreateSignKeyServiceForRsaPrivateFactory), Scope = InjectionScope.Singleton)]
    public class JwtGenerateCreateSignKeyServiceForRsaPrivateFactory : IFactory<IJwtGenerateCreateSignKeyService>
    {
        private JwtGenerateCreateSignKeyServiceForRsaPrivate _jwtGenerateCreateSignKeyServiceForRsaPrivate;

        public JwtGenerateCreateSignKeyServiceForRsaPrivateFactory(JwtGenerateCreateSignKeyServiceForRsaPrivate jwtGenerateCreateSignKeyServiceForRsaPrivate)
        {
            _jwtGenerateCreateSignKeyServiceForRsaPrivate = jwtGenerateCreateSignKeyServiceForRsaPrivate;
        }
        public IJwtGenerateCreateSignKeyService Create()
        {
            return _jwtGenerateCreateSignKeyServiceForRsaPrivate;
        }
    }
}
