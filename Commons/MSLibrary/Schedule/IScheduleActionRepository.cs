using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Schedule
{
    /// <summary>
    /// 调度作业仓储
    /// </summary>
    public interface IScheduleActionRepository
    {
        Task<ScheduleAction> QueryByID(Guid id);
        Task<ScheduleAction> QueryByName(string name);

        Task<QueryResult<ScheduleAction>> QueryByPage(string name, int page, int pageSize);
    }
}
