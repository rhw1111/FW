using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
 

namespace IdentityCenter.Main.IdentityServer
{
    public interface IIdentityRefreshTokenRepository
    {
        Task<IdentityRefreshToken?> QueryByHandle(string handle, CancellationToken cancellationToken = default);
        Task<IList<IdentityRefreshToken>> QueryBySubjectClient(string subjecId, string clientId,int top, CancellationToken cancellationToken = default);
        Task Delete(IList<Guid> idList, CancellationToken cancellationToken = default);

    }
}
