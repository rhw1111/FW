using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.Security.WhitelistPolicy.DAL;
using MSLibrary.DI;

namespace MSLibrary.Security.WhitelistPolicy
{
    [Injection(InterfaceType = typeof(ISystemOperationRepository), Scope = InjectionScope.Singleton)]
    public class SystemOperationRepository : ISystemOperationRepository
    {
        private ISystemOperationStore _systemOperationStore;

        public SystemOperationRepository(ISystemOperationStore systemOperationStore)
        {
            _systemOperationStore = systemOperationStore;
        }
        public async Task<SystemOperation> QueryById(Guid id)
        {
            return await _systemOperationStore.QueryById(id);
        }

        public async Task<SystemOperation> QueryByName(string name)
        {
            return await _systemOperationStore.QueryByName(name);
        }

        public async Task<SystemOperation> QueryByName(string name, int status)
        {
            return await _systemOperationStore.QueryByName(name,status);
        }

        public async Task<QueryResult<SystemOperation>> QueryByPage(int page, int pageSize)
        {
            return await _systemOperationStore.QueryByPage(page,pageSize);
        }

        public async Task<QueryResult<SystemOperation>> QueryByPage(string name, int page, int pageSize)
        {
            return await _systemOperationStore.QueryByPage(name, page, pageSize);
        }
    }
}
