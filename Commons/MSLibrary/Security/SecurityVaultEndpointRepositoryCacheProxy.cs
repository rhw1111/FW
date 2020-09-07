using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.Cache;

namespace MSLibrary.Security
{
    [Injection(InterfaceType = typeof(ISecurityVaultEndpointRepositoryCacheProxy), Scope = InjectionScope.Singleton)]
    public class SecurityVaultEndpointRepositoryCacheProxy : ISecurityVaultEndpointRepositoryCacheProxy
    {
        public static KVCacheVisitorSetting KVCacheVisitorSetting = new KVCacheVisitorSetting()
        {
            Name = "_SecurityVaultEndpointRepository",
            ExpireSeconds = -1,
            MaxLength = 2000
        };

        private ISecurityVaultEndpointRepository _securityVaultEndpointRepository;

        public SecurityVaultEndpointRepositoryCacheProxy(ISecurityVaultEndpointRepository securityVaultEndpointRepository)
        {
            _securityVaultEndpointRepository = securityVaultEndpointRepository;
            _kvcacheVisitor = CacheInnerHelper.CreateKVCacheVisitor(KVCacheVisitorSetting);
        }

        private KVCacheVisitor _kvcacheVisitor;


        public async Task<SecurityVaultEndpoint> QueryByName(string name)
        {
            return (await _kvcacheVisitor.Get(
                async (k) =>
                {
                    var obj= await _securityVaultEndpointRepository.QueryByName(name);
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
