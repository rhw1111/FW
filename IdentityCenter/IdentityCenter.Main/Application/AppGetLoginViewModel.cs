using IdentityCenter.Main.DTOModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using IdentityCenter.Main.Configuration;
using IdentityCenter.Main.IdentityServer;

namespace IdentityCenter.Main.Application
{
    [Injection(InterfaceType = typeof(IAppGetLoginViewModel), Scope = InjectionScope.Singleton)]
    public class AppGetLoginViewModel : IAppGetLoginViewModel
    {
        private readonly ISystemConfigurationService _systemConfigurationService;
        private readonly IIdentityProviderRepositoryCacheProxy _identityProviderRepository;
        private readonly IIdentityHostRepositoryCacheProxy _identityHostRepositoryCacheProxy;

        public AppGetLoginViewModel(ISystemConfigurationService systemConfigurationService, IIdentityProviderRepositoryCacheProxy identityProviderRepository,
            IIdentityHostRepositoryCacheProxy identityHostRepositoryCacheProxy
            )
        {
            _systemConfigurationService = systemConfigurationService;
            _identityProviderRepository = identityProviderRepository;
            _identityHostRepositoryCacheProxy = identityHostRepositoryCacheProxy;
        }
        public async Task<LoginViewModel> Do(IList<string> schemeNames, CancellationToken cancellationToken = default)
        {
            LoginViewModel model = new LoginViewModel()
            {
                IdentityProviders = new List<IdentityProviderModel>()
            };

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

            var localLoginSetting= host.LocalLoginSetting;
            model.AllowRememberLogin = localLoginSetting.AllowRememberLogin;
            model.EnableLocalLogin = localLoginSetting.AllowLocalLogin;

            var providers=await _identityProviderRepository.QueryBySchemeName(schemeNames, cancellationToken);

            foreach(var item in providers)
            {
                if (item.Active)
                {
                    model.IdentityProviders.Add(new IdentityProviderModel() { DisplayName = item.DisplayName, Icon = item.Icon, SchemeName = item.SchemeName });
                }
            }

            return model;
        }
    }
}
