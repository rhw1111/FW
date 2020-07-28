using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.Schedule.DAL;

namespace MSLibrary.Schedule
{
    [Injection(InterfaceType = typeof(IScheduleHostConfigurationRepository), Scope = InjectionScope.Singleton)]
    public class ScheduleHostConfigurationRepository : IScheduleHostConfigurationRepository
    {
        private readonly IScheduleHostConfigurationStore _scheduleHostConfigurationStore;

        public ScheduleHostConfigurationRepository(IScheduleHostConfigurationStore scheduleHostConfigurationStore)
        {
            _scheduleHostConfigurationStore = scheduleHostConfigurationStore;
        }

        public async Task<ScheduleHostConfiguration> QueryByName(string name, CancellationToken cancellationToken = default)
        {
            return await _scheduleHostConfigurationStore.QueryByName(name, cancellationToken);
        }
    }
}
