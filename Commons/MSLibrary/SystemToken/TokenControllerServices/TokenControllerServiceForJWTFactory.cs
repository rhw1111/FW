using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.SystemToken.TokenControllerServices
{
    [Injection(InterfaceType = typeof(TokenControllerServiceForJWTFactory), Scope = InjectionScope.Singleton)]
    public class TokenControllerServiceForJWTFactory : IFactory<ITokenControllerService>
    {
        private TokenControllerServiceForJWT _tokenControllerServiceForJWT;

        public TokenControllerServiceForJWTFactory(TokenControllerServiceForJWT tokenControllerServiceForJWT)
        {
            _tokenControllerServiceForJWT = tokenControllerServiceForJWT;
        }
        public ITokenControllerService Create()
        {
            return _tokenControllerServiceForJWT;
        }
    }
}
