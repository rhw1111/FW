using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.SocketManagement.DAL;
using MSLibrary.DI;

namespace MSLibrary.SocketManagement
{
    [Injection(InterfaceType = typeof(ITcpListenerRepository), Scope = InjectionScope.Singleton)]
    public class TcpListenerRepository : ITcpListenerRepository
    {
        private ITcpListenerStore _tcpListenerStore;

        public TcpListenerRepository(ITcpListenerStore tcpListenerStore)
        {
            _tcpListenerStore = tcpListenerStore;
        }
        public async Task<TcpListener> QueryById(Guid id)
        {
            return await _tcpListenerStore.QueryById(id);
        }

        public async Task<TcpListener> QueryByName(string name)
        {
            return await _tcpListenerStore.QueryByName(name);
        }

        public async Task<QueryResult<TcpListener>> QueryByName(string name, int page, int pageSize)
        {
            return await _tcpListenerStore.QueryByName(name,page,pageSize);
        }
    }
}
