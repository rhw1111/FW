using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using MSLibrary.DI;
using MSLibrary.Cache;
using IdentityCenter.Main.IdentityServer.DAL;

namespace IdentityCenter.Main.IdentityServer
{
    [Injection(InterfaceType = typeof(IIdentityResourceDataRepository), Scope = InjectionScope.Singleton)]
    public class IdentityResourceDataRepository : IIdentityResourceDataRepository
    {
        private readonly IIdentityResourceDataStore _identityResourceDataStore;

        public IdentityResourceDataRepository(IIdentityResourceDataStore identityResourceDataStore)
        {
            _identityResourceDataStore = identityResourceDataStore;
        }

        public async Task<IList<IdentityResourceData>> QueryAllEnabled(CancellationToken cancellationToken = default)
        {
            return await _identityResourceDataStore.QueryAllEnabled(cancellationToken);
        }

        public async Task<IList<IdentityResourceData>> QueryEnabled(IList<string> names, CancellationToken cancellationToken = default)
        {
            return await _identityResourceDataStore.QueryEnabled(names, cancellationToken);
        }
    }
}
