using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.CommonQueue.DAL;

namespace MSLibrary.CommonQueue
{
    [Injection(InterfaceType = typeof(ICommonMessageClientTypeRepository), Scope = InjectionScope.Singleton)]
    public class CommonMessageClientTypeRepository : ICommonMessageClientTypeRepository
    {
        private ICommonMessageClientTypeStore _commonMessageClientTypeStore;

        public CommonMessageClientTypeRepository(ICommonMessageClientTypeStore commonMessageClientTypeStore)
        {
            _commonMessageClientTypeStore = commonMessageClientTypeStore;
        }

        public async Task<CommonMessageClientType> QueryByName(string name)
        {
            return await _commonMessageClientTypeStore.QueryByName(name);
        }
    }
}
