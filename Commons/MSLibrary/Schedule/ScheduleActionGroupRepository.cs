using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.Schedule.DAL;
using MSLibrary.DI;

namespace MSLibrary.Schedule
{
    [Injection(InterfaceType = typeof(IScheduleActionGroupRepository), Scope = InjectionScope.Singleton)]
    public class ScheduleActionGroupRepository : IScheduleActionGroupRepository
    {
        private IScheduleActionGroupStore _scheduleActionGroupStore;

        public ScheduleActionGroupRepository(IScheduleActionGroupStore scheduleActionGroupStore)
        {
            _scheduleActionGroupStore = scheduleActionGroupStore;
        }
        public async Task<ScheduleActionGroup> QueryByID(Guid id)
        {
            return await _scheduleActionGroupStore.QueryByID(id);
        }

        public async Task<ScheduleActionGroup> QueryByName(string name)
        {
            return await _scheduleActionGroupStore.QueryByName(name);
        }

        public async Task<QueryResult<ScheduleActionGroup>> QueryByPage(string name, int page, int pageSize)
        {
            return await _scheduleActionGroupStore.QueryByPage(name,page,pageSize);
        }
    }
}
