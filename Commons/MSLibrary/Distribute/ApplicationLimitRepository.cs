using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.Distribute.DAL;

namespace MSLibrary.Distribute
{
    [Injection(InterfaceType = typeof(IApplicationLimitRepository), Scope = InjectionScope.Singleton)]
    public class ApplicationLimitRepository : IApplicationLimitRepository
    {
        private IApplicationLimitStore _applicationLimitStore;

        public ApplicationLimitRepository(IApplicationLimitStore applicationLimitStore)
        {
            _applicationLimitStore = applicationLimitStore;
        }
        public async Task<ApplicationLimit> QueryByName(string name)
        {
            return await _applicationLimitStore.QueryByName(name);
        }

        public  ApplicationLimit QueryByNameSync(string name)
        {
            return  _applicationLimitStore.QueryByNameSync(name);
        }
    }
}
