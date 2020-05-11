using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.Security.BusinessSecurityRule.DAL;

namespace MSLibrary.Security.BusinessSecurityRule
{
    [Injection(InterfaceType = typeof(IBusinessActionRepository), Scope = InjectionScope.Singleton)]
    public class BusinessActionRepository : IBusinessActionRepository
    {
        private IBusinessActionStore _businessActionStore;

        public BusinessActionRepository(IBusinessActionStore businessActionStore)
        {
            _businessActionStore = businessActionStore;
        }

        public async Task<BusinessAction> QueryById(Guid id)
        {
            return await _businessActionStore.QueryById(id);
        }

        public async Task<BusinessAction> QueryByName(string name)
        {
            return await _businessActionStore.QueryByName(name);
        }

        public async Task<QueryResult<BusinessAction>> QueryByName(string name, int page, int pageSize)
        {
            return await _businessActionStore.QueryByName(name, page, pageSize);
        }
    }
}
