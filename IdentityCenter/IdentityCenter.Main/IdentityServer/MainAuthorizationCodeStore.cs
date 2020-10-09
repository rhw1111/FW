using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;
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
    public class MainAuthorizationCodeStore : IAuthorizationCodeStore
    {
        private readonly IIdentityAuthorizationCodeRepository _identityAuthorizationCodeRepository;

        public MainAuthorizationCodeStore(IIdentityAuthorizationCodeRepository identityAuthorizationCodeRepository)
        {
            _identityAuthorizationCodeRepository = identityAuthorizationCodeRepository;
        }

        public async Task<AuthorizationCode> GetAuthorizationCodeAsync(string code)
        {
            var identityCode=await getAuthorizationCode(code,true);
            return await identityCode!.GenerateAuthorizationCode();         
        }

        public async Task RemoveAuthorizationCodeAsync(string code)
        {
            var identityCode = await getAuthorizationCode(code,false);
            if (identityCode!=null)
            {
                await identityCode.Delete();
            }
        }

        public async Task<string> StoreAuthorizationCodeAsync(AuthorizationCode code)
        {
            string strCode = Guid.NewGuid().ToString();
       

            var  properties = new Dictionary<string, string>();
            foreach(var item in code.Properties)
            {
                properties[item.Key] = item.Value;
            }

            IdentityAuthorizationCode identityCode = new IdentityAuthorizationCode()
            {
                ID = Guid.NewGuid(),
                ClientId = code.ClientId,
                Code = strCode,
                CodeChallenge = code.CodeChallenge,
                CodeChallengeMethod = code.CodeChallengeMethod,
                CreationTime = DateTime.UtcNow,            
                IsOpenId = code.IsOpenId,
                Lifetime = code.Lifetime,
                Nonce = code.Nonce,
                Properties = properties,
                RedirectUri = code.RedirectUri,
                RequestedScopes = code.RequestedScopes.ToArray(),
                SessionId = code.SessionId,
                StateHash = code.StateHash,
                SubjectData = await code.Subject.GetBinaryData(),
                WasConsentShown = code.WasConsentShown
            };

            await identityCode.Add();

            return strCode;
        }

        private async Task<IdentityAuthorizationCode?> getAuthorizationCode(string code,bool throwIfNull)
        {
            var identityCode = await _identityAuthorizationCodeRepository.QueryByCode(code);

            if (identityCode == null && throwIfNull)
            {
                var fragment = new TextFragment()
                {
                    Code = IdentityCenterTextCodes.NotFoundIdentityAuthorizationCodeByCode,
                    DefaultFormatting = "找不到Code为{0}的IdentityAuthorizationCode",
                    ReplaceParameters = new List<object>() { code }
                };

                throw new UtilityException((int)IdentityCenterErrorCodes.NotFoundIdentityAuthorizationCodeByCode, fragment, 1, 0);
            }
            return identityCode;
        }
    }
}
