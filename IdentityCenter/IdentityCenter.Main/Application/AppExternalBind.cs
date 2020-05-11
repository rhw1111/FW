using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using MSLibrary.Transaction;
using IdentityCenter.Main.Entities;
using IdentityCenter.Main.IdentityServer;
using IdentityCenter.Main.DTOModel;

namespace IdentityCenter.Main.Application
{
    [Injection(InterfaceType = typeof(IAppExternalBind), Scope = InjectionScope.Singleton)]
    public class AppExternalBind : IAppExternalBind
    {
        private readonly IUserAccountPasswordValidateService _userAccountPasswordValidateService;
        private readonly IIdentityProviderRepositoryCacheProxy _identityProviderRepository;

        public AppExternalBind(IUserAccountPasswordValidateService userAccountPasswordValidateService, IIdentityProviderRepositoryCacheProxy identityProviderRepository)
        {
            _userAccountPasswordValidateService = userAccountPasswordValidateService;
            _identityProviderRepository = identityProviderRepository;
        }

        public async Task<ExternalBindResult> Do(ExternalBindUser bindUser, AuthenticateResult authenticateResult, CancellationToken cancellationToken = default)
        {
            var schemeName = authenticateResult.Properties.Items["scheme"];
            var provider = await _identityProviderRepository.QueryBySchemeName(schemeName, cancellationToken);
            if (provider == null)
            {
                var fragment = new TextFragment()
                {
                    Code = IdentityCenterTextCodes.NotFoundIdentityProviderBySchemeName,
                    DefaultFormatting = "找不到SchemeName为{0}的认证方",
                    ReplaceParameters = new List<object>() { schemeName }
                };

                throw new UtilityException((int)IdentityCenterErrorCodes.NotFoundIdentityProviderBySchemeName, fragment, 1, 0);
            }

            if (!provider.Active)
            {
                var fragment = new TextFragment()
                {
                    Code = IdentityCenterTextCodes.IdentityProviderNotActiveBySchemeName,
                    DefaultFormatting = "SchemeName为{0}的认证方未激活",
                    ReplaceParameters = new List<object>() { schemeName }
                };

                throw new UtilityException((int)IdentityCenterErrorCodes.IdentityProviderNotActiveBySchemeName, fragment, 1, 0);
            }
            UserAccount account;
            UserThirdPartyAccount partyAccount;
            var principalResult = await provider.GetClaims(authenticateResult.Principal, cancellationToken);
            if (bindUser==null)
            {
                //bindUser为空，表示创建新的用户账号
                 account = new UserAccount()
                {
                    ID = Guid.NewGuid(),
                    Name = Guid.NewGuid().ToString(),
                    Password = Guid.NewGuid().ToString(),
                    CreateTime = DateTime.UtcNow,
                    ModifyTime = DateTime.UtcNow,
                    Active = true
                };

                 partyAccount = new UserThirdPartyAccount()
                {
                    ID = Guid.NewGuid(),
                    Source = provider.Type,
                    ThirdPartyID = principalResult.ProviderUserId,
                    AccountID = account.ID,
                    CreateTime = DateTime.UtcNow,
                    ModifyTime = DateTime.UtcNow,
                };

                await provider.InitUserAccount(authenticateResult, account, partyAccount);

                await using (DBTransactionScope scope = new DBTransactionScope(System.Transactions.TransactionScopeOption.Required, new System.Transactions.TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = new TimeSpan(0, 0, 30) }))
                {
                    await account.Add(cancellationToken);
                    await account.AddThirdPartyAccount(partyAccount, cancellationToken);
                    scope.Complete();
                }

            }
            else
            {
                //需要绑定现有账号
                 account=await _userAccountPasswordValidateService.Validate(bindUser.Username, bindUser.Password, cancellationToken);

                 partyAccount = new UserThirdPartyAccount()
                {
                    ID = Guid.NewGuid(),
                    Source = provider.Type,
                    ThirdPartyID = principalResult.ProviderUserId,
                    AccountID = account.ID,
                    CreateTime = DateTime.UtcNow,
                    ModifyTime = DateTime.UtcNow,
                };

                await account.AddThirdPartyAccount(partyAccount, cancellationToken);
            }

            ExternalBindResult result = new ExternalBindResult()
            {
                ProviderUserId = partyAccount.ThirdPartyID,
                ReturnUrl = authenticateResult.Properties.Items["returnUrl"] ?? "~/",
                SchemeName = schemeName,
                SubjectID = account.ID.ToString(),
                UserName = account.Name
            };

            return result;
        }
    }
}
