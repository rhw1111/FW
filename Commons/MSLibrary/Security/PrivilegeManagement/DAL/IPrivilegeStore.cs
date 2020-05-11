using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Security.PrivilegeManagement.DAL
{
    /// <summary>
    /// 权限数据操作
    /// </summary>
    public interface IPrivilegeStore
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="privilege"></param>
        /// <returns></returns>
        Task Add(Privilege privilege);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="privilege"></param>
        /// <returns></returns>
        Task Update(Privilege privilege);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task Delete(Guid id);
        /// <summary>
        /// 根据Id查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Privilege> QueryById(Guid id);

        /// <summary>
        /// 根据查询条件分页查询
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<QueryResult<Privilege>> QueryByCriteria(PrivilegeQueryCriteria criteria,int page,int pageSize);
        /// <summary>
        /// 根据角色分页查询所有关联的权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<QueryResult<Privilege>> QueryByRole(Guid roleId,int page,int pageSize);
        /// <summary>
        /// 根据角色获取所有关联的角色
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        Task QueryByRole(Guid roleId,Func<Privilege,Task> callback);
        /// <summary>
        /// 根据用户关键字和权限名称查询
        /// </summary>
        /// <param name="userKey"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<Privilege> QueryByNameAndUser(string userKey,Guid systemId,string name);

        /// <summary>
        /// 新增角色权限关联关系
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="privilegeId"></param>
        /// <returns></returns>
        Task AddPrivilegeRelation(Guid roleId, Guid privilegeId);
        /// <summary>
        /// 删除角色权限关联关系
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="privilegeId"></param>
        /// <returns></returns>
        Task DeletePrivilegeRelation(Guid roleId, Guid privilegeId);


    }
}
