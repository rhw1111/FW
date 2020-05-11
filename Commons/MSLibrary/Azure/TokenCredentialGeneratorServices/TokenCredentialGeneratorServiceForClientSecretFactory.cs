using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Azure.TokenCredentialGeneratorServices
{

    [Injection(InterfaceType = typeof(TokenCredentialGeneratorServiceForClientSecretFactory), Scope = InjectionScope.Singleton)]
    public class TokenCredentialGeneratorServiceForClientSecretFactory : IFactory<ITokenCredentialGeneratorService>
    {
        private TokenCredentialGeneratorServiceForClientSecret _tokenCredentialGeneratorServiceForClientSecret;

        public TokenCredentialGeneratorServiceForClientSecretFactory(TokenCredentialGeneratorServiceForClientSecret tokenCredentialGeneratorServiceForClientSecret)
        {
            _tokenCredentialGeneratorServiceForClientSecret = tokenCredentialGeneratorServiceForClientSecret;
        }
        public ITokenCredentialGeneratorService Create()
        {
            return _tokenCredentialGeneratorServiceForClientSecret;
        }
    }
}
