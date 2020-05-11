using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.SocketManagement.DAL;
using MSLibrary.DI;

namespace MSLibrary.SocketManagement
{
    [Injection(InterfaceType = typeof(ITcpListenerLogRepository), Scope = InjectionScope.Singleton)]
    public class TcpListenerLogRepository : ITcpListenerLogRepository
    {
        private ITcpListenerLogStore _tcpListenerLogStore;

        public TcpListenerLogRepository(ITcpListenerLogStore tcpListenerLogStore)
        {
            _tcpListenerLogStore = tcpListenerLogStore;
        }
        public async Task<TcpListenerLog> QueryById(string listenerName,Guid id)
        {
            return await _tcpListenerLogStore.QueryById(listenerName, id);
        }

        public async Task<QueryResult<TcpListenerLog>> QueryByListener(string listenerName, int page, int pageSize)
        {
            return await _tcpListenerLogStore.QueryByListener(listenerName, page, pageSize);
        }

        public async Task<List<TcpListenerLog>> QueryLatestByListener(string listenerName, DateTime requestTime, Guid? latestId, int size)
        {
            return await _tcpListenerLogStore.QueryLatestByListener(listenerName, requestTime, latestId, size);
        }
    }
}
