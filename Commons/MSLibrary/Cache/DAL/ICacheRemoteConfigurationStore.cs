using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Cache.DAL
{
    /// <summary>
    /// 缓存配置仓储
    /// </summary>
    public interface ICacheRemoteConfigurationStore
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        Task Add(CacheRemoteConfiguration configuration);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        Task Update(CacheRemoteConfiguration configuration);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task Delete(Guid id);
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
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<QueryResult<CacheRemoteConfiguration>> QueryByName(string name,int page,int pageSize);
    }
}
