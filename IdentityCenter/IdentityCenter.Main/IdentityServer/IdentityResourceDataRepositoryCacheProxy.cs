using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using MSLibrary.DI;
using MSLibrary.Cache;

namespace IdentityCenter.Main.IdentityServer
{
    [Injection(InterfaceType = typeof(IIdentityResourceDataRepositoryCacheProxy), Scope = InjectionScope.Singleton)]
    public class IdentityResourceDataRepositoryCacheProxy : IIdentityResourceDataRepositoryCacheProxy
    {
        public static KVCacheVisitorSetting KVCacheVisitorSetting = new KVCacheVisitorSetting()
        {
            Name = "_IdentityResourceDataRepository",
            ExpireSeconds = -1,
            MaxLength = 5
        };

        private IIdentityResourceDataRepository _identityResourceDataRepository;

        public IdentityResourceDataRepositoryCacheProxy(IIdentityResourceDataRepository identityResourceDataRepository)
        {
            _identityResourceDataRepository = identityResourceDataRepository;
            _kvcacheVisitor = CacheInnerHelper.CreateKVCacheVisitor(KVCacheVisitorSetting);
        }

        private KVCacheVisitor _kvcacheVisitor;


        public async Task<IList<IdentityResourceData>> QueryAllEnabled(CancellationToken cancellationToken = default)
        {
            var resourceList = await _kvcacheVisitor.Get(
                async (k) =>
                {
                    return await _identityResourceDataRepository.QueryAllEnabled(cancellationToken);
                },
                "All"
                );

            return resourceList;
        }

        public async Task<IList<IdentityResourceData>> QueryEnabled(IList<string> names, CancellationToken cancellationToken = default)
        {
            var resourceList = await _kvcacheVisitor.Get(
                async (k) =>
                {
                    return await _identityResourceDataRepository.QueryAllEnabled(cancellationToken);
                },
                "All"
                );

            var result = (from item in resourceList
                          where names.Contains(item.Name)
                          select item).ToList();
            return result;
        }
    }
}
