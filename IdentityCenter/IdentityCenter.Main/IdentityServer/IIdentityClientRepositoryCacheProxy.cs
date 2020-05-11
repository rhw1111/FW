using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityCenter.Main.IdentityServer
{
    public interface IIdentityClientRepositoryCacheProxy
    {
        Task<IdentityClient?> QueryByClientID(string clientID, CancellationToken cancellationToken = default);
    }
}
