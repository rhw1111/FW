using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.Cache;


namespace MSLibrary.Context
{
    [Injection(InterfaceType = typeof(IEnvironmentClaimGeneratorRepositoryCacheProxy), Scope = InjectionScope.Singleton)]
    public class EnvironmentClaimGeneratorRepositoryCacheProxy : IEnvironmentClaimGeneratorRepositoryCacheProxy
    {
        public static KVCacheVisitorSetting KVCacheVisitorSetting = new KVCacheVisitorSetting()
        {
            Name = "_EnvironmentClaimGeneratorRepository",
            ExpireSeconds = -1,
            MaxLength = 500
        };

        private IEnvironmentClaimGeneratorRepository _environmentClaimGeneratorRepository;

        private KVCacheVisitor _kvcacheVisitor;


        public EnvironmentClaimGeneratorRepositoryCacheProxy(IEnvironmentClaimGeneratorRepository environmentClaimGeneratorRepository)
        {
            _environmentClaimGeneratorRepository = environmentClaimGeneratorRepository;
            _kvcacheVisitor = CacheInnerHelper.CreateKVCacheVisitor(KVCacheVisitorSetting);
        }


        public async Task<EnvironmentClaimGenerator> QueryByName(string name)
        {
            return await _kvcacheVisitor.Get(
                async(k)=>
                {
                    return await _environmentClaimGeneratorRepository.QueryByName(name);
                },
                name
                );

        }
    }
}
