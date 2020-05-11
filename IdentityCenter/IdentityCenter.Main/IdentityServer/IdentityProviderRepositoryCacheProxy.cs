using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.Cache;

namespace IdentityCenter.Main.IdentityServer
{

    [Injection(InterfaceType = typeof(IIdentityProviderRepositoryCacheProxy), Scope = InjectionScope.Singleton)]
    public class IdentityProviderRepositoryCacheProxy : IIdentityProviderRepositoryCacheProxy
    {
        public static KVCacheVisitorSetting KVCacheVisitorSetting = new KVCacheVisitorSetting()
        {
            Name = "_IdentityProviderRepository",
            ExpireSeconds = -1,
            MaxLength = 500
        };

        private IIdentityProviderRepository _identityProviderRepository;

        public IdentityProviderRepositoryCacheProxy(IIdentityProviderRepository identityProviderRepository)
        {
            _identityProviderRepository = identityProviderRepository;
            _kvcacheVisitor = CacheInnerHelper.CreateKVCacheVisitor(KVCacheVisitorSetting);
        }

        private KVCacheVisitor _kvcacheVisitor;


        public async Task<IdentityProvider?> QueryBySchemeName(string name, CancellationToken cancellationToken = default)
        {
            return await _kvcacheVisitor.Get(
                async (k) =>
                {
                    return await _identityProviderRepository.QueryBySchemeName(name, cancellationToken);
                },
                name
                );
        }

        public async Task<IList<IdentityProvider>> QueryBySchemeName(IList<string> names, CancellationToken cancellationToken = default)
        {
            List<IdentityProvider> providers = new List<IdentityProvider>();
            foreach(var item in names)
            {
                var provider =await QueryBySchemeName(item, cancellationToken);
                if (provider!=null)
                {
                    providers.Add(provider);
                }
            }
            return providers;
        }
    }
}
