using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Cache
{
    /// <summary>
    /// 远程缓存配置仓储
    /// </summary>
    public interface ICacheRemoteConfigurationRepository
    {
        /// <summary>
        /// 根据Id查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<CacheRemoteConfiguration> QueryById(Guid id);
        /// <summary>
        /// 根据名称查询
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<CacheRemoteConfiguration> QueryByName(string name);
        /// <summary>
        /// 根据名称匹配分页查询
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pzge"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<QueryResult<CacheRemoteConfiguration>> QueryByName(string name,int page,int pageSize);
    }
}
