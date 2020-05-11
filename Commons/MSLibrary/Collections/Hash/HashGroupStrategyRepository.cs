using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.Collections.Hash.DAL;
using MSLibrary.DI;

namespace MSLibrary.Collections.Hash
{
    [Injection(InterfaceType = typeof(IHashGroupStrategyRepository), Scope = InjectionScope.Singleton)]
    public class HashGroupStrategyRepository : IHashGroupStrategyRepository
    {
        private IHashGroupStrategyStore _hashGroupStrategyStore;

        public HashGroupStrategyRepository(IHashGroupStrategyStore hashGroupStrategyStore)
        {
            _hashGroupStrategyStore = hashGroupStrategyStore;
        }
        public async Task<HashGroupStrategy> QueryById(Guid id)
        {
             return await _hashGroupStrategyStore.QueryById(id);
        }

        public async Task<QueryResult<HashGroupStrategy>> QueryByName(string name, int page, int pageSize)
        {
            return await _hashGroupStrategyStore.QueryByName(name,page,pageSize);
        }
    }
}
