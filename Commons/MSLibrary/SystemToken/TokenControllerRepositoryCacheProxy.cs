using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.Cache;

namespace MSLibrary.SystemToken
{
    [Injection(InterfaceType = typeof(ITokenControllerRepositoryCacheProxy), Scope = InjectionScope.Singleton)]
    public class TokenControllerRepositoryCacheProxy : ITokenControllerRepositoryCacheProxy
    {
        public static KVCacheVisitorSetting KVCacheVisitorSetting = new KVCacheVisitorSetting()
        {
            Name = "_TokenControllerRepository",
            ExpireSeconds = -1,
            MaxLength = 200
        };

        private ITokenControllerRepository _tokenControllerRepository;

        public TokenControllerRepositoryCacheProxy(ITokenControllerRepository tokenControllerRepository)
        {
            _tokenControllerRepository = tokenControllerRepository;
            _kvcacheVisitor = CacheInnerHelper.CreateKVCacheVisitor(KVCacheVisitorSetting);
        }

        private KVCacheVisitor _kvcacheVisitor;


        public async Task<TokenController> QueryByName(string name)
        {
            return (await _kvcacheVisitor.Get(
                async (k) =>
                {
                    var obj= await _tokenControllerRepository.QueryByName(name);
                    if (obj == null)
                    {
                        return (obj, false);
                    }
                    else
                    {
                        return (obj, true);
                    }
                }, name
                )).Item1;
        }
    }
}
