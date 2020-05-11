using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Collections.Hash.DAL
{
    /// <summary>
    /// 哈希组数据操作
    /// </summary>
    public interface IHashGroupStore
    {
        Task Add(HashGroup group);
        Task Update(HashGroup group);
        Task Delete(Guid id);

        Task<HashGroup> QueryById(Guid id);

        Task<QueryResult<HashGroup>> QueryByName(string name, int page, int pageSize);
        Task<HashGroup> QueryByName(string name);
        HashGroup QueryByNameSync(string name);

        Task QueryByType(string type, Func<HashGroup,Task> action);

    }
}
