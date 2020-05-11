using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary.DI;
using IdentityCenter.Main.IdentityServer.DAL;

namespace IdentityCenter.Main.IdentityServer
{
    [Injection(InterfaceType = typeof(IIdentityClientHostRepository), Scope = InjectionScope.Singleton)]
    public class IdentityClientHostRepository : IIdentityClientHostRepository
    {
        private readonly IIdentityClientHostStore _identityClientHostStore;

        public IdentityClientHostRepository(IIdentityClientHostStore identityClientHostStore)
        {
            _identityClientHostStore = identityClientHostStore;
        }
        public async Task<IdentityClientHost?> QueryByName(string name, CancellationToken cancellationToken = default)
        {
            return await _identityClientHostStore.QueryByName(name, cancellationToken);
        }
    }
}
