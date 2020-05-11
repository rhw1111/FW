using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace IdentityCenter.Main.IdentityServer
{
    public interface IApiResourceDataRepository
    {
        Task<IList<ApiResourceData>> QueryAllEnabled(CancellationToken cancellationToken = default);
        Task<IList<ApiResourceData>> QueryEnabled(IList<string> names, CancellationToken cancellationToken = default);
        Task<ApiResourceData?> QueryEnabled(string name, CancellationToken cancellationToken = default);
    }
}
