using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MSLibrary.Schedule
{
    public interface IScheduleHostConfigurationRepositoryCacheProxy
    {
        Task<ScheduleHostConfiguration> QueryByName(string name, CancellationToken cancellationToken = default);
    }
}
