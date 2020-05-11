using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Security.PrivilegeManagement
{
    /// <summary>
    /// 权限仓储
    /// </summary>
    public interface IPrivilegeRepository
    {
        /// <summary>
        /// 根据Id查询权限
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
        Task<QueryResult<Privilege>> QueryByCriteria(PrivilegeQueryCriteria criteria, int page, int pageSize);
        /// <summary>
        /// 根据用户关键字和权限名称查询
        /// </summary>
        /// <param name="userKey"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<Privilege> QueryByNameAndUser(string userKey,Guid systemId, string name);
    }

    /// <summary>
    /// 权限查询的条件
    /// </summary>
    public class PrivilegeQueryCriteria
    {
        /// <summary>
        /// 是否使用权限系统作为查询条件
        /// </summary>
        public bool UseSystem { get; set; }
        /// <summary>
        /// 要查询的权限系统ID
        /// </summary>
        public Guid SystemId { get; set; }
        /// <summary>
        /// 是否使用权限名称匹配作为查询条件
        /// </summary>
        public bool UseName { get; set; }
        /// <summary>
        /// 要查询的权限名称匹配值
        /// </summary>
        public string Name { get; set; }
    }
}
