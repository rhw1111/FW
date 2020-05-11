using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.MessageQueue
{
    /// <summary>
    /// 队列执行组仓储
    /// </summary>
    public interface ISQueueProcessGroupRepository
    {
        /// <summary>
        /// 根据名称查询队列执行组
        /// </summary>
        /// <param name="name">执行组名称</param>
        /// <returns></returns>
        Task<SQueueProcessGroup> QueryByName(string name);
        /// <summary>
        /// 根据名称分页查询队列执行组
        /// </summary>
        /// <param name="name"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<QueryResult<SQueueProcessGroup>> QueryByName(string name, int page, int pageSize);
    }
}
