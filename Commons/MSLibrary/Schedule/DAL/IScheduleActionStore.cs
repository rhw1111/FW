using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Schedule.DAL
{
    /// <summary>
    /// 调度动作数据操作
    /// </summary>
    public interface IScheduleActionStore
    {
        /// <summary>
        /// 根据Id查询调度动作
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ScheduleAction> QueryByID(Guid id);
        /// <summary>
        /// 根据名称查询调度动作
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<ScheduleAction> QueryByName(string name);
        /// <summary>
        /// 根据名称匹配分页查询调度动作
        /// </summary>
        /// <param name="name"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<QueryResult<ScheduleAction>> QueryByPage(string name, int page, int pageSize);
        /// <summary>
        /// 根据调度动作组Id分页查询调度动作
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<QueryResult<ScheduleAction>> QueryByPageGroup(Guid groupId, int page, int pageSize);
        /// <summary>
        /// 根据调度动作组Id查询
        /// </summary>
        /// <param name="id"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        Task<ScheduleAction> QueryByGroup(Guid id, Guid groupId);

        /// <summary>
        /// 查询调度动作id下面的所有指定状态的调度动作
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="status"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        Task QueryAllAction(Guid groupId, int status, Func<ScheduleAction, Task> callback);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        Task Add(ScheduleAction action);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        Task Update(ScheduleAction action);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task Delete(Guid id);
        /// <summary>
        /// 增加动作与组的关联关系
        /// </summary>
        /// <param name="actionId"></param>
        /// <param name="queueId"></param>
        /// <returns></returns>
        Task AddActionGroupRelation(Guid actionId, Guid queueId);
        /// <summary>
        /// 删除动作与组的关联关系
        /// </summary>
        /// <param name="actionId"></param>
        /// <param name="queueId"></param>
        /// <returns></returns>
        Task DeleteActionGroupRelation(Guid actionId, Guid queueId);
        /// <summary>
        /// 分页查询尚未分配组且匹配动作名称的调度动作
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<QueryResult<ScheduleAction>> QueryByNullGroup(string name,int page, int pageSize);

    }
}
