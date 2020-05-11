using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityCenter.Main.IdentityServer.DAL
{
    public interface IIdentityConsentStore
    {
        Task Add(IdentityConsent consent, CancellationToken cancellationToken = default);
        Task Delete(Guid id, CancellationToken cancellationToken = default);
        Task<IdentityConsent?> QueryBySubjectClient(string subjectId, string clientId, CancellationToken cancellationToken = default);
        Task<IList<IdentityConsent>> QueryBySubject(string subjectId, CancellationToken cancellationToken = default);

    }
}
