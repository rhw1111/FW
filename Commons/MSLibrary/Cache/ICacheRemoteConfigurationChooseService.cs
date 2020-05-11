using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Cache
{
    /// <summary>
    /// 缓存远程配置选择服务
    /// </summary>
    public interface ICacheRemoteConfigurationChooseService
    {
        /// <summary>
        /// 根据缓存关键字选择对应的远程配置
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        Task<CacheRemoteConfiguration> Choose(string cacheKey);
    }
}
