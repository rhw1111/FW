using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary.Schedule;
using MSLibrary.DI;
using FW.TestPlatform.Main.Configuration;

namespace FW.TestPlatform.Main.Schedule
{
    [Injection(InterfaceType = typeof(IGetScheduleHostApplicationNameService), Scope = InjectionScope.Singleton)]
    public class GetScheduleHostApplicationNameService : IGetScheduleHostApplicationNameService
    {
        private readonly ISystemConfigurationService _systemConfigurationService;

        public GetScheduleHostApplicationNameService(ISystemConfigurationService systemConfigurationService)
        {
            _systemConfigurationService = systemConfigurationService;
        }
        public async Task<string> Get(CancellationToken cancellationToken = default)
        {
            return await _systemConfigurationService.GetHostApplicationNameAsync(cancellationToken);
        }
    }
}
