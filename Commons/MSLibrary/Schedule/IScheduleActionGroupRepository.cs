using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Schedule
{
    /// <summary>
    /// 调度动作组仓储
    /// </summary>
    public interface IScheduleActionGroupRepository
    {
        Task<ScheduleActionGroup> QueryByID(Guid id);
        Task<ScheduleActionGroup> QueryByName(string name);

        Task<QueryResult<ScheduleActionGroup>> QueryByPage(string name, int page, int pageSize);
    }
}
