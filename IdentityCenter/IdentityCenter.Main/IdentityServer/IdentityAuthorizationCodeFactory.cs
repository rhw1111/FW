using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.Serializer;

namespace IdentityCenter.Main.IdentityServer
{
    [Injection(InterfaceType = typeof(IIdentityAuthorizationCodeFactory), Scope = InjectionScope.Singleton)]
    public class IdentityAuthorizationCodeFactory : IIdentityAuthorizationCodeFactory
    {
        public async Task<IdentityAuthorizationCode> Create(string serializeData)
        {
            var authorizationCodeData = JsonSerializerHelper.Deserialize<IdentityAuthorizationCodeData>(serializeData);

            IdentityAuthorizationCode authorizationCode = new IdentityAuthorizationCode()
            {
                ID = authorizationCodeData.ID,
                ClientId = authorizationCodeData.ClientId,
                CodeChallenge = authorizationCodeData.CodeChallenge,
                CodeChallengeMethod = authorizationCodeData.CodeChallengeMethod,
                CreationTime = authorizationCodeData.CreationTime,
                IsOpenId = authorizationCodeData.IsOpenId,
                Lifetime = authorizationCodeData.Lifetime,
                Nonce = authorizationCodeData.Nonce,
                Properties = authorizationCodeData.Properties,
                RedirectUri = authorizationCodeData.RedirectUri,
                RequestedScopes = authorizationCodeData.RequestedScopes,
                SessionId = authorizationCodeData.SessionId,
                StateHash = authorizationCodeData.StateHash,
                SubjectData = authorizationCodeData.SubjectData,
                WasConsentShown = authorizationCodeData.WasConsentShown
            };

            return await Task.FromResult(authorizationCode);
        }
    }
}
