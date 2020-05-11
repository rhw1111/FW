using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Schedule
{
    public interface IScheduleActionGroupRepositoryCacheProxy
    {
        Task<ScheduleActionGroup> QueryByName(string name);
    }
}
