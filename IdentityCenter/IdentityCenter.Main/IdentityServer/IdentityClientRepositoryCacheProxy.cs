using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.Cache;

namespace IdentityCenter.Main.IdentityServer
{
    [Injection(InterfaceType = typeof(IIdentityClientRepositoryCacheProxy), Scope = InjectionScope.Singleton)]
    public class IdentityClientRepositoryCacheProxy : IIdentityClientRepositoryCacheProxy
    {
        public static KVCacheVisitorSetting KVCacheVisitorSetting = new KVCacheVisitorSetting()
        {
            Name = "_IdentityClientRepository",
            ExpireSeconds = -1,
            MaxLength = 5000
        };

        private IIdentityClientRepository _identityClientRepository;

        public IdentityClientRepositoryCacheProxy(IIdentityClientRepository identityClientRepository)
        {
            _identityClientRepository = identityClientRepository;
            _kvcacheVisitor = CacheInnerHelper.CreateKVCacheVisitor(KVCacheVisitorSetting);
        }

        private KVCacheVisitor _kvcacheVisitor;

        public async Task<IdentityClient?> QueryByClientID(string clientID, CancellationToken cancellationToken = default)
        {
            return await _kvcacheVisitor.Get(
                async (k) =>
                {
                    return await _identityClientRepository.QueryByClientID(clientID, cancellationToken);
                },
                clientID
                );
        }
    }
}
