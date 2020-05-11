using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.Security.PrivilegeManagement.DAL;

namespace MSLibrary.Security.PrivilegeManagement
{
    [Injection(InterfaceType = typeof(IPrivilegeSystemRepository), Scope = InjectionScope.Singleton)]
    public class PrivilegeSystemRepository : IPrivilegeSystemRepository
    {
        private IPrivilegeSystemStore _privilegeSystemStore;

        public PrivilegeSystemRepository(IPrivilegeSystemStore privilegeSystemStore)
        {
            _privilegeSystemStore = privilegeSystemStore;
        }
        public async Task<PrivilegeSystem> QueryById(Guid id)
        {
            return await _privilegeSystemStore.QueryById(id);
        }

        public async Task<QueryResult<PrivilegeSystem>> QueryByName(string name, int page, int pageSize)
        {
            return await _privilegeSystemStore.QueryByName(name,page,pageSize);
        }
    }
}
