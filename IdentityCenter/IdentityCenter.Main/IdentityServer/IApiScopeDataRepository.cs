using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityCenter.Main.IdentityServer
{
    public interface IApiScopeDataRepository
    {
        Task<IList<ApiScopeData>> QueryAllEnabled(CancellationToken cancellationToken = default);
        Task<IList<ApiScopeData>> QueryEnabled(IList<string> names, CancellationToken cancellationToken = default);
        Task<ApiScopeData?> QueryEnabled(string name, CancellationToken cancellationToken = default);
    }
}
