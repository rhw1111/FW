using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.Cache;

namespace MSLibrary.Context
{
    [Injection(InterfaceType = typeof(IHttpClaimGeneratorRepositoryCacheProxy), Scope = InjectionScope.Singleton)]
    public class HttpClaimGeneratorRepositoryCacheProxy : IHttpClaimGeneratorRepositoryCacheProxy
    {
        public static KVCacheVisitorSetting KVCacheVisitorSetting = new KVCacheVisitorSetting()
        {
            Name = "_HttpClaimGeneratorRepository",
            ExpireSeconds = -1,
            MaxLength = 500
        };

        private IHttpClaimGeneratorRepository _httpClaimGeneratorRepository;

        private KVCacheVisitor _kvcacheVisitor;


        public HttpClaimGeneratorRepositoryCacheProxy(IHttpClaimGeneratorRepository httpClaimGeneratorRepository)
        {
            _httpClaimGeneratorRepository = httpClaimGeneratorRepository;
            _kvcacheVisitor = CacheInnerHelper.CreateKVCacheVisitor(KVCacheVisitorSetting);
        }


        public async Task<HttpClaimGenerator> QueryByName(string name)
        {
            return await _kvcacheVisitor.Get(
                async (k) =>
                {
                    return await _httpClaimGeneratorRepository.QueryByName(name);
                },
                name
                );
        }
    }
}
