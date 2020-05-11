using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Security.WhitelistPolicy.DAL
{
    /// <summary>
    /// 系统操作的数据操作
    /// </summary>
    public interface ISystemOperationStore
    {
        Task Add(SystemOperation operation);
        Task Update(SystemOperation operation);
        Task Delete(Guid id);

        Task<SystemOperation> QueryById(Guid id);
        Task<SystemOperation> QueryByName(string name);
        Task<SystemOperation> QueryByName(string name,int status);
        Task<QueryResult<SystemOperation>> QueryByPage(int page, int pageSize);
        Task<QueryResult<SystemOperation>> QueryByPage(string name, int page, int pageSize);

    }
}
