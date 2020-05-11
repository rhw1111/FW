using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Security.PrivilegeManagement
{
    /// <summary>
    /// 角色仓储
    /// </summary>
    public interface IRoleRepository
    {
        /// <summary>
        /// 根据Id查询
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
        Task<QueryResult<Role>> QueryByName(string name, int page, int pageSize);
        /// <summary>
        /// 根据用户关键字分页查询
        /// </summary>
        /// <param name="userKey"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<QueryResult<Role>> QueryByUser(string userKey, int page, int pageSize);
    }
}
