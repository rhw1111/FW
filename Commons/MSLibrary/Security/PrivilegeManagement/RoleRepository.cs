using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.Security.PrivilegeManagement.DAL;
using MSLibrary.DI;

namespace MSLibrary.Security.PrivilegeManagement
{
    [Injection(InterfaceType = typeof(IRoleRepository), Scope = InjectionScope.Singleton)]
    public class RoleRepository : IRoleRepository
    {
        private IRoleStore _roleStore;

        public RoleRepository(IRoleStore roleStore)
        {
            _roleStore = roleStore;
        }

        public async Task<Role> QueryById(Guid id)
        {
            return await _roleStore.QueryById(id);
        }

        public async Task<QueryResult<Role>> QueryByName(string name, int page, int pageSize)
        {
            return await _roleStore.QueryByName(name,page,pageSize);
        }

        public async Task<QueryResult<Role>> QueryByUser(string userKey, int page, int pageSize)
        {
            return await _roleStore.QueryByUser(userKey, page, pageSize);
        }
    }
}
