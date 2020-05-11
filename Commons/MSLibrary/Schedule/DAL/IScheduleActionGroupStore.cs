using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Schedule.DAL
{
    /// <summary>
    /// 调度动作组数据操作
    /// </summary>
    public interface IScheduleActionGroupStore
    {
        /// <summary>
        /// 根据Id查询调度动作组
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ScheduleActionGroup> QueryByID(Guid id);
        /// <summary>
        /// 根据名称查询调度动作组
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<ScheduleActionGroup> QueryByName(string name);
        /// <summary>
        /// 根据名称匹配分页查询调度动作组
        /// </summary>
        /// <param name="name"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<QueryResult<ScheduleActionGroup>> QueryByPage(string name, int page, int pageSize);



        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        Task Add(ScheduleActionGroup group);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        Task Update(ScheduleActionGroup group);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task Delete(Guid id);
    }
}
