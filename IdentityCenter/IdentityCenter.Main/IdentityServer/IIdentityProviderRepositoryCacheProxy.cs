using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityCenter.Main.IdentityServer
{
    public interface IIdentityProviderRepositoryCacheProxy
    {
        Task<IdentityProvider?> QueryBySchemeName(string name, CancellationToken cancellationToken = default);
        Task<IList<IdentityProvider>> QueryBySchemeName(IList<string> names, CancellationToken cancellationToken = default);

    }
}
