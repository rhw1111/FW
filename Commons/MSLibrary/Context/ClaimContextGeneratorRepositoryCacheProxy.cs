using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.Cache;

namespace MSLibrary.Context
{
    [Injection(InterfaceType = typeof(IClaimContextGeneratorRepositoryCacheProxy), Scope = InjectionScope.Singleton)]
    public class ClaimContextGeneratorRepositoryCacheProxy : IClaimContextGeneratorRepositoryCacheProxy
    {
        public static KVCacheVisitorSetting KVCacheVisitorSetting = new KVCacheVisitorSetting()
        {
            Name = "_ClaimContextGeneratorRepository",
            ExpireSeconds = -1,
            MaxLength = 500
        };

        private IClaimContextGeneratorRepository _claimContextGeneratorRepository;

        public ClaimContextGeneratorRepositoryCacheProxy(IClaimContextGeneratorRepository claimContextGeneratorRepository)
        {
            _claimContextGeneratorRepository = claimContextGeneratorRepository;
            _kvcacheVisitor = CacheInnerHelper.CreateKVCacheVisitor(KVCacheVisitorSetting);
        }

        private KVCacheVisitor _kvcacheVisitor;

        public async Task<ClaimContextGenerator> QueryByName(string name)
        {
            return (await _kvcacheVisitor.Get(
                async(k)=>
                {
                    var obj= await _claimContextGeneratorRepository.QueryByName(name);
                    if (obj == null)
                    {
                        return (obj, false);
                    }
                    else
                    {
                        return (obj, true);
                    }
                },
                name
                )).Item1;
        }
    }
}
