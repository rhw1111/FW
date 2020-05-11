using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Security.RequestController
{
    /// <summary>
    /// 请求跟踪的仓储接口
    /// </summary>
    public interface IRequestTrackerRepository
    {
        /// <summary>
        /// 查询全局请求跟踪
        /// </summary>
        /// <returns></returns>
        Task<RequestTracker> QueryGlobal();
        /// <summary>
        /// 根据RequestKey查询请求跟踪
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<RequestTracker> QueryByRequestKey(string key);
    }
}
