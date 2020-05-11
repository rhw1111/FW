using IdentityCenter.Main.DTOModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using IdentityCenter.Main.IdentityServer.ClientBindings;
using IdentityCenter.Main.Configuration;

namespace IdentityCenter.Main.Application
{
    [Injection(InterfaceType = typeof(IAppOpenIDRefreshToken), Scope = InjectionScope.Singleton)]
    public class AppOpenIDRefreshToken : IAppOpenIDRefreshToken
    {
        private readonly ISystemConfigurationService _systemConfigurationService;
        private readonly IIdentityClientOpenIDBindingRepositoryCacheProxy _identityClientOpenIDBindingRepositoryCacheProxy;

        public AppOpenIDRefreshToken(ISystemConfigurationService systemConfigurationService,IIdentityClientOpenIDBindingRepositoryCacheProxy identityClientOpenIDBindingRepositoryCacheProxy)
        {
            _systemConfigurationService = systemConfigurationService;
            _identityClientOpenIDBindingRepositoryCacheProxy = identityClientOpenIDBindingRepositoryCacheProxy;
        }
        public async Task<RefreshTokenModel> Do(string bindingName,string refreshToken, CancellationToken cancellationToken = default)
        {
            var binding=await _identityClientOpenIDBindingRepositoryCacheProxy.QueryByName(bindingName, cancellationToken);
            if (binding==null)
            {
                var fragment = new TextFragment()
                {
                    Code = IdentityCenterTextCodes.NotFoundIdentityClientOpenIDBindingByName,
                    DefaultFormatting = "找不到名称为{0}的客户端认证OpenID绑定",
                    ReplaceParameters = new List<object>() { bindingName }
                };

                throw new UtilityException((int)IdentityCenterErrorCodes.NotFoundIdentityClientOpenIDBindingByName, fragment, 1, 0);
            }

            var result = await binding.RefreshToken(refreshToken, cancellationToken);
            return new RefreshTokenModel()
            {
                Token = result.AccessToken,
                RefreshToken = result.RefreshToken,
                Expire = DateTime.UtcNow.AddSeconds(result.ExpireSeconds)
            };
        }
    }
}
