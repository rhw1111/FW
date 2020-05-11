using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityCenter.Main.IdentityServer.DAL
{
    public interface IIdentityClientBindingStore
    {
        IAsyncEnumerable<IdentityClientBinding> QueryAll(CancellationToken cancellationToken = default);
    }
}
