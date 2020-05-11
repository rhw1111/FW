using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Security.BusinessSecurityRule.DAL
{
    /// <summary>
    /// 业务动作组数据操作
    /// </summary>
    public interface IBusinessActionGroupStore
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        Task Add(BusinessActionGroup group);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        Task Update(BusinessActionGroup group);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        Task Delete(Guid groupId);
        /// <summary>
        /// 根据名称匹配分页查询
        /// </summary>
        /// <param name="name"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<QueryResult<BusinessActionGroup>> QueryByName(string name,int page,int pageSize);
        /// <summary>
        /// 根据Id查询
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<BusinessActionGroup> QueryById(Guid Id);
        /// <summary>
        /// 根据名称查询
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<BusinessActionGroup> QueryByName(string name);

        /// <summary>
        /// 根据业务动作分页查询关联的组
        /// </summary>
        /// <param name="actionId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<QueryResult<BusinessActionGroup>> QueryByAction(Guid actionId, int page, int pageSize);

    }
}
