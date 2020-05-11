using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.Cache;

namespace IdentityCenter.Main.IdentityServer
{

    [Injection(InterfaceType = typeof(IIdentityHostRepositoryCacheProxy), Scope = InjectionScope.Singleton)]
    public class IdentityHostRepositoryCacheProxy : IIdentityHostRepositoryCacheProxy
    {
        public static KVCacheVisitorSetting KVCacheVisitorSetting = new KVCacheVisitorSetting()
        {
            Name = "_IdentityHostRepository",
            ExpireSeconds = -1,
            MaxLength = 500
        };

        private IIdentityHostRepository _identityHostRepository;

        public IdentityHostRepositoryCacheProxy(IIdentityHostRepository identityHostRepository)
        {
            _identityHostRepository = identityHostRepository;
            _kvcacheVisitor = CacheInnerHelper.CreateKVCacheVisitor(KVCacheVisitorSetting);
        }

        private KVCacheVisitor _kvcacheVisitor;

        public async Task<IdentityHost?> QueryByName(string name, CancellationToken cancellationToken = default)
        {
            return await _kvcacheVisitor.Get(
                async (k) =>
                {
                    return await _identityHostRepository.QueryByName(name, cancellationToken);
                },
                name
                );
        }
    }
}
