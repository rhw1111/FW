using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.Schedule.DAL;
using MSLibrary.DI;

namespace MSLibrary.Schedule
{
    [Injection(InterfaceType = typeof(IScheduleActionRepository), Scope = InjectionScope.Singleton)]
    public class ScheduleActionRepository : IScheduleActionRepository
    {
        private IScheduleActionStore _scheduleActionStore;

        public ScheduleActionRepository(IScheduleActionStore scheduleActionStore)
        {
            _scheduleActionStore = scheduleActionStore;
        }
        public async Task<ScheduleAction> QueryByID(Guid id)
        {
            return await _scheduleActionStore.QueryByID(id);
        }

        public async Task<ScheduleAction> QueryByName(string name)
        {
            return await _scheduleActionStore.QueryByName(name);
        }

        public async Task<QueryResult<ScheduleAction>> QueryByPage(string name, int page, int pageSize)
        {
            return await _scheduleActionStore.QueryByPage(name,page,pageSize);
        }
    }
}
