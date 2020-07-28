using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MSLibrary.Schedule
{
    /// <summary>
    /// 获取当前的批处理主机的应用程序名称
    /// </summary>
    public interface IGetScheduleHostApplicationNameService
    {
        Task<string> Get(CancellationToken cancellationToken = default);

    }
}
