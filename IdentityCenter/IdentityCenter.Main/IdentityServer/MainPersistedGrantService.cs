using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;

namespace IdentityCenter.Main.IdentityServer
{
    public class MainPersistedGrantService : IPersistedGrantService
    {
        private readonly IIdentityConsentRepository _identityConsentRepository;
        private readonly IUserConsentStore _userConsentStore;
        private readonly IRefreshTokenStore _refreshTokenStore;

        public MainPersistedGrantService(IIdentityConsentRepository identityConsentRepository, IUserConsentStore userConsentStore, IRefreshTokenStore refreshTokenStore)
        {
            _identityConsentRepository = identityConsentRepository;
            _userConsentStore = userConsentStore;
            _refreshTokenStore = refreshTokenStore;
        }
        public async Task<IEnumerable<Consent>> GetAllGrantsAsync(string subjectId)
        {
            var identityConsents = await _identityConsentRepository.QueryBySubject(subjectId);
            List<Consent> consents = new List<Consent>();

            foreach(var item in identityConsents)
            {
                consents.Add(await item.GenerateConsent());
            }

            return consents;
        }

        public async Task RemoveAllGrantsAsync(string subjectId, string clientId)
        {
            await _refreshTokenStore.RemoveRefreshTokensAsync(subjectId, clientId);
            await _userConsentStore.RemoveUserConsentAsync(subjectId, clientId);
        }
    }
}
