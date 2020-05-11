using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.Xrm.DAL;

namespace MSLibrary.Xrm
{
    [Injection(InterfaceType = typeof(ICrmServiceFactoryRepository), Scope = InjectionScope.Singleton)]
    public class CrmServiceFactoryRepository:ICrmServiceFactoryRepository
    {

        private ICrmServiceFactoryStore _crmServiceFactoryStore;

        public CrmServiceFactoryRepository(ICrmServiceFactoryStore crmServiceFactoryStore)
        {
            _crmServiceFactoryStore = crmServiceFactoryStore;
        }
        /// <summary>
        /// 根据工厂名称查询
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<CrmServiceFactory> QueryByName(string name)
        {
            return await _crmServiceFactoryStore.QueryByName(name);
        }
    }
}
