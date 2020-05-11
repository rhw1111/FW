using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Security.Jwt.JwtGenerateValidateSignKeyServices
{
    [Injection(InterfaceType = typeof(JwtGenerateValidateSignKeyServiceForMetadataServiceFactory), Scope = InjectionScope.Singleton)]
    public class JwtGenerateValidateSignKeyServiceForMetadataServiceFactory : IFactory<IJwtGenerateValidateSignKeyService>
    {
        private JwtGenerateValidateSignKeyServiceForMetadataService _jwtGenerateValidateSignKeyServiceForMetadataService;

        public JwtGenerateValidateSignKeyServiceForMetadataServiceFactory(JwtGenerateValidateSignKeyServiceForMetadataService jwtGenerateValidateSignKeyServiceForMetadataService)
        {
            _jwtGenerateValidateSignKeyServiceForMetadataService = jwtGenerateValidateSignKeyServiceForMetadataService;
        }
        public IJwtGenerateValidateSignKeyService Create()
        {
            return _jwtGenerateValidateSignKeyServiceForMetadataService;
        }
    }
}
