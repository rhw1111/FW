using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Collections.Hash
{
    /// <summary>
    /// 哈希组策略仓储
    /// </summary>
    public interface IHashGroupStrategyRepository
    {
        /// <summary>
        /// 根据id查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<HashGroupStrategy> QueryById(Guid id);
        /// <summary>
        /// 根据名称匹配分页查询
        /// </summary>
        /// <param name="name"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<QueryResult<HashGroupStrategy>> QueryByName(string name, int page, int pageSize);
    }
}
