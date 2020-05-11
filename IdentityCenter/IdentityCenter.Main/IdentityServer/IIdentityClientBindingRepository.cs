using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityCenter.Main.IdentityServer
{
    public interface IIdentityClientBindingRepository
    {
        IAsyncEnumerable<IdentityClientBinding> QueryAll(CancellationToken cancellationToken = default);
    }
}
