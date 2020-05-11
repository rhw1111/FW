using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.SocketManagement.DAL;
using MSLibrary.DI;

namespace MSLibrary.SocketManagement
{
    [Injection(InterfaceType = typeof(ITcpClientEndpointRepository), Scope = InjectionScope.Singleton)]
    public class TcpClientEndpointRepository : ITcpClientEndpointRepository
    {
        private ITcpClientEndpointStore _tcpClientEndpointStore;
        
        public TcpClientEndpointRepository(ITcpClientEndpointStore tcpClientEndpointStore)
        {
            _tcpClientEndpointStore = tcpClientEndpointStore;
        }
        public async Task<TcpClientEndpoint> QueryById(Guid id)
        {
            return await _tcpClientEndpointStore.QueryById(id);
        }

        public async Task<TcpClientEndpoint> QueryByName(string name)
        {
            return await _tcpClientEndpointStore.QueryByName(name);
        }

        public async Task<QueryResult<TcpClientEndpoint>> QueryByName(string name, int page, int pageSize)
        {
            return await _tcpClientEndpointStore.QueryByName(name,page,pageSize);
        }
    }
}
