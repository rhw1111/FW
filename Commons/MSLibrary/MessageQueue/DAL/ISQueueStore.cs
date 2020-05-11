using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.MessageQueue.DAL
{
    /// <summary>
    /// 队列数据操作接口
    /// </summary>
    public interface ISQueueStore
    {
        Task Add(SQueue queue);

        Task Update(SQueue queue);

        Task Delete(Guid id);

        Task AddProcessGroupRelation(Guid processGroupId,Guid queueId);

        Task DeleteProcessGroupRelation(Guid processGroupId, Guid queueId);

        Task QueryByProceeGroup(Guid processGroupId,Func<SQueue,Task> callback);
        Task QueryByProceeGroup(string processGroupName, Func<SQueue, Task> callback);

        Task<SQueue> QueryById(Guid id);

        Task<SQueue> QueryByCode(string groupName, bool isDead, int code);
        Task<QueryResult<SQueue>> QueryByGroup(string groupName, bool isDead, int page, int pageSize);

        Task<QueryResult<SQueue>> Query(int page, int pageSize);

        Task<QueryResult<SQueue>> QueryByProceeGroup(Guid processGroupId,int page,int pageSize);

        Task<QueryResult<SQueue>> QueryByProceeGroup(string processGroupName, int page, int pageSize);

        Task<QueryResult<SQueue>> QueryByNullProcessGroup(int page, int pageSize);


    }
}
