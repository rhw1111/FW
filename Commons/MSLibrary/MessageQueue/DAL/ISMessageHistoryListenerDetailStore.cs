using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.MessageQueue.DAL
{
    public interface ISMessageHistoryListenerDetailStore
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="historyId"></param>
        /// <param name="detail"></param>
        /// <returns></returns>
        Task Add(SMessageHistoryListenerDetail detail);
        /// <summary>
        /// 根据监听名称查询
        /// </summary>
        /// <param name="historyId"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<SMessageHistoryListenerDetail> QueryByName(Guid historyId,string name);
    }
}
