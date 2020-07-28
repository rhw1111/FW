using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MSLibrary.Schedule.Application
{
    public interface IAppGetScheduleController
    {
        Task<IScheduleController> Do(CancellationToken cancellationToken);
    }

    public interface IScheduleController
    {
        Task Stop();
    }
}
