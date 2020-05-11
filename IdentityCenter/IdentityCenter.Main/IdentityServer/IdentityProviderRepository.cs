using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.DI;

namespace IdentityCenter.Main.IdentityServer
{
    [Injection(InterfaceType = typeof(IIdentityProviderRepository), Scope = InjectionScope.Singleton)]
    public class IdentityProviderRepository : IIdentityProviderRepository
    {
        public static IDictionary<string, IdentityProvider> IdentityProviders { get; } = new Dictionary<string, IdentityProvider>();

        public async Task<IdentityProvider?> QueryBySchemeName(string name, CancellationToken cancellationToken = default)
        {
            if (IdentityProviders.TryGetValue(name,out IdentityProvider? provider))
            {
                return await Task.FromResult(provider);
            }
            return null;
        }

        public async Task<IList<IdentityProvider>> QueryBySchemeName(IList<string> names, CancellationToken cancellationToken = default)
        {
            List<IdentityProvider> providers = new List<IdentityProvider>();
            foreach(var item in names)
            {
                if (IdentityProviders.TryGetValue(item, out IdentityProvider? provider))
                {
                    providers.Add(provider);
                }
            }

            return await Task.FromResult(providers);
        }
    }
}
