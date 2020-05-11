using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Azure.TokenCredentialGeneratorServices
{
    [Injection(InterfaceType = typeof(TokenCredentialGeneratorServiceForUsernamePasswordFactory), Scope = InjectionScope.Singleton)]
    public class TokenCredentialGeneratorServiceForUsernamePasswordFactory : IFactory<ITokenCredentialGeneratorService>
    {
        private TokenCredentialGeneratorServiceForUsernamePassword _tokenCredentialGeneratorServiceForUsernamePassword;
        public TokenCredentialGeneratorServiceForUsernamePasswordFactory(TokenCredentialGeneratorServiceForUsernamePassword tokenCredentialGeneratorServiceForUsernamePassword)
        {
            _tokenCredentialGeneratorServiceForUsernamePassword = tokenCredentialGeneratorServiceForUsernamePassword;
        }
        public ITokenCredentialGeneratorService Create()
        {
            return _tokenCredentialGeneratorServiceForUsernamePassword;
        }
    }
}
