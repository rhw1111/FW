using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.Security.WhitelistPolicy.DAL;
using MSLibrary.DI;

namespace MSLibrary.Security.WhitelistPolicy
{
    [Injection(InterfaceType = typeof(IClientWhitelistRepository), Scope = InjectionScope.Singleton)]
    public class ClientWhitelistRepository : IClientWhitelistRepository
    {
        private IClientWhitelistStore _clientWhitelistStore;

        public ClientWhitelistRepository(IClientWhitelistStore clientWhitelistStore)
        {
            _clientWhitelistStore = clientWhitelistStore;
        }
        public async Task<ClientWhitelist> QueryById(Guid id)
        {
            return await _clientWhitelistStore.QueryById(id);
        }

        public async Task<QueryResult<ClientWhitelist>> QueryByPage(int page, int pageSize)
        {
            return await _clientWhitelistStore.QueryByPage(page, pageSize);
        }

        public async Task<QueryResult<ClientWhitelist>> QueryByPage(int page, int pageSize, string systemName)
        {
            return await _clientWhitelistStore.QueryByPage(page, pageSize, systemName);
        }

        public async Task<ClientWhitelist> QueryBySystemName(string systemName)
        {
            return await _clientWhitelistStore.QueryBySystemName(systemName);
        }
    }
}
