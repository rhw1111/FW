using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.Schedule.Application
{
    [Injection(InterfaceType = typeof(IAppGetScheduleController), Scope = InjectionScope.Singleton)]
    public class AppGetScheduleController : IAppGetScheduleController
    {
        private readonly IGetScheduleHostApplicationNameService _getScheduleHostApplicationNameService;
        private readonly IScheduleHostConfigurationRepositoryCacheProxy _scheduleHostConfigurationRepositoryCacheProxy;
        private readonly IScheduleActionGroupRepositoryCacheProxy _scheduleActionGroupRepositoryCacheProxy;

        public AppGetScheduleController(IGetScheduleHostApplicationNameService getScheduleHostApplicationNameService, IScheduleHostConfigurationRepositoryCacheProxy scheduleHostConfigurationRepositoryCacheProxy, IScheduleActionGroupRepositoryCacheProxy scheduleActionGroupRepositoryCacheProxy)
        {
            _getScheduleHostApplicationNameService = getScheduleHostApplicationNameService;
            _scheduleHostConfigurationRepositoryCacheProxy = scheduleHostConfigurationRepositoryCacheProxy;
            _scheduleActionGroupRepositoryCacheProxy = scheduleActionGroupRepositoryCacheProxy;
        }

        public async Task<IScheduleController> Do(CancellationToken cancellationToken)
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

            var group = await _scheduleActionGroupRepositoryCacheProxy.QueryByName(hostConfiguration.ScheduleGroupName);
            if (group == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundScheduleActionGroupByName,
                    DefaultFormatting = "找不到名称为{0}的调度动作组",
                    ReplaceParameters = new List<object>() { hostConfiguration.ScheduleGroupName }
                };

                throw new UtilityException((int)Errors.NotFoundScheduleActionGroupByName, fragment);
            }

            ScheduleControllerDefaut result = new ScheduleControllerDefaut(group);

            await group.Start();

            return result;
        }
    }

    public class ScheduleControllerDefaut : IScheduleController
    {
        private readonly ScheduleActionGroup _group;

        public ScheduleControllerDefaut(ScheduleActionGroup group)
        {
            _group = group;
        }
        public async Task Stop()
        {
            await _group.Shutdown();
        }
    }
}
