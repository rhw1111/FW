using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.MessageQueue.DAL
{
    /// <summary>
    /// 消息数据操作接口
    /// </summary>
    public interface ISMessageStore
    {
        Task Add(SQueue queue, SMessage message);
        Task Delete(SQueue queue,Guid id);
        Task AddRetry(SQueue queue,Guid id, string exceptionMessage);
        Task UpdateLastExecuteTime(SQueue queue, Guid id);
        Task AddToDead(SQueue queue,SMessage message);

        /// <summary>
        /// 查询指定Key并且期望执行时间早于指定时间的消息
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="key"></param>
        /// <param name="expectTime"></param>
        /// <returns></returns>
        Task<SMessage> QueryByKeyAndBeforeExpectTime(SQueue queue, string key, DateTime expectTime);

        Task QueryAllByQueue(SQueue queue, int pageSize, Func<List<SMessage>, Task<bool>> callBack);

        Task<QueryResult<SMessage>> QueryByQueue(SQueue queue, int page, int pageSize);

        Task<SMessage> QueryByOriginalID(SQueue queue,Guid originalMessageID,Guid listenerID);


        Task<SMessage> QueryByDelayID(SQueue queue, Guid delayMessageID);

    }
}
