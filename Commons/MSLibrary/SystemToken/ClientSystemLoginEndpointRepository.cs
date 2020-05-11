using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.SystemToken.DAL;

namespace MSLibrary.SystemToken
{
    public class ClientSystemLoginEndpointRepository : IClientSystemLoginEndpointRepository
    {
        private IClientSystemLoginEndpointStore _clientSystemLoginEndpointStore;

        public ClientSystemLoginEndpointRepository(IClientSystemLoginEndpointStore clientSystemLoginEndpointStore)
        {
            _clientSystemLoginEndpointStore = clientSystemLoginEndpointStore;
        }

        public async Task<ClientSystemLoginEndpoint> QueryById(Guid id)
        {
            return await _clientSystemLoginEndpointStore.QueryById(id);
        }

        public async Task<ClientSystemLoginEndpoint> QueryByName(string name)
        {
            return await _clientSystemLoginEndpointStore.QueryByName(name);
        }

        public async Task<QueryResult<ClientSystemLoginEndpoint>> QueryByPage(string name, int page, int pageSize)
        {
            return await _clientSystemLoginEndpointStore.QueryByPage(name,page,pageSize);
        }
    }
}
