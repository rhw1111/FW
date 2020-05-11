using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.MessageQueue.DAL
{
    /// <summary>
    /// 消息历史数据操作
    /// </summary>
    public interface ISMessageHistoryStore
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="history"></param>
        /// <returns></returns>
        Task Add(SMessageHistory history);
        /// <summary>
        /// 根据Id查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<SMessageHistory> QueryById(Guid id);


        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        Task UpdateStatus(Guid id, int status);
    }
}
