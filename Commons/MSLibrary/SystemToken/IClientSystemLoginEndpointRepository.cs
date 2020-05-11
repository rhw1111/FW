using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.SystemToken
{
    public interface IClientSystemLoginEndpointRepository
    {
        Task<ClientSystemLoginEndpoint> QueryById(Guid id);
        Task<ClientSystemLoginEndpoint> QueryByName(string name);
        Task<QueryResult<ClientSystemLoginEndpoint>> QueryByPage(string name, int page, int pageSize);
    }
}
