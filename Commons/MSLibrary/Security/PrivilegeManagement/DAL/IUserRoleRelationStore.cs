using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Security.PrivilegeManagement.DAL
{
    /// <summary>
    /// 用户角色关联关系数据操作
    /// </summary>
    public interface IUserRoleRelationStore
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="relation"></param>
        /// <returns></returns>
        Task Add(UserRoleRelation relation);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="relation"></param>
        /// <returns></returns>
        Task Update(UserRoleRelation relation);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task Delete(Guid id);
        Task Delete(Guid roleId, Guid id);
        /// <summary>
        /// 根据id查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<UserRoleRelation> QueryById(Guid id);
        /// <summary>
        /// 根据角色和Id查询
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<UserRoleRelation> QueryByRole(Guid roleId,Guid id);
        /// <summary>
        /// 根据角色分页查询
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<QueryResult<UserRoleRelation>> QueryByRole(Guid roleId,int page,int pageSize);
        /// <summary>
        /// 根据角色和用户关键字匹配分页查询
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="userKey"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<QueryResult<UserRoleRelation>> QueryByRoleAndUserKey(Guid roleId,string userKey, int page, int pageSize);

        /// <summary>
        /// 根据角色查询所有关联关系
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name=""></param>
        /// <returns></returns>
        Task QueryByRole(Guid roleId,Func<UserRoleRelation,Task> callback);
    }
}
