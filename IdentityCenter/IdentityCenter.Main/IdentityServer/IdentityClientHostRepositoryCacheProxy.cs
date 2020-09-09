using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.Cache;

namespace IdentityCenter.Main.IdentityServer
{
    [Injection(InterfaceType = typeof(IIdentityClientHostRepositoryCacheProxy), Scope = InjectionScope.Singleton)]
    public class IdentityClientHostRepositoryCacheProxy : IIdentityClientHostRepositoryCacheProxy
    {
        public static KVCacheVisitorSetting KVCacheVisitorSetting = new KVCacheVisitorSetting()
        {
            Name = "_IdentityClientHostRepository",
            ExpireSeconds = -1,
            MaxLength = 500
        };

        private IIdentityClientHostRepository _identityClientHostRepository;

        public IdentityClientHostRepositoryCacheProxy(IIdentityClientHostRepository identityClientHostRepository)
        {
            _identityClientHostRepository = identityClientHostRepository;
            _kvcacheVisitor = CacheInnerHelper.CreateKVCacheVisitor(KVCacheVisitorSetting);
        }

        private KVCacheVisitor _kvcacheVisitor;

        public async Task<IdentityClientHost?> QueryByName(string name, CancellationToken cancellationToken = default)
        {
            return (await _kvcacheVisitor.Get(
                async (k) =>
                {
                    var obj= await _identityClientHostRepository.QueryByName(name, cancellationToken);
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
