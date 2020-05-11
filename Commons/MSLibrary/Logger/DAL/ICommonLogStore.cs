using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Logger.DAL
{
    /// <summary>
    /// 通用日志数据操作
    /// </summary>
    public interface ICommonLogStore
    {
        Task Add(CommonLog log);
        Task AddLocal(CommonLog log);
        Task<CommonLog> QueryByID(Guid id, Guid parentID, string parentAction);
        Task<CommonLog> QueryLocalByID(Guid id);

        Task<QueryResult<CommonLog>> QueryLocal(string message,int page, int pageSize);

        Task<QueryResult<CommonLog>> QueryByParentId(Guid parentID,string parentAction,int page,int pageSize);

        Task<List<CommonLog>> QueryRootByConditionTop(string parentAction,int? level,int top);

    }
}
