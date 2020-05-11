using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.Collections.Hash.DAL;

namespace MSLibrary.Collections.Hash
{
    [Injection(InterfaceType = typeof(IHashGroupRepository), Scope = InjectionScope.Singleton)]
    public class HashGroupRepository : IHashGroupRepository
    {
        private IHashGroupStore _hashGroupStore;

        public HashGroupRepository(IHashGroupStore hashGroupStore)
        {
            _hashGroupStore = hashGroupStore;
        }

        public async Task<HashGroup> QueryById(Guid id)
        {
            return await _hashGroupStore.QueryById(id);
        }

        public async Task<HashGroup> QueryByName(string name)
        {
            return await _hashGroupStore.QueryByName(name);
        }

        public async Task<QueryResult<HashGroup>> QueryByName(string name, int page, int pageSize)
        {
            return await _hashGroupStore.QueryByName(name,page,pageSize);
        }

        public HashGroup QueryByNameSync(string name)
        {
            return _hashGroupStore.QueryByNameSync(name);
        }

        public async Task QueryByType(string type, Func<HashGroup, Task> action)
        {
            await _hashGroupStore.QueryByType(type, action);
        }
    }
}
