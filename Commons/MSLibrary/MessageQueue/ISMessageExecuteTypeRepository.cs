using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.MessageQueue
{
    /// <summary>
    /// 消息执行类型仓储
    /// </summary>
    public interface ISMessageExecuteTypeRepository
    {
        /// <summary>
        /// 根据名称查询
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<SMessageExecuteType> QueryByName(string name);
        /// <summary>
        /// 根据名称分页查询
        /// </summary>
        /// <param name="name"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<QueryResult<SMessageExecuteType>> Query(string name, int page, int pageSize);
    }
}
