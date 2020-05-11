using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.SystemToken
{
    /// <summary>
    /// 系统登陆终结点仓储
    /// </summary>
    public interface ISystemLoginEndpointRepository
    {
        /// <summary>
        /// 根据系统名称查询系统登陆终结点
        /// </summary>
        /// <param name="sysName"></param>
        /// <returns></returns>
        Task<SystemLoginEndpoint> QueryByName(string sysName);
        /// <summary>
        /// 根据Id查询
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<SystemLoginEndpoint> QueryById(Guid id);
        /// <summary>
        /// 根据名称匹配分页查询
        /// </summary>
        /// <param name="name"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<QueryResult<SystemLoginEndpoint>> QueryByPage(string name, int page, int pageSize);

    }
}
