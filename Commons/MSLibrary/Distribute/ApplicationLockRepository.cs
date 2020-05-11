using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.Distribute.DAL;

namespace MSLibrary.Distribute
{
    [Injection(InterfaceType = typeof(IApplicationLockRepository), Scope = InjectionScope.Singleton)]
    public class ApplicationLockRepository : IApplicationLockRepository
    {
        private IApplicationLockStore _applicationLockStore;

        public ApplicationLockRepository(IApplicationLockStore applicationLockStore)
        {
            _applicationLockStore = applicationLockStore;
        }

        public async Task<ApplicationLock> QueryByName(string name)
        {
            return await _applicationLockStore.QueryByName(name);
        }

        public  ApplicationLock QueryByNameSync(string name)
        {
            return _applicationLockStore.QueryByNameSync(name);
        }
    }
}
