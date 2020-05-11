using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.SocketManagement.DAL
{
    /// <summary>
    /// Tcp客户端终结点数据操作
    /// </summary>
    public interface ITcpClientEndpointStore
    {
        Task Add(TcpClientEndpoint endpoint);
        Task Update(TcpClientEndpoint endpoint);
        Task Delete(Guid id);

        Task<TcpClientEndpoint> QueryById(Guid id);

        Task<TcpClientEndpoint> QueryByName(string name);
        Task<QueryResult<TcpClientEndpoint>> QueryByName(string name, int page, int pageSize);
    }
}
