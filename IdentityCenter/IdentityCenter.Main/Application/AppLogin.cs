using IdentityCenter.Main.DTOModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using IdentityCenter.Main.Entities;
using IdentityCenter.Main.Configuration;
using IdentityCenter.Main.IdentityServer;

namespace IdentityCenter.Main.Application
{
    [Injection(InterfaceType = typeof(IAppLogin), Scope = InjectionScope.Singleton)]
    public class AppLogin : IAppLogin
    {
        private readonly IUserAccountPasswordValidateService _userAccountPasswordValidateService;
        private readonly ISystemConfigurationService _systemConfigurationService;
        private readonly IIdentityHostRepositoryCacheProxy _identityHostRepositoryCacheProxy;

        public AppLogin(IUserAccountPasswordValidateService userAccountPasswordValidateService, ISystemConfigurationService systemConfigurationService, IIdentityHostRepositoryCacheProxy identityHostRepositoryCacheProxy)
        {
            _userAccountPasswordValidateService = userAccountPasswordValidateService;
            _systemConfigurationService = systemConfigurationService;
            _identityHostRepositoryCacheProxy = identityHostRepositoryCacheProxy;
        }

        public async Task<LocalLoginResult> Do(LocalLoginRequest request, CancellationToken cancellationToken = default)
        {
            LocalLoginResult result = new LocalLoginResult();
            var appName=await _systemConfigurationService.GetIdentityHostApplicationName(cancellationToken);

            var host=await _identityHostRepositoryCacheProxy.QueryByName(appName, cancellationToken);
            if (host==null)
            {
                var fragment = new TextFragment()
                {
                    Code = IdentityCenterTextCodes.NotFoundIdentityHostByName,
                    DefaultFormatting = "找不到名称为{0}的认证主机",
                    ReplaceParameters = new List<object>() { appName }
                };

                throw new UtilityException((int)IdentityCenterErrorCodes.NotFoundIdentityHostByName, fragment, 1, 0);
            }


            var localLoginSetting = host.LocalLoginSetting ;

            if (localLoginSetting.AllowRememberLogin && request.RememberLogin)
            {
                result.RememberLogin = true;
                result.RememberMeLoginDuration = localLoginSetting.RememberLoginDuration;
            }
            else
            {
                result.RememberLogin = false;
            }

            var userAccount = await _userAccountPasswordValidateService.Validate(request.Username, request.Password, cancellationToken);


            result.SubjectID = userAccount.ID.ToString();
            result.UserName = userAccount.Name;
            return result;

        }
    }
}
