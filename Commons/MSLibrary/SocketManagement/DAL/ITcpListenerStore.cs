using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.SocketManagement.DAL
{
    /// <summary>
    /// SocketListener数据操作接口
    /// </summary>
    public interface ITcpListenerStore
    {
        Task Add(TcpListener listener);
        Task Update(TcpListener listener);
        Task Delete(Guid id);

        Task<TcpListener> QueryById(Guid id);

        Task<TcpListener> QueryByName(string name);
        Task<QueryResult<TcpListener>> QueryByName(string name,int page,int pageSize);
    }
}
