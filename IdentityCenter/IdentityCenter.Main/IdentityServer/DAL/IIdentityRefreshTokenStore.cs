using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityCenter.Main.IdentityServer.DAL
{
    public interface IIdentityRefreshTokenStore
    {
        Task Add(IdentityRefreshToken token, CancellationToken cancellationToken = default);
        Task Update(IdentityRefreshToken token, CancellationToken cancellationToken = default);
        Task Delete(Guid id, CancellationToken cancellationToken = default);
        Task Delete(IList<Guid> idList, CancellationToken cancellationToken = default);
        Task<IdentityRefreshToken?> QueryByHandle(string handle, CancellationToken cancellationToken = default);
        Task<IList<IdentityRefreshToken>> QueryBySubjectClient(string subjecId, string clientId, int top, CancellationToken cancellationToken = default);
    }
}
