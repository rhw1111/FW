using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Security.PrivilegeManagement.DAL
{
    /// <summary>
    /// 角色数据操作
    /// </summary>
    public interface IRoleStore
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        Task Add(Role role);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        Task Update(Role role);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task Delete(Guid id);
        /// <summary>
        /// 根据id查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Role> QueryById(Guid id);
        /// <summary>
        /// 根据名称匹配分页查询
        /// </summary>
        /// <param name="name"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<QueryResult<Role>> QueryByName(string name,int page,int pageSize);
        /// <summary>
        /// 根据用户关键字分页查询
        /// </summary>
        /// <param name="userKey"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<QueryResult<Role>> QueryByUser(string userKey,int page,int pageSize);
    }
}
