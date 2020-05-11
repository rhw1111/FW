using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary.DI;
using IdentityCenter.Main.IdentityServer.DAL;

namespace IdentityCenter.Main.IdentityServer
{
    [Injection(InterfaceType = typeof(IIdentityHostRepository), Scope = InjectionScope.Singleton)]
    public class IdentityHostRepository : IIdentityHostRepository
    {
        private IIdentityHostStore _identityHostStore;

        public IdentityHostRepository(IIdentityHostStore identityHostStore)
        {
            _identityHostStore = identityHostStore;
        }
        public async Task<IdentityHost?> QueryByName(string name, CancellationToken cancellationToken = default)
        {
            return await _identityHostStore.QueryByName(name, cancellationToken);
        }
    }
}
