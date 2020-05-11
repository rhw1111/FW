using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.MessageQueue.DAL;
using MSLibrary.DI;

namespace MSLibrary.MessageQueue
{
    [Injection(InterfaceType = typeof(IClientSMessageTypeListenerEndpointRepository), Scope = InjectionScope.Singleton)]
    public class ClientSMessageTypeListenerEndpointRepository : IClientSMessageTypeListenerEndpointRepository
    {
        private IClientSMessageTypeListenerEndpointStore _clientSMessageTypeListenerEndpointStore;

        public ClientSMessageTypeListenerEndpointRepository(IClientSMessageTypeListenerEndpointStore clientSMessageTypeListenerEndpointStore)
        {
            _clientSMessageTypeListenerEndpointStore = clientSMessageTypeListenerEndpointStore;
        }
        public async Task<ClientSMessageTypeListenerEndpoint> QueryById(Guid id)
        {
            return await _clientSMessageTypeListenerEndpointStore.QueryById(id);
        }

        public async Task<ClientSMessageTypeListenerEndpoint> QueryByName(string name)
        {
            return await _clientSMessageTypeListenerEndpointStore.QueryByName(name);
        }

        public async Task<QueryResult<ClientSMessageTypeListenerEndpoint>> QueryByPage(string name, int page, int pageSize)
        {
            return await _clientSMessageTypeListenerEndpointStore.QueryByPage(name,page,pageSize);
        }
    }
}
