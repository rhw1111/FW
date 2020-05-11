using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.Context;
using MSLibrary.LanguageTranslate;
using IdentityCenter.Main.Configuration;
using IdentityCenter.Main.IdentityServer;


namespace IdentityCenter.Main.Application
{
    [Injection(InterfaceType = typeof(IAppHostContextInitGenerate), Scope = InjectionScope.Singleton)]
    public class AppHostContextInitGenerate : IAppHostContextInitGenerate
    {
        private readonly IIdentityHostRepositoryCacheProxy _identityHostRepositoryCacheProxy;
        private readonly ISystemConfigurationService _systemConfigurationService;
        private readonly IEnvironmentClaimGeneratorRepositoryCacheProxy _environmentClaimGeneratorRepositoryCacheProxy;
        private readonly IClaimContextGeneratorRepositoryCacheProxy _claimContextGeneratorRepositoryCacheProxy;

        public AppHostContextInitGenerate(IIdentityHostRepositoryCacheProxy identityHostRepositoryCacheProxy, ISystemConfigurationService systemConfigurationService,
            IEnvironmentClaimGeneratorRepositoryCacheProxy environmentClaimGeneratorRepositoryCacheProxy,
            IClaimContextGeneratorRepositoryCacheProxy claimContextGeneratorRepositoryCacheProxy
            )
        {
            _identityHostRepositoryCacheProxy = identityHostRepositoryCacheProxy;
            _systemConfigurationService = systemConfigurationService;
            _environmentClaimGeneratorRepositoryCacheProxy = environmentClaimGeneratorRepositoryCacheProxy;
            _claimContextGeneratorRepositoryCacheProxy = claimContextGeneratorRepositoryCacheProxy;
        }
        public async Task<IHostContextInit> Do()
        {
            var appName = await _systemConfigurationService.GetIdentityHostApplicationName();

            var host = await _identityHostRepositoryCacheProxy.QueryByName(appName);
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


            var environmentClaimGenerator = await _environmentClaimGeneratorRepositoryCacheProxy.QueryByName(host.EnvironmentClaimGeneratorName);
            var claimContextGenerator = await _claimContextGeneratorRepositoryCacheProxy.QueryByName(host.ClaimContextGeneratorName);



            if (environmentClaimGenerator == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundEnvironmentClaimGeneratorByName,
                    DefaultFormatting = "没有找到名称为{0}的环境声明生成器",
                    ReplaceParameters = new List<object>() { host.EnvironmentClaimGeneratorName }
                };

                throw new UtilityException((int)Errors.NotFoundEnvironmentClaimGeneratorByName, fragment);
            }

            if (claimContextGenerator == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundClaimContextGeneratorByName,
                    DefaultFormatting = "没有找到名称为{0}的上下文生成器",
                    ReplaceParameters = new List<object>() { host.ClaimContextGeneratorName }
                };

                throw new UtilityException((int)Errors.NotFoundClaimContextGeneratorByName, fragment);
            }

            var claims = await environmentClaimGenerator.Generate();


            return new HostContextInitDefault(claimContextGenerator, claims);

        }
    }

    public class HostContextInitDefault : IHostContextInit
    {
        private readonly ClaimContextGenerator _claimContextGenerator;
        private readonly ClaimsIdentity _identity;

        public HostContextInitDefault(ClaimContextGenerator claimContextGenerator, ClaimsIdentity identity)
        {
            _claimContextGenerator = claimContextGenerator;
            _identity = identity;
        }

        public void Init()
        {
            _claimContextGenerator.ContextInit(_identity.Claims);
        }
    }

}
