using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Azure.TokenCredentialGeneratorServices
{

    [Injection(InterfaceType = typeof(TokenCredentialGeneratorServiceForManagedIdentityFactory), Scope = InjectionScope.Singleton)]
    public class TokenCredentialGeneratorServiceForManagedIdentityFactory : IFactory<ITokenCredentialGeneratorService>
    {
        private TokenCredentialGeneratorServiceForManagedIdentity _tokenCredentialGeneratorServiceForManagedIdentity;

        public TokenCredentialGeneratorServiceForManagedIdentityFactory(TokenCredentialGeneratorServiceForManagedIdentity tokenCredentialGeneratorServiceForManagedIdentity)
        {
            _tokenCredentialGeneratorServiceForManagedIdentity = tokenCredentialGeneratorServiceForManagedIdentity;
        }
        public ITokenCredentialGeneratorService Create()
        {
            return _tokenCredentialGeneratorServiceForManagedIdentity;
        }
    }
}
