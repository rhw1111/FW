using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.Serializer;
namespace IdentityCenter.Main.IdentityServer
{
    [Injection(InterfaceType = typeof(IIdentityConsentFactory), Scope = InjectionScope.Singleton)]
    public class IdentityConsentFactory : IIdentityConsentFactory
    {
        public async Task<IdentityConsent> Create(string serializeData)
        {
            var identityConsentData = JsonSerializerHelper.Deserialize<IdentityConsentData>(serializeData);

            IdentityConsent consent = new IdentityConsent()
            {
                ID = identityConsentData.ID,
                ClientId = identityConsentData.ClientId,
                SubjectId = identityConsentData.SubjectId,
                CreationTime = identityConsentData.CreationTime,
                Expiration = identityConsentData.Expiration,
                Scopes = identityConsentData.Scopes
            };

            return await Task.FromResult(consent);
        }
    }
}
