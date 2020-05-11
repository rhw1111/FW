using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.DI;
using IdentityCenter.Main.IdentityServer.DAL;

namespace IdentityCenter.Main.IdentityServer.ClientBindings
{
    [Injection(InterfaceType = typeof(IIdentityClientOpenIDBindingRepository), Scope = InjectionScope.Singleton)]
    public class IdentityClientOpenIDBindingRepository : IIdentityClientOpenIDBindingRepository
    {
        private readonly IIdentityClientOpenIDBindingStore _identityClientOpenIDBindingStore;
        public IdentityClientOpenIDBindingRepository(IIdentityClientOpenIDBindingStore identityClientOpenIDBindingStore)
        {
            _identityClientOpenIDBindingStore = identityClientOpenIDBindingStore;
        }
        public async Task<IdentityClientOpenIDBinding?> QueryByName(string name,CancellationToken cancellationToken = default)
        {
            return await _identityClientOpenIDBindingStore.QueryByName(name, cancellationToken);
        }
    }
}
