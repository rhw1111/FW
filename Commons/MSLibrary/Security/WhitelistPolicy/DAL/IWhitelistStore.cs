using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Security.WhitelistPolicy.DAL
{
    public interface IWhitelistStore
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Task Add(Whitelist data);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Task Update(Whitelist data);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task Delete(Guid id);
        /// <summary>
        /// 根据id查询白名单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Whitelist> QueryById(Guid id);
        /// <summary>
        /// 分页查询白名单
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<QueryResult<Whitelist>> QueryByPage(int page, int pageSize);
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

        /// <summary>
        /// 根据系统操作的id、白名单的系统名称、状态查询
        /// </summary>
        /// <param name="systemOperationId"></param>
        /// <param name="systemName"></param>
        /// <param name="status">状态</param>
        /// <returns></returns>
        Task<Whitelist> QueryBySystemOperationRelation(Guid systemOperationId, string systemName,int status);

    }
}
