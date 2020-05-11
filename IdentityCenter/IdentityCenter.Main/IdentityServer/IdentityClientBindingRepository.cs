using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary.DI;
using IdentityCenter.Main.IdentityServer.DAL;

namespace IdentityCenter.Main.IdentityServer
{
    [Injection(InterfaceType = typeof(IIdentityClientBindingRepository), Scope = InjectionScope.Singleton)]
    public class IdentityClientBindingRepository : IIdentityClientBindingRepository
    {
        private readonly IIdentityClientBindingStore _identityClientBindingStore;

        public IdentityClientBindingRepository(IIdentityClientBindingStore identityClientBindingStore)
        {
            _identityClientBindingStore = identityClientBindingStore;
        }
        public IAsyncEnumerable<IdentityClientBinding> QueryAll(CancellationToken cancellationToken = default)
        {
            return  _identityClientBindingStore.QueryAll(cancellationToken);
        }
    }
}
