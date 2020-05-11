using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.MessageQueue.DAL;

namespace MSLibrary.MessageQueue
{
    [Injection(InterfaceType = typeof(ISQueueRepository), Scope = InjectionScope.Singleton)]
    public class SQueueRepository : ISQueueRepository
    {
        private ISQueueStore _squeueStore;

        public SQueueRepository(ISQueueStore squeueStore)
        {
            _squeueStore = squeueStore;
        }

        public async Task<QueryResult<SQueue>> Query(int page, int pageSize)
        {
            return await _squeueStore.Query(page, pageSize);
        }

        public async Task<SQueue> QueryByCode(string groupName, bool isDead, int code)
        {
            return await _squeueStore.QueryByCode(groupName, isDead, code);
        }

        public async Task<QueryResult<SQueue>> QueryByGroup(string groupName, bool isDead, int page, int pageSize)
        {
            return await _squeueStore.QueryByGroup(groupName, isDead, page, pageSize);
        }

        public async Task<SQueue> QueryById(Guid id)
        {
            return await _squeueStore.QueryById(id);
        }

        public async Task<QueryResult<SQueue>> QueryByNullProcessGroup(int page, int pageSize)
        {
            return await _squeueStore.QueryByNullProcessGroup(page, pageSize);
        }

        public async Task QueryByProcessGroup(string processGroupName, Func<SQueue, Task> callback)
        {
             await _squeueStore.QueryByProceeGroup(processGroupName, callback);
        }
    }
}
