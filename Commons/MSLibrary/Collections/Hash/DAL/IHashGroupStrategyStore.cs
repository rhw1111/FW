using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Collections.Hash.DAL
{
    /// <summary>
    /// 哈希组策略数据操作
    /// </summary>
    public interface IHashGroupStrategyStore
    {
        Task Add(HashGroupStrategy strategy);
        Task Update(HashGroupStrategy strategy);

        Task Delete(Guid id);

        Task<HashGroupStrategy> QueryById(Guid id);

        Task<QueryResult<HashGroupStrategy>> QueryByName(string name, int page, int pageSize);
    }
}
