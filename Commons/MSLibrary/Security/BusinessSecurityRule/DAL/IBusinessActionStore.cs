using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Security.BusinessSecurityRule.DAL
{
    /// <summary>
    /// 业务动作数据操作
    /// </summary>
    public interface IBusinessActionStore
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        Task Add(BusinessAction action);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        Task Update(BusinessAction action);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="actionId"></param>
        /// <returns></returns>
        Task Delete(Guid actionId);
        /// <summary>
        /// 根据Id查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BusinessAction> QueryById(Guid id);
        /// <summary>
        /// 根据名称查询
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<BusinessAction> QueryByName(string name);
        /// <summary>
        /// 根据组Id查询所有关联的业务动作
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        Task QueryByGroup(Guid groupId, Func<BusinessAction, Task> callback);
        /// <summary>
        /// 根据组Id分页查询关联的业务动作
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<QueryResult<BusinessAction>> QueryByGroup(Guid groupId, int page, int pageSize);
        /// <summary>
        /// 根据名称匹配分页查询业务动作
        /// </summary>
        /// <param name="name"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<QueryResult<BusinessAction>> QueryByName(string name, int page, int pageSize);

        /// <summary>
        /// 新增业务动作与业务动作组的关联关系
        /// </summary>
        /// <param name="actionId"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        Task AddGroupRelation(Guid actionId,Guid groupId);
        /// <summary>
        /// 删除业务动作与业务动作组的关联关系
        /// </summary>
        /// <param name="actionId"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        Task DeleteGroupRelation(Guid actionId,Guid groupId);
        /// <summary>
        /// 分页查询没有关联指定组的业务动作
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<QueryResult<BusinessAction>> QueryByNullRelationGroup(Guid groupId, int page, int pageSize);
    }
}
