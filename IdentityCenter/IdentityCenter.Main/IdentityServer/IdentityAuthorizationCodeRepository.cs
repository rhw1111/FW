using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary.DI;
using IdentityCenter.Main.IdentityServer.DAL;

namespace IdentityCenter.Main.IdentityServer
{
    [Injection(InterfaceType = typeof(IIdentityAuthorizationCodeRepository), Scope = InjectionScope.Singleton)]
    public class IdentityAuthorizationCodeRepository : IIdentityAuthorizationCodeRepository
    {
        private IIdentityAuthorizationCodeStore _identityAuthorizationCodeStore;

        public IdentityAuthorizationCodeRepository(IIdentityAuthorizationCodeStore identityAuthorizationCodeStore)
        {
            _identityAuthorizationCodeStore = identityAuthorizationCodeStore;
        }
        public async Task<IdentityAuthorizationCode?> QueryByCode(string code, CancellationToken cancellationToken = default)
        {
            return await _identityAuthorizationCodeStore.QueryByCode(code, cancellationToken);
        }
    }
}
