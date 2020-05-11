using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using IdentityCenter.Main.Entities;
using IdentityCenter.Main.Configuration;
using IdentityCenter.Main.IdentityServer;
using IdentityCenter.Main.DTOModel;

namespace IdentityCenter.Main.Application
{
    [Injection(InterfaceType = typeof(IAppExternalLoginCallback), Scope = InjectionScope.Singleton)]
    public class AppExternalLoginCallback : IAppExternalLoginCallback
    {
        private readonly ISystemConfigurationService _systemConfigurationService;
        private readonly IIdentityProviderRepositoryCacheProxy _identityProviderRepository;
        private readonly IUserAccountRepository _userAccountRepository;
        private readonly IIdentityHostRepositoryCacheProxy _identityHostRepositoryCacheProxy;

        public AppExternalLoginCallback(ISystemConfigurationService systemConfigurationService, IIdentityProviderRepositoryCacheProxy identityProviderRepository, IUserAccountRepository userAccountRepository,
            IIdentityHostRepositoryCacheProxy identityHostRepositoryCacheProxy
            )
        {
            _systemConfigurationService = systemConfigurationService;
            _identityProviderRepository = identityProviderRepository;
            _userAccountRepository = userAccountRepository;
            _identityHostRepositoryCacheProxy = identityHostRepositoryCacheProxy;
        }
        public async Task<ExternalLoginCallbackResult> Do(AuthenticateResult authenticateResult, CancellationToken cancellationToken = default)
        {
            ExternalLoginCallbackResult result = new ExternalLoginCallbackResult();

            var schemeName = authenticateResult.Properties.Items["scheme"];
            var provider = await _identityProviderRepository.QueryBySchemeName(schemeName,cancellationToken);
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

            var claimsResult=await provider.GetClaims(authenticateResult.Principal,cancellationToken);

            result.ProviderUserId = claimsResult.ProviderUserId;
            result.SchemeName = schemeName;
            result.ReturnUrl = authenticateResult.Properties.Items["returnUrl"] ?? "~/";

            var userAccount=await _userAccountRepository.QueryByThirdParty(provider.Type, claimsResult.ProviderUserId, cancellationToken);
            if (userAccount!=null)
            {             
                result.ExistsUserAccount = true;
                result.SubjectID = userAccount.ID.ToString();
                result.UserName = userAccount.Name;
            }
            else
            {
                var appName = await _systemConfigurationService.GetIdentityHostApplicationName(cancellationToken);

                var host = await _identityHostRepositoryCacheProxy.QueryByName(appName, cancellationToken);
                if (host == null)
                {
                    var fragment = new TextFragment()
                    {
                        Code = IdentityCenterTextCodes.NotFoundIdentityHostByName,
                        DefaultFormatting = "找不到名称为{0}的认证主机",
                        ReplaceParameters = new List<object>() { appName }
                    };

                    throw new UtilityException((int)IdentityCenterErrorCodes.NotFoundIdentityHostByName, fragment, 1, 0);
                }

                result.ExistsUserAccount = false;
                result.ExternalIdentityBindPage = host.ExternalIdentityBindPage;
            }

            return result;
        }
    }
}
