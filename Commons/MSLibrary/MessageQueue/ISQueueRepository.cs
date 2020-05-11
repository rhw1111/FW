using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.MessageQueue
{
    /// <summary>
    /// 消息队列仓储
    /// </summary>
    public interface ISQueueRepository
    {
        /// <summary>
        /// 根据分组名称分页查询
        /// </summary>
        /// <param name="groupName">分组名称</param>
        /// <param name="isDead">是否死队列</param>
        /// <param name="page">页数</param>
        /// <param name="pageSize">每页数量</param>
        /// <returns></returns>
        Task<QueryResult<SQueue>> QueryByGroup(string groupName,bool isDead, int page, int pageSize);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="page">页数</param>
        /// <param name="pageSize">每页数量</param>
        /// <returns></returns>
        Task<QueryResult<SQueue>> Query(int page, int pageSize);

        /// <summary>
        /// 根据散列值查询
        /// </summary>
        /// <param name="groupName">分组名称</param>
        /// <param name="isDead">是否死队列</param>
        /// <param name="code">散列值</param>
        /// <returns></returns>
        Task<SQueue> QueryByCode(string groupName, bool isDead,int code);
        /// <summary>
        /// 根据队列执行组名称获取关联的所有队列
        /// </summary>
        /// <param name="processGroupName">队列执行组名称</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        Task QueryByProcessGroup(string processGroupName,Func<SQueue,Task> callback);
        /// <summary>
        /// 分页查询所有没有关联队列执行组的队列
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<QueryResult<SQueue>> QueryByNullProcessGroup(int page, int pageSize);

        Task<SQueue> QueryById(Guid id);

    }
}
