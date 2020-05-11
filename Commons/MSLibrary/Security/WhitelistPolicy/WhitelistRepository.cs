using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.Security.WhitelistPolicy.DAL;

namespace MSLibrary.Security.WhitelistPolicy
{
    [Injection(InterfaceType = typeof(IWhitelistRepository), Scope = InjectionScope.Singleton)]
    public class WhitelistRepository : IWhitelistRepository
    {
        private IWhitelistStore _whitelistStore;

        public WhitelistRepository(IWhitelistStore whitelistStore)
        {
            _whitelistStore = whitelistStore;
        }
        public async Task<Whitelist> QueryById(Guid id)
        {
            return await _whitelistStore.QueryById(id);
        }

        public async Task<QueryResult<Whitelist>> QueryByPage(int page, int pageSize)
        {
            return await _whitelistStore.QueryByPage(page, pageSize);
        }

        public async Task<Whitelist> QueryBySystemOperationRelation(Guid systemOperationId, Guid whitelistId)
        {
            return await _whitelistStore.QueryBySystemOperationRelation(systemOperationId, whitelistId);
        }

        public async Task<Whitelist> QueryBySystemOperationRelation(Guid systemOperationId, string systemName)
        {
            return await _whitelistStore.QueryBySystemOperationRelation(systemOperationId, systemName);
        }
    }
}
