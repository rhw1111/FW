using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using IdentityCenter.Main.DTOModel;
using IdentityCenter.Main.Configuration;
using IdentityCenter.Main.IdentityServer;

namespace IdentityCenter.Main.Application
{
    [Injection(InterfaceType = typeof(IAppExternalLoginPre), Scope = InjectionScope.Singleton)]
    public class AppExternalLoginPre : IAppExternalLoginPre
    {
        private readonly ISystemConfigurationService _systemConfigurationService;
        private readonly IIdentityProviderRepositoryCacheProxy _identityProviderRepository;
        private readonly IIdentityHostRepositoryCacheProxy _identityHostRepositoryCacheProxy;

        public AppExternalLoginPre(ISystemConfigurationService systemConfigurationService, IIdentityProviderRepositoryCacheProxy identityProviderRepository,
            IIdentityHostRepositoryCacheProxy identityHostRepositoryCacheProxy
            )
        {
            _systemConfigurationService = systemConfigurationService;
            _identityProviderRepository = identityProviderRepository;
            _identityHostRepositoryCacheProxy = identityHostRepositoryCacheProxy;
        }


        public async Task<ExternalLoginPreResult> Do(string schemeName, CancellationToken cancellationToken = default)
        {
            var provider=await _identityProviderRepository.QueryBySchemeName(schemeName, cancellationToken);
            if (provider==null)
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


            ExternalLoginPreResult result = new ExternalLoginPreResult()
            {
                ExternalCallbackUri = host.ExternalCallbackUri
            };


            return result;
        }
    }
}
