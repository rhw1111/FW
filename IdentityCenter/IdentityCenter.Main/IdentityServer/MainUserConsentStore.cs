using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using IdentityServer4.Validation;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using IdentityCenter.Main.Entities;
using IdentityServer4.Stores;
using IdentityServer4.Models;
using System.Linq;

namespace IdentityCenter.Main.IdentityServer
{
    public class MainUserConsentStore : IUserConsentStore
    {
        private readonly IIdentityConsentRepository _identityConsentRepository;

        public MainUserConsentStore(IIdentityConsentRepository identityConsentRepository)
        {
            _identityConsentRepository = identityConsentRepository;
        }

        public async Task<Consent> GetUserConsentAsync(string subjectId, string clientId)
        {
          
            var identityConsent = await _identityConsentRepository.QueryBySubjectClient(subjectId, clientId);
            if (identityConsent==null)
            {
                var fragment = new TextFragment()
                {
                    Code = IdentityCenterTextCodes.NotFoundIdentityConsentBySubjectAndClient,
                    DefaultFormatting = "找不到SubjectId为{0}、ClientId为{1}的认证确认",
                    ReplaceParameters = new List<object>() { subjectId,clientId }
                };

                throw new UtilityException((int)IdentityCenterErrorCodes.NotFoundIdentityConsentBySubjectAndClient, fragment, 1, 0);
            }
            return await identityConsent.GenerateConsent();
        }

        public async Task RemoveUserConsentAsync(string subjectId, string clientId)
        {
            var identityConsent = await _identityConsentRepository.QueryBySubjectClient(subjectId, clientId);
            if (identityConsent != null)
            {
              await identityConsent.Delete();
            }
        }

        public async Task StoreUserConsentAsync(Consent consent)
        {
            IdentityConsent identityConsent = new IdentityConsent()
            {
                ClientId = consent.ClientId,
                SubjectId = consent.SubjectId,
                ID = Guid.NewGuid(),
                CreationTime = consent.CreationTime,
                Expiration = consent.Expiration,
                Scopes = consent.Scopes.ToArray()
            };
            await identityConsent.Add();
        }
    }
}
