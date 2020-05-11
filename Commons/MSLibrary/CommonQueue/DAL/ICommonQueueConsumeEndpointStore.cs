using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.CommonQueue.DAL
{
    /// <summary>
    /// 通用队列终结点数据操作
    /// </summary>
    public interface ICommonQueueConsumeEndpointStore
    {
        Task Add(CommonQueueConsumeEndpoint endpoint);
        Task Update(CommonQueueConsumeEndpoint endpoint);
        Task Delete(Guid id);
        Task<CommonQueueConsumeEndpoint> QueryByID(Guid id);
        Task<QueryResult<CommonQueueConsumeEndpoint>> QueryByPage(string name, int page, int pageSize);
    }
}
