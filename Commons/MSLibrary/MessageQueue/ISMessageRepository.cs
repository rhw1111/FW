using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.MessageQueue
{
    /// <summary>
    /// 消息仓储
    /// </summary>
    public interface ISMessageRepository
    {
        /// <summary>
        /// 获取指定队列的所有消息
        /// pageSize指定每次获取的数量
        /// 如果callback返回false表示不需要继续往下执行查询，直接退出查询
        /// </summary>
        /// <param name="queue">队列</param>
        /// <param name="pageSize">每次获取的数量</param>
        /// <param name="callBack"></param>
        /// <returns></returns>
        Task QueryAllByQueue(SQueue queue, int pageSize, Func<List<SMessage>, Task<bool>> callBack);
        /// <summary>
        /// 分页查询指定队列的消息
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<QueryResult<SMessage>> QueryByQueue(SQueue queue,int page,int pageSize);

    }
}
