using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Security.PrivilegeManagement
{
    /// <summary>
    /// 权限系统仓储
    /// </summary>
    public interface IPrivilegeSystemRepository
    {
        /// <summary>
        /// 根据id查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<PrivilegeSystem> QueryById(Guid id);
        /// <summary>
        /// 根据名称匹配查询
        /// </summary>
        /// <param name="name"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<QueryResult<PrivilegeSystem>> QueryByName(string name, int page, int pageSize);

    }
}
