using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityCenter.Main.IdentityServer
{
    public interface IIdentityResourceDataRepository
    {
        Task<IList<IdentityResourceData>> QueryAllEnabled(CancellationToken cancellationToken = default);
        Task<IList<IdentityResourceData>> QueryEnabled(IList<string> names, CancellationToken cancellationToken = default);
    }
}
