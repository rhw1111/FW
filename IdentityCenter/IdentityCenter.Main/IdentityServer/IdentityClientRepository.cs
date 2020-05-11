using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary.DI;
using IdentityCenter.Main.IdentityServer.DAL;

namespace IdentityCenter.Main.IdentityServer
{
    [Injection(InterfaceType = typeof(IIdentityClientRepository), Scope = InjectionScope.Singleton)]
    public class IdentityClientRepository : IIdentityClientRepository
    {
        private readonly IIdentityClientStore _identityClientStore;

        public IdentityClientRepository(IIdentityClientStore identityClientStore)
        {
            _identityClientStore = identityClientStore;
        }

        public async Task<IdentityClient?> QueryByClientID(string clientID, CancellationToken cancellationToken = default)
        {
            return await _identityClientStore.QueryByClientID(clientID, cancellationToken);
        }
    }
}
