using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityCenter.Main.IdentityServer
{
    public interface IIdentityConsentRepository
    {
        Task<IdentityConsent?> QueryBySubjectClient(string subjectId, string clientId, CancellationToken cancellationToken = default);
        Task<IList<IdentityConsent>> QueryBySubject(string subjectId, CancellationToken cancellationToken = default);
    }
}
