using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Security.Jwt.JwtGenerateValidateSignKeyServices
{
    [Injection(InterfaceType = typeof(JwtGenerateValidateSignKeyServiceForRsaPublicFactory), Scope = InjectionScope.Singleton)]
    public class JwtGenerateValidateSignKeyServiceForRsaPublicFactory : IFactory<IJwtGenerateValidateSignKeyService>
    {
        private JwtGenerateValidateSignKeyServiceForRsaPublic _jwtGenerateValidateSignKeyServiceForRsaPublic;

        public JwtGenerateValidateSignKeyServiceForRsaPublicFactory(JwtGenerateValidateSignKeyServiceForRsaPublic jwtGenerateValidateSignKeyServiceForRsaPublic)
        {
            _jwtGenerateValidateSignKeyServiceForRsaPublic = jwtGenerateValidateSignKeyServiceForRsaPublic;
        }
        public IJwtGenerateValidateSignKeyService Create()
        {
            return _jwtGenerateValidateSignKeyServiceForRsaPublic;
        }
    }
}
