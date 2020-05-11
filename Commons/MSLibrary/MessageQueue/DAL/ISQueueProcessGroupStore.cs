using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.MessageQueue.DAL
{
    /// <summary>
    /// 队列执行组数据操作
    /// </summary>
    public interface ISQueueProcessGroupStore
    {
        Task Add(SQueueProcessGroup group);

        Task Update(SQueueProcessGroup group);

        Task Delete(Guid id);

        Task<SQueueProcessGroup> QueryByName(string name);

        Task<SQueueProcessGroup> QueryById(Guid id);

        Task<QueryResult<SQueueProcessGroup>> QueryByName(string name,int page,int pageSize);
    }
}
