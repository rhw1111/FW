using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityCenter.Main.IdentityServer.ClientBindings
{
    public interface IIdentityClientOpenIDBindingRepositoryCacheProxy
    {
        Task<IdentityClientOpenIDBinding?> QueryByName(string name,CancellationToken cancellationToken = default);
    }
}
