using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Security.WhitelistPolicy
{
    /// <summary>
    /// 客户端白名单仓储
    /// </summary>
    public interface IClientWhitelistRepository
    {
        /// <summary>
        /// 根据id查询客户端白名单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ClientWhitelist> QueryById(Guid id);

        /// <summary>
        /// 根据系统名称查询客户端白名单
        /// </summary>
        /// <param name="systemName"></param>
        /// <returns></returns>
        Task<ClientWhitelist> QueryBySystemName(string systemName);

        /// <summary>
        /// 分页查询客户端白名单
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<QueryResult<ClientWhitelist>> QueryByPage(int page, int pageSize);

        /// <summary>
        /// 分页查询匹配系统名称客户端白名单
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<QueryResult<ClientWhitelist>> QueryByPage(int page, int pageSize, string systemName);
    }
}
