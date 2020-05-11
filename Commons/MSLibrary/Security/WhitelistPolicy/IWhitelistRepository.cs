using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Security.WhitelistPolicy
{
    /// <summary>
    /// 白名单仓储
    /// </summary>
    public interface IWhitelistRepository
    {
        /// <summary>
        /// 分页查询白名单
        /// </summary>
        /// <param name="page">查询页数</param>
        /// <param name="pageSize">每页数量</param>
        /// <returns></returns>
        Task<QueryResult<Whitelist>> QueryByPage(int page, int pageSize);
        /// <summary>
        /// 根据Id查询白名单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Whitelist> QueryById(Guid id);

        /// <summary>
        /// 根据系统操作的id和白名单的id查询
        /// </summary>
        /// <param name="systemOperationId"></param>
        /// <param name="whitelistId"></param>
        /// <returns></returns>
        Task<Whitelist> QueryBySystemOperationRelation(Guid systemOperationId, Guid whitelistId);

        /// <summary>
        /// 根据系统操作的id和白名单的系统名称查询
        /// </summary>
        /// <param name="systemOperationId"></param>
        /// <param name="systemName"></param>
        /// <returns></returns>
        Task<Whitelist> QueryBySystemOperationRelation(Guid systemOperationId, string systemName);

    }
}
