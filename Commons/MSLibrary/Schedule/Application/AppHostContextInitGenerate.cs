using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.Context;
using MSLibrary.LanguageTranslate;
using System.Security.Claims;

namespace MSLibrary.Schedule.Application
{
    [Injection(InterfaceType = typeof(IAppHostContextInitGenerate), Scope = InjectionScope.Singleton)]
    public class AppHostContextInitGenerate : IAppHostContextInitGenerate
    {
        private readonly IGetScheduleHostApplicationNameService _getScheduleHostApplicationNameService;
        private readonly IScheduleHostConfigurationRepositoryCacheProxy _scheduleHostConfigurationRepositoryCacheProxy;
        private readonly IEnvironmentClaimGeneratorRepositoryCacheProxy _environmentClaimGeneratorRepositoryCacheProxy;
        private readonly IClaimContextGeneratorRepositoryCacheProxy _claimContextGeneratorRepositoryCacheProxy;

        public AppHostContextInitGenerate(IGetScheduleHostApplicationNameService getScheduleHostApplicationNameService, IScheduleHostConfigurationRepositoryCacheProxy scheduleHostConfigurationRepositoryCacheProxy, IEnvironmentClaimGeneratorRepositoryCacheProxy environmentClaimGeneratorRepositoryCacheProxy, IClaimContextGeneratorRepositoryCacheProxy claimContextGeneratorRepositoryCacheProxy)
        {
            _getScheduleHostApplicationNameService = getScheduleHostApplicationNameService;
            _scheduleHostConfigurationRepositoryCacheProxy = scheduleHostConfigurationRepositoryCacheProxy;
            _environmentClaimGeneratorRepositoryCacheProxy = environmentClaimGeneratorRepositoryCacheProxy;
            _claimContextGeneratorRepositoryCacheProxy = claimContextGeneratorRepositoryCacheProxy;
        }


        public async Task<IHostContextInit> Do(CancellationToken cancellationToken)
        {
            //获取当前应用名称
            var applicationName = await _getScheduleHostApplicationNameService.Get(cancellationToken);
            //获取该应用的主机配置
            var hostConfiguration = await _scheduleHostConfigurationRepositoryCacheProxy.QueryByName(applicationName, cancellationToken);
            if (hostConfiguration == null)
            {

                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundScheduleHostConfigurationByName,
                    DefaultFormatting = "找不到应用名称为{0}的调度主机配置",
                    ReplaceParameters = new List<object>() { applicationName }
                };

                throw new UtilityException((int)Errors.NotFoundScheduleHostConfigurationByName, fragment);
            }

            var environmentClaimGenerator = await _environmentClaimGeneratorRepositoryCacheProxy.QueryByName(hostConfiguration.EnvironmentClaimGeneratorName);
            var claimContextGenerator = await _claimContextGeneratorRepositoryCacheProxy.QueryByName(hostConfiguration.ClaimContextGeneratorName);



            if (environmentClaimGenerator == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundEnvironmentClaimGeneratorByName,
                    DefaultFormatting = "没有找到名称为{0}的环境声明生成器",
                    ReplaceParameters = new List<object>() { hostConfiguration.EnvironmentClaimGeneratorName }
                };

                throw new UtilityException((int)Errors.NotFoundEnvironmentClaimGeneratorByName, fragment);
            }

            if (claimContextGenerator == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundClaimContextGeneratorByName,
                    DefaultFormatting = "没有找到名称为{0}的上下文生成器",
                    ReplaceParameters = new List<object>() { hostConfiguration.ClaimContextGeneratorName }
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
