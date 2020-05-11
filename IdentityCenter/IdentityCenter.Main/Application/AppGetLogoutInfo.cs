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
    [Injection(InterfaceType = typeof(IAppGetLogoutInfo), Scope = InjectionScope.Singleton)]
    public class AppGetLogoutInfo : IAppGetLogoutInfo
    {
        private readonly ISystemConfigurationService _systemConfigurationService;
        private readonly IIdentityHostRepositoryCacheProxy _identityHostRepositoryCacheProxy;

        public AppGetLogoutInfo(ISystemConfigurationService systemConfigurationService,IIdentityHostRepositoryCacheProxy identityHostRepositoryCacheProxy)  
        {
            _systemConfigurationService = systemConfigurationService;
            _identityHostRepositoryCacheProxy = identityHostRepositoryCacheProxy;
        }

        public async Task<LogoutInfoModel> Do(CancellationToken cancellationToken = default)
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

            return new LogoutInfoModel()
            {
                LoggedPage = host.LoggedPage,
                ExternalLogoutCallbackUri = host.ExternalLogoutCallbackUri

            };
        }
    }
}
