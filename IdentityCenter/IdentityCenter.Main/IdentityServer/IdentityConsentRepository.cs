using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary.DI;
using IdentityCenter.Main.IdentityServer.DAL;

namespace IdentityCenter.Main.IdentityServer
{
    [Injection(InterfaceType = typeof(IIdentityConsentRepository), Scope = InjectionScope.Singleton)]
    public class IdentityConsentRepository : IIdentityConsentRepository
    {
        private readonly IIdentityConsentStore _identityConsentStore;

        public IdentityConsentRepository(IIdentityConsentStore identityConsentStore)
        {
            _identityConsentStore = identityConsentStore;
        }

        public async Task<IList<IdentityConsent>> QueryBySubject(string subjectId, CancellationToken cancellationToken = default)
        {
            return await _identityConsentStore.QueryBySubject(subjectId, cancellationToken);
        }

        public async Task<IdentityConsent?> QueryBySubjectClient(string subjectId, string clientId, CancellationToken cancellationToken = default)
        {
            return await _identityConsentStore.QueryBySubjectClient(subjectId, clientId, cancellationToken);         
        }
    }
}
