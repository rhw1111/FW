using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MSLibrary.Cache;
using MSLibrary.DI;

namespace MSLibrary.Context
{
    /// <summary>
    /// Http声明生成器仓储帮助类
    /// 提供缓存服务
    /// 简化需要缓存服务的调用方使用
    /// </summary>
    [Injection(InterfaceType = typeof(HttpClaimGeneratorRepositoryHelper), Scope = InjectionScope.Singleton)]
    public class HttpClaimGeneratorRepositoryHelper
    {
        private IHttpClaimGeneratorRepository _httpClaimGeneratorRepository;


        private static int _cacheSize = 2000;

        /// <summary>
        /// 缓存数量
        /// 默认2000
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
                _generatorsByName.Length = value;
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
                _generatorsByName.Clear();
            }
        }

        private static HashLinkedCache<string, CacheTimeContainer<HttpClaimGenerator>> _generatorsByName = new HashLinkedCache<string, CacheTimeContainer<HttpClaimGenerator>>() { Length = CacheSize };


        public HttpClaimGeneratorRepositoryHelper(IHttpClaimGeneratorRepository httpClaimGeneratorRepository)
        {
            _httpClaimGeneratorRepository = httpClaimGeneratorRepository;
        }
        public async Task<HttpClaimGenerator> QueryByName(string name)
        {
            CacheTimeContainer<HttpClaimGenerator> generatorItem = _generatorsByName.GetValue(name);
            if (generatorItem == null || generatorItem.Expire())
            {
               
                var generator = await _httpClaimGeneratorRepository.QueryByName(name);
                generatorItem = new CacheTimeContainer<HttpClaimGenerator>(generator, CacheTimeout);
                _generatorsByName.SetValue(name, generatorItem);
            }

            return generatorItem.Value;
        }
    }



}
