using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.Security.BusinessSecurityRule.DAL;

namespace MSLibrary.Security.BusinessSecurityRule
{
    [Injection(InterfaceType = typeof(IBusinessActionGroupRepository), Scope = InjectionScope.Singleton)]
    public class BusinessActionGroupRepository : IBusinessActionGroupRepository
    {
        private IBusinessActionGroupStore _businessActionGroupStore;

        public BusinessActionGroupRepository(IBusinessActionGroupStore businessActionGroupStore)
        {
            _businessActionGroupStore = businessActionGroupStore;
        }
        public async Task<BusinessActionGroup> QueryById(Guid id)
        {
            return await _businessActionGroupStore.QueryById(id);
        }

        public async Task<BusinessActionGroup> QueryByName(string name)
        {
            return await _businessActionGroupStore.QueryByName(name);
        }

        public async Task<QueryResult<BusinessActionGroup>> QueryByName(string name, int page, int pageSize)
        {
            return await _businessActionGroupStore.QueryByName(name, page, pageSize);
        }
    }
}
