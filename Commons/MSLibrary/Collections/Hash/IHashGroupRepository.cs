using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Collections.Hash
{
    /// <summary>
    /// 哈希组仓储
    /// </summary>
    public interface IHashGroupRepository
    {
        /// <summary>
        /// 根据id查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<HashGroup> QueryById(Guid id);
        /// <summary>
        /// 根据id查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<HashGroup> QueryByName(string name);
        HashGroup QueryByNameSync(string name);

        /// <summary>
        /// 根据名称匹配分页查询
        /// </summary>
        /// <param name="name"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<QueryResult<HashGroup>> QueryByName(string name, int page, int pageSize);
        /// <summary>
        /// 根据类型查询该类型下所有的组
        /// </summary>
        /// <param name="type"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        Task QueryByType(string type, Func<HashGroup, Task> action);

    }
}
