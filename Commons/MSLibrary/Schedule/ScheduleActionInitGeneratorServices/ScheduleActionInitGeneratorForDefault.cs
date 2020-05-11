using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using MSLibrary.DI;
using MSLibrary.Serializer;
using MSLibrary.Context;
using MSLibrary.LanguageTranslate;
using System.Security.Claims;

namespace MSLibrary.Schedule.ScheduleActionInitGeneratorServices
{
    /// <summary>
    /// 默认的调度动作初始化服务
    /// 配置格式为
    /// {
    ///     "EnvironmentClaimGeneratorName":"环境声明生成器名称",
    ///     "ClaimContextGeneratorName":"声明上下文生成器名称"
    /// }
    /// </summary>
    [Injection(InterfaceType = typeof(ScheduleActionInitGeneratorServiceForDefault), Scope = InjectionScope.Singleton)]
    public class ScheduleActionInitGeneratorServiceForDefault : IScheduleActionInitGeneratorService
    {
        private IEnvironmentClaimGeneratorRepositoryCacheProxy _environmentClaimGeneratorRepositoryCacheProxy;
        private IClaimContextGeneratorRepositoryCacheProxy _claimContextGeneratorRepositoryCacheProxy;

        public ScheduleActionInitGeneratorServiceForDefault(IEnvironmentClaimGeneratorRepositoryCacheProxy environmentClaimGeneratorRepositoryCacheProxy, IClaimContextGeneratorRepositoryCacheProxy claimContextGeneratorRepositoryCacheProxy)
        {
            _environmentClaimGeneratorRepositoryCacheProxy = environmentClaimGeneratorRepositoryCacheProxy;
            _claimContextGeneratorRepositoryCacheProxy = claimContextGeneratorRepositoryCacheProxy;
        }

        public async Task<IScheduleActionInit> Generator(string configiration)
        {
            var configurationObj = JsonSerializerHelper.Deserialize<Configuration>(configiration);

            var environmentClaimGenerator = await _environmentClaimGeneratorRepositoryCacheProxy.QueryByName(configurationObj.EnvironmentClaimGeneratorName);
            var claimContextGenerator = await _claimContextGeneratorRepositoryCacheProxy.QueryByName(configurationObj.ClaimContextGeneratorName);



            if (environmentClaimGenerator == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundEnvironmentClaimGeneratorByName,
                    DefaultFormatting = "没有找到名称为{0}的环境声明生成器",
                    ReplaceParameters = new List<object>() { configurationObj.EnvironmentClaimGeneratorName }
                };

                throw new UtilityException((int)Errors.NotFoundEnvironmentClaimGeneratorByName, fragment);
            }

            if (claimContextGenerator == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundClaimContextGeneratorByName,
                    DefaultFormatting = "没有找到名称为{0}的上下文生成器",
                    ReplaceParameters = new List<object>() { configurationObj.ClaimContextGeneratorName }
                };

                throw new UtilityException((int)Errors.NotFoundClaimContextGeneratorByName, fragment);
            }

            var claims = await environmentClaimGenerator.Generate();

            return new ScheduleActionInitDefault(claimContextGenerator, claims);
        }



        [DataContract]
        private class Configuration
        {
            [DataMember]
            public string EnvironmentClaimGeneratorName { get; set; }
            [DataMember]
            public string ClaimContextGeneratorName { get; set; }
        }
    }

    public class ScheduleActionInitDefault : IScheduleActionInit
    {
        private ClaimContextGenerator _claimContextGenerator;
        private ClaimsIdentity _identity;

        public ScheduleActionInitDefault(ClaimContextGenerator claimContextGenerator, ClaimsIdentity identity)
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
