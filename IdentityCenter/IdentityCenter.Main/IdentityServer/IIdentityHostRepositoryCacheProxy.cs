using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace IdentityCenter.Main.IdentityServer
{

    public interface IIdentityHostRepositoryCacheProxy
    {
        Task<IdentityHost?> QueryByName(string name, CancellationToken cancellationToken = default);
    }
}
