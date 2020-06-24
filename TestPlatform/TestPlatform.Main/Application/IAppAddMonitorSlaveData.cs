using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FW.TestPlatform.Main.DTOModel;

namespace FW.TestPlatform.Main.Application
{
    public interface IAppAddMonitorSlaveData
    {
        Task Do(IList<MonitorSlaveDataAddModel> modelList, CancellationToken cancellationToken = default);
    }
}
