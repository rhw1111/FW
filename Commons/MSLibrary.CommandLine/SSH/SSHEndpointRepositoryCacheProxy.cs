using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.Cache;

namespace MSLibrary.CommandLine.SSH
{
    [Injection(InterfaceType = typeof(ISSHEndpointRepositoryCacheProxy), Scope = InjectionScope.Singleton)]
    public class SSHEndpointRepositoryCacheProxy : ISSHEndpointRepositoryCacheProxy
    {
        public static KVCacheVisitorSetting KVCacheVisitorSetting = new KVCacheVisitorSetting()
        {
            Name = "_SystemConfigurationRepository",
            ExpireSeconds = 600,
            MaxLength = 500
        };


        private ISSHEndpointRepository _sshEndpointRepository;

        public SSHEndpointRepositoryCacheProxy(ISSHEndpointRepository sshEndpointRepository)
        {
            _sshEndpointRepository = sshEndpointRepository;
            _kvcacheVisitor = CacheInnerHelper.CreateKVCacheVisitor(KVCacheVisitorSetting);
        }

        private KVCacheVisitor _kvcacheVisitor;


        public async Task<SSHEndpoint?> QueryByName(string name, CancellationToken cancellationToken = default)
        {
            return await _kvcacheVisitor.Get(
                async (k) =>
                {
                    return await _sshEndpointRepository.QueryByName(name);
                }, name
                );
        }
    }
}
