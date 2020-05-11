using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.CommonQueue.DAL
{
    public interface ICommonQueueProductEndpointStore
    {
        Task Add(CommonQueueProductEndpoint endpoint);
        Task Update(CommonQueueProductEndpoint endpoint);
        Task Delete(Guid id);
        Task<CommonQueueProductEndpoint> QueryByID(Guid id);
        Task<QueryResult<CommonQueueProductEndpoint>> QueryByPage(string name, int page, int pageSize);
    }
}
