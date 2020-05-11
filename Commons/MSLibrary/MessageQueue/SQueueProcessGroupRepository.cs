using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.MessageQueue.DAL;
using MSLibrary.DI;
using System.Threading.Tasks;

namespace MSLibrary.MessageQueue
{
    [Injection(InterfaceType = typeof(ISQueueProcessGroupRepository), Scope = InjectionScope.Singleton)]
    public class SQueueProcessGroupRepository : ISQueueProcessGroupRepository
    {
        private ISQueueProcessGroupStore _sQueueProcessGroupStore;

        public SQueueProcessGroupRepository(ISQueueProcessGroupStore sQueueProcessGroupStore)
        {
            _sQueueProcessGroupStore = sQueueProcessGroupStore;
        }
        public async Task<SQueueProcessGroup> QueryByName(string name)
        {
            return await _sQueueProcessGroupStore.QueryByName(name);
        }

        public async Task<QueryResult<SQueueProcessGroup>> QueryByName(string name, int page, int pageSize)
        {
            return await _sQueueProcessGroupStore.QueryByName(name, page, pageSize);
        }
    }
}
