using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using IdentityCenter.Main.IdentityServer;

namespace IdentityCenter.Main.Application
{
    [Injection(InterfaceType = typeof(IAppInitIdentityOption), Scope = InjectionScope.Singleton)]
    public class AppInitIdentityOption : IAppInitIdentityOption
    {
        private readonly IIdentityProviderRepositoryCacheProxy _identityProviderRepositoryCacheProxy;

        public AppInitIdentityOption(IIdentityProviderRepositoryCacheProxy identityProviderRepositoryCacheProxy)
        {
            _identityProviderRepositoryCacheProxy = identityProviderRepositoryCacheProxy;
        }
        public async Task Do<T>(string schemeName, T options, CancellationToken cancellationToken = default)
        {
            var provider=await _identityProviderRepositoryCacheProxy.QueryBySchemeName(schemeName, cancellationToken);
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
            await provider.InitOption(options, cancellationToken);
        }
    }
}
