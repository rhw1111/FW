using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using IdentityModel;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using IdentityCenter.Main.Entities;

namespace IdentityCenter.Main.IdentityServer
{
    /// <summary>
    /// IdentityServer的用户信息服务
    /// </summary>
    [Injection(InterfaceType = typeof(IProfileService), Scope = InjectionScope.Transient)]
    public class MainProfileService : IProfileService
    {
        private IUserAccountRepository _userAccountRepository;

        public MainProfileService(IUserAccountRepository userAccountRepository)
        {
            _userAccountRepository = userAccountRepository;
        }
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var strUserID = context.Subject.GetSubjectId();
            var userAccount = await getUserAccount(strUserID);

            var claims = new List<Claim>
            {
                new Claim(UserAccountClaimTypes.Subject, userAccount.ID.ToString()),
                new Claim(UserAccountClaimTypes.Name, userAccount.Name)
            };

            context.IssuedClaims = claims;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var strUserID = context.Subject.GetSubjectId();
            var userAccount = await getUserAccount(strUserID);
            context.IsActive = userAccount.Active;
        }

        private async Task<UserAccount> getUserAccount(string strUserID)
        {
            if (!Guid.TryParse(strUserID, out Guid userID))
            {
                var fragment = new TextFragment()
                {
                    Code = IdentityCenterTextCodes.IdentityServerUserIDFormatError,
                    DefaultFormatting = "认证服务中的用户ID格式不正确，期待的格式为{0}，用户ID为{1}",
                    ReplaceParameters = new List<object>() { typeof(Guid).FullName??string.Empty, strUserID }
                };

                throw new UtilityException((int)IdentityCenterErrorCodes.IdentityServerUserIDFormatError, fragment, 1, 0);
            }

            var userAccount = await _userAccountRepository.QueryByID(userID);
            if (userAccount == null)
            {
                var fragment = new TextFragment()
                {
                    Code = IdentityCenterTextCodes.NotFoundUserAccountByID,
                    DefaultFormatting = "找不到ID为{0}的用户账号",
                    ReplaceParameters = new List<object>() { strUserID }
                };

                throw new UtilityException((int)IdentityCenterErrorCodes.NotFoundUserAccountByID, fragment);

            }

            return userAccount;
        }
    }
}
