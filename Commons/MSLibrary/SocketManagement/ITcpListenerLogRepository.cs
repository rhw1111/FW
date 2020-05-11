using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.SocketManagement
{
    /// <summary>
    /// Tcp监听日志仓储接口
    /// </summary>
    public interface ITcpListenerLogRepository
    {
        /// <summary>
        /// 根据Id查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TcpListenerLog> QueryById(string listenerName,Guid id);
        /// <summary>
        /// 根据监听名称分页查询
        /// </summary>
        /// <param name="listenerName"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<QueryResult<TcpListenerLog>> QueryByListener(string listenerName, int page, int pageSize);
        /// <summary>
        /// 根据监听名称和请求时间获取晚于请求时间的指定数量的日志
        /// </summary>
        /// <param name="listenerName"></param>
        /// <param name="requestTime"></param>
        /// <param name="latestId"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        Task<List<TcpListenerLog>> QueryLatestByListener(string listenerName, DateTime requestTime, Guid? latestId, int size);
    }
}
