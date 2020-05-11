using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.MessageQueue
{
    public interface IClientSMessageTypeListenerEndpointRepository
    {
        Task<ClientSMessageTypeListenerEndpoint> QueryById(Guid id);
        Task<ClientSMessageTypeListenerEndpoint> QueryByName(string name);
        Task<QueryResult<ClientSMessageTypeListenerEndpoint>> QueryByPage(string name, int page, int pageSize);
    }
}
