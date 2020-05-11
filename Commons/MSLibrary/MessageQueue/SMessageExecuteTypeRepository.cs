using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.MessageQueue.DAL;
using MSLibrary.DI;

namespace MSLibrary.MessageQueue
{
    [Injection(InterfaceType = typeof(ISMessageExecuteTypeRepository), Scope = InjectionScope.Singleton)]
    public class SMessageExecuteTypeRepository : ISMessageExecuteTypeRepository
    {
        private ISMessageExecuteTypeStore _sMessageExecuteTypeStore;

        public SMessageExecuteTypeRepository(ISMessageExecuteTypeStore sMessageExecuteTypeStore)
        {
            _sMessageExecuteTypeStore = sMessageExecuteTypeStore;
        }
        public async Task<QueryResult<SMessageExecuteType>> Query(string name, int page, int pageSize)
        {
            return await _sMessageExecuteTypeStore.Query(name, page, pageSize);
        }

        public async Task<SMessageExecuteType> QueryByName(string name)
        {
            return await _sMessageExecuteTypeStore.QueryByName(name);
        }
    }
}
