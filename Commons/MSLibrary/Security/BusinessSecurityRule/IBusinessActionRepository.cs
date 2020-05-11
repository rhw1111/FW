using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Security.BusinessSecurityRule
{
    /// <summary>
    /// 业务动作仓储
    /// </summary>
    public interface IBusinessActionRepository
    {
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
        /// 根据名称匹配查询
        /// </summary>
        /// <param name="name"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<QueryResult<BusinessAction>> QueryByName(string name, int page, int pageSize);

    }
}
