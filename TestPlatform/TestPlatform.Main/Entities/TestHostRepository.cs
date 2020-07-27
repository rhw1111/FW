using MSLibrary;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary.DI;
using FW.TestPlatform.Main.Entities.DAL;

namespace FW.TestPlatform.Main.Entities
{
    [Injection(InterfaceType = typeof(ITestHostRepository), Scope = InjectionScope.Singleton)]
    public class TestHostRepository : ITestHostRepository
    {
        private readonly ITestHostStore _testHostStore;

        public TestHostRepository(ITestHostStore testHostStore)
        {
            _testHostStore = testHostStore;
        }

        public async Task<TestHost?> QueryByName(string address, CancellationToken cancellationToken = default)
        {
            return await _testHostStore.QueryByName(address, cancellationToken);
        }
        public async Task<Guid?> QueryByNameNoLock(string address, CancellationToken cancellationToken = default)
        {
            return await _testHostStore.QueryByNameNoLock(address, cancellationToken);
        }
        public async Task<QueryResult<TestHost>> QueryByPage(string matchAddress, int page, int pageSize, CancellationToken cancellationToken = default)
        {
            return await _testHostStore.QueryByPage(matchAddress, page, pageSize, cancellationToken);
        }

        public async Task<TestHost?> QueryByID(Guid id, CancellationToken cancellationToken = default)
        {
            return await _testHostStore.QueryByID(id, cancellationToken);
        }

        public IAsyncEnumerable<TestHost> GetHosts(CancellationToken cancellationToken = default)
        {
            return _testHostStore.GetHosts(cancellationToken);
        }

        public async Task DeleteTestHosts(List<Guid> ids, CancellationToken cancellationToken = default)
        {
            await _testHostStore.DeleteTestHosts(ids, cancellationToken);
        }


        public async Task<bool> GetTestHostsBySSHEndpointId(Guid sshEndPointId, CancellationToken cancellationToken = default)
        {
            return await _testHostStore.GetTestHostsBySSHEndpointId(sshEndPointId, cancellationToken);
        }
    }
}
