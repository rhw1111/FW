using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary.CommandLine.SSH.DAL;
using MSLibrary.DI;

namespace MSLibrary.CommandLine.SSH
{
    [Injection(InterfaceType = typeof(ISSHEndpointRepository), Scope = InjectionScope.Singleton)]
    public class SSHEndpointRepository : ISSHEndpointRepository
    {
        private ISSHEndpointStore _sshEndpointStore;

        public SSHEndpointRepository(ISSHEndpointStore sshEndpointStore)
        {
            _sshEndpointStore = sshEndpointStore;
        }

        public async Task<SSHEndpoint?> QueryByName(string name, CancellationToken cancellationToken = default)
        {
            return await _sshEndpointStore.QueryByName(name, cancellationToken);
        }

        public async Task<SSHEndpoint?> QueryByID(Guid id, CancellationToken cancellationToken = default)
        {
            return await _sshEndpointStore.QueryByID(id, cancellationToken);
        }
        public async Task<QueryResult<SSHEndpoint>> QueryByPage(string matchName, int page, int pageSize, CancellationToken cancellationToken = default)
        {
            return await _sshEndpointStore.QueryByPage(matchName, page, pageSize, cancellationToken);
        }
        public async Task<Guid?> QueryByNameNoLock(string name, CancellationToken cancellationToken = default)
        {
            return await _sshEndpointStore.QueryByNameNoLock(name, cancellationToken);
        }

        public async Task DeleteMutiple(List<Guid> ids, CancellationToken cancellationToken = default)
        {
            await _sshEndpointStore.DeleteMutiple(ids, cancellationToken);
        }
    }
}
