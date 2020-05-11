using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Logger
{
    /// <summary>
    /// 通用日志仓储
    /// </summary>
    public interface ICommonLogRepository
    {
        /// <summary>
        /// 根据ID、ParentID、ParentActionName查询
        /// </summary>
        /// <param name="id"></param>
        /// <param name="parentID"></param>
        /// <param name="parentAction"></param>
        /// <returns></returns>
        Task<CommonLog> QueryByID(Guid id, Guid parentID, string parentAction);
        /// <summary>
        /// 根据Id查询本地日志
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<CommonLog> QueryLocalByID(Guid id);
        /// <summary>
        /// 分页查询前匹配Message的本地日志
        /// </summary>
        /// <param name="message"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<QueryResult<CommonLog>> QueryLocal(string message, int page, int pageSize);
        /// <summary>
        /// 根据ParentID、ParentActionName分页查询
        /// </summary>
        /// <param name="parentID"></param>
        /// <param name="parentAction"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<QueryResult<CommonLog>> QueryByParentId(Guid parentID, string parentAction, int page, int pageSize);
        /// <summary>
        /// 查询符合条件的指定数量的日志
        /// </summary>
        /// <param name="parentAction"></param>
        /// <param name="level"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        Task<List<CommonLog>> QueryRootByConditionTop(string parentAction, int? level, int top);
    }
}
