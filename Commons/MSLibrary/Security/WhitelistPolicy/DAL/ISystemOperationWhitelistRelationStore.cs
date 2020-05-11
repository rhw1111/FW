using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Security.WhitelistPolicy.DAL
{
    /// <summary>
    /// 系统操作和白名单关联关系的数据操作
    /// </summary>
    public interface ISystemOperationWhitelistRelationStore
    {
        /// <summary>
        /// 新增关联关系
        /// </summary>
        /// <param name="systemOperationId">系统操作的Id</param>
        /// <param name="whitelistId">白名单的Id</param>
        /// <returns></returns>
        Task Add(Guid systemOperationId,Guid whitelistId);
        /// <summary>
        /// 删除关联关系
        /// </summary>
        /// <param name="systemOperationId"></param>
        /// <param name="whitelistId"></param>
        /// <returns></returns>
        Task Delete(Guid systemOperationId, Guid whitelistId);
    }
}
