using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.MessageQueue.DAL;
using MSLibrary.DI;

namespace MSLibrary.MessageQueue
{
    [Injection(InterfaceType = typeof(ISMessageRepository), Scope = InjectionScope.Singleton)]
    public class SMessageRepository : ISMessageRepository
    {
        private ISMessageStore _sMessageStore;

        public SMessageRepository(ISMessageStore sMessageStore)
        {
            _sMessageStore = sMessageStore;
        }

        public async Task QueryAllByQueue(SQueue queue, int pageSize, Func<List<SMessage>, Task<bool>> callBack)
        {
            await _sMessageStore.QueryAllByQueue(queue, pageSize, callBack);
        }

        public async Task<QueryResult<SMessage>> QueryByQueue(SQueue queue, int page, int pageSize)
        {
            return await _sMessageStore.QueryByQueue(queue, page, pageSize);
        }
    }
}
