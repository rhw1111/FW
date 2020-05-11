using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.SystemToken
{
    /// <summary>
    /// 验证终结点仓储
    /// </summary>
    public interface IAuthorizationEndpointRepository
    {
        /// <summary>
        /// 根据id查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<AuthorizationEndpoint> QueryById(Guid id);
        /// <summary>
        /// 根据名称查询验证终结点
        /// </summary>
        /// <param name="name">验证终结点名称</param>
        /// <returns></returns>
        Task<AuthorizationEndpoint> QueryByName(string name);

        /// <summary>
        /// 分页查询匹配名称的验证终结点
        /// </summary>
        /// <param name="authorizationName"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<QueryResult<AuthorizationEndpoint>> QueryByPage(string authorizationName, int page, int pageSize);
    }
}
