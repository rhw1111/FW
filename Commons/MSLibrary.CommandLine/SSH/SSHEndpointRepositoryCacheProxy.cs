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
            Name = "_SSHEndpointRepository",
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
            return (await _kvcacheVisitor.Get(
                async (k) =>
                {
                    var obj= await _sshEndpointRepository.QueryByName(name, cancellationToken);
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
