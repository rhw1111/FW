using MSLibrary.Storge.DAL;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.Storge.DAL;

namespace MSLibrary.Storge
{
    [Injection(InterfaceType = typeof(IStoreGroupRepository), Scope = InjectionScope.Singleton)]
    public class StoreGroupRepository : IStoreGroupRepository
    {
        private IStoreGroupStore _storeGroupStore;

        public StoreGroupRepository(IStoreGroupStore storeGroupStore)
        {
            _storeGroupStore = storeGroupStore;
        }
        public async Task<StoreGroup> QueryByName(string name)
        {
            return await _storeGroupStore.QueryByName(name);
        }
    }
}
