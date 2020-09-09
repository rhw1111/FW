using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using MSLibrary.DI;
using MSLibrary.Cache;
namespace IdentityCenter.Main.IdentityServer.ClientBindings
{
    [Injection(InterfaceType = typeof(IIdentityClientOpenIDBindingRepositoryCacheProxy), Scope = InjectionScope.Singleton)]
    public class IdentityClientOpenIDBindingRepositoryCacheProxy : IIdentityClientOpenIDBindingRepositoryCacheProxy
    {
        public static KVCacheVisitorSetting KVCacheVisitorSetting = new KVCacheVisitorSetting()
        {
            Name = "_IdentityClientOpenIDBindingRepository",
            ExpireSeconds = -1,
            MaxLength = 500
        };

        private IIdentityClientOpenIDBindingRepository _identityClientOpenIDBindingRepository;

        public IdentityClientOpenIDBindingRepositoryCacheProxy(IIdentityClientOpenIDBindingRepository identityClientOpenIDBindingRepository)
        {
            _identityClientOpenIDBindingRepository = identityClientOpenIDBindingRepository;
            _kvcacheVisitor = CacheInnerHelper.CreateKVCacheVisitor(KVCacheVisitorSetting);
        }

        private KVCacheVisitor _kvcacheVisitor;

        public async Task<IdentityClientOpenIDBinding?> QueryByName(string name, CancellationToken cancellationToken = default)
        {
            return (await _kvcacheVisitor.Get(
                async (k) =>
                {
                    var obj= await _identityClientOpenIDBindingRepository.QueryByName(name, cancellationToken);
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
