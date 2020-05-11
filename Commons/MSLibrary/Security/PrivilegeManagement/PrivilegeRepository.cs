using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.Security.PrivilegeManagement.DAL;
using MSLibrary.DI;

namespace MSLibrary.Security.PrivilegeManagement
{
    [Injection(InterfaceType = typeof(IPrivilegeRepository), Scope = InjectionScope.Singleton)]
    public class PrivilegeRepository : IPrivilegeRepository
    {
        private IPrivilegeStore _privilegeStore;

        public PrivilegeRepository(IPrivilegeStore privilegeStore)
        {
            _privilegeStore = privilegeStore;
        }
        public async Task<QueryResult<Privilege>> QueryByCriteria(PrivilegeQueryCriteria criteria, int page, int pageSize)
        {
            return await _privilegeStore.QueryByCriteria(criteria, page, pageSize);
        }

        public async Task<Privilege> QueryById(Guid id)
        {
            return await _privilegeStore.QueryById(id);
        }

        public async Task<Privilege> QueryByNameAndUser(string userKey, Guid systemId, string name)
        {
            return await _privilegeStore.QueryByNameAndUser(userKey, systemId, name);
        }
    }
}
