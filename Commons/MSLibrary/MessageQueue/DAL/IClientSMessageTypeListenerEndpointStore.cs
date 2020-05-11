using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.MessageQueue.DAL
{
    /// <summary>
    /// 客户端消息类型监听终结点数据操作
    /// </summary>
    public interface IClientSMessageTypeListenerEndpointStore
    {
        Task Add(ClientSMessageTypeListenerEndpoint endpoint);
        Task Update(ClientSMessageTypeListenerEndpoint endpoint);
        Task Delete(Guid id);
        Task<ClientSMessageTypeListenerEndpoint> QueryById(Guid id);
        Task<ClientSMessageTypeListenerEndpoint> QueryByName(string name);
        Task<QueryResult<ClientSMessageTypeListenerEndpoint>> QueryByPage(string name,int page,int pageSize);
    }
}
