using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Security.WhitelistPolicy
{
    /// <summary>
    /// 系统操作仓储接口
    /// </summary>
    public interface ISystemOperationRepository
    {
        /// <summary>
        /// 根据Id查询系统操作
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
        Task<SystemOperation> QueryById(Guid id);
        /// <summary>
        /// 根据名称查询系统操作
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns></returns>
        Task<SystemOperation> QueryByName(string name);
        /// <summary>
        /// 根据名称和状态查询系统操作
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="status">状态</param>
        /// <returns></returns>
        Task<SystemOperation> QueryByName(string name,int status);
        /// <summary>
        /// 分页查询系统操作
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<QueryResult<SystemOperation>> QueryByPage(int page, int pageSize);
        /// <summary>
        /// 根据名称分页查询系统操作
        /// </summary>
        /// <param name="name"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<QueryResult<SystemOperation>> QueryByPage(string name, int page, int pageSize);

    }
}
