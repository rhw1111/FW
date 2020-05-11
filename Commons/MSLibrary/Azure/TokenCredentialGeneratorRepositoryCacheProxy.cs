using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.Cache;

namespace MSLibrary.Azure
{
    [Injection(InterfaceType = typeof(ITokenCredentialGeneratorRepositoryCacheProxy), Scope = InjectionScope.Singleton)]
    public class TokenCredentialGeneratorRepositoryCacheProxy : ITokenCredentialGeneratorRepositoryCacheProxy
    {
        public static KVCacheVisitorSetting KVCacheVisitorSetting = new KVCacheVisitorSetting()
        {
            Name = "_TokenCredentialGeneratorRepository",
             ExpireSeconds=-1,
             MaxLength=500
        };


        private ITokenCredentialGeneratorRepository _tokenCredentialGeneratorRepository;

        public TokenCredentialGeneratorRepositoryCacheProxy(ITokenCredentialGeneratorRepository tokenCredentialGeneratorRepository)
        {
            _tokenCredentialGeneratorRepository = tokenCredentialGeneratorRepository;
            _kvcacheVisitor = CacheInnerHelper.CreateKVCacheVisitor(KVCacheVisitorSetting);
        }

        private KVCacheVisitor _kvcacheVisitor;

        public async Task<TokenCredentialGenerator> QueryByName(string name)
        {
            return await _kvcacheVisitor.Get(
                async (k) =>
                {
                    return await _tokenCredentialGeneratorRepository.QueryByName(name);
                }, name
                );
        }
    }
}
