using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Schedule
{
    public interface IScheduleActionRepositoryCacheProxy
    {
        Task<ScheduleAction> QueryByName(string name);
    }
}
