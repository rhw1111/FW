using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.Cache;
using MSLibrary.DI;

namespace MSLibrary.Security.Jwt
{
    /// <summary>
    /// Jwt终结点仓储帮助器
    /// 加入了缓存功能
    /// 简化需要缓存服务的服务工厂仓储的调用方使用
    /// 
    /// </summary>
    [Injection(InterfaceType = typeof(JwtEnpointRepositoryHelper), Scope = InjectionScope.Singleton)]
    public class JwtEnpointRepositoryHelper
    {
        private static HashLinkedCache<string, CacheTimeContainer<JwtEnpoint>> _nameEnpoints = new HashLinkedCache<string, CacheTimeContainer<JwtEnpoint>>() { Length = CacheSize };


        private IJwtEnpointRepository _jwtEnpointRepository;

        public JwtEnpointRepositoryHelper(IJwtEnpointRepository jwtEnpointRepository)
        {
            _jwtEnpointRepository = jwtEnpointRepository; ;
        }

        private static int _cacheSize = 200;

        /// <summary>
        /// 缓存数量
        /// 默认200
        /// </summary>
        public static int CacheSize
        {
            get
            {
                return _cacheSize;
            }
            set
            {
                _cacheSize = value;
                _nameEnpoints.Length = value;
            }
        }
        /// <summary>
        /// 缓存时间
        /// 默认600秒
        /// </summary>
        public static int CacheTimeout { get; set; } = 600;


        /// <summary>
        /// 清除缓存
        /// </summary>
        public static bool Refreash
        {
            set
            {
                _nameEnpoints.Clear();
            }
        }


        public async Task<JwtEnpoint> QueryByName(string name)
        {
            CacheTimeContainer<JwtEnpoint> endpointItem = _nameEnpoints.GetValue(name);
            if (endpointItem == null || endpointItem.Expire())
            {
                var endpoint = await _jwtEnpointRepository.QueryByName(name);
                endpointItem = new CacheTimeContainer<JwtEnpoint>(endpoint, CacheTimeout,0);
                _nameEnpoints.SetValue(name, endpointItem);
            }

            return endpointItem.Value;
        }

    }
}
