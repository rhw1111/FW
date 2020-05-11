using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.SocketManagement.DAL
{
    public interface ITcpListenerLogStore
    {
        Task Add(TcpListenerLog log);
        Task Delete(string listenerName,Guid id);
        Task<TcpListenerLog> QueryById(string listenerName,Guid id);
        Task<QueryResult<TcpListenerLog>> QueryByListener(string listenerName,int page,int pageSize);
        Task<List<TcpListenerLog>> QueryLatestByListener(string listenerName,DateTime requestTime,Guid? latestId,int size);
    }
}
