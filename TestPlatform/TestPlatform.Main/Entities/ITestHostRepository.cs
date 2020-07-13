using MSLibrary;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FW.TestPlatform.Main.Entities
{
    public interface ITestHostRepository
    {
        Task<TestHost?> QueryByID(Guid id, CancellationToken cancellationToken = default);
        IAsyncEnumerable<TestHost> GetHosts(CancellationToken cancellationToken = default);
        Task<TestHost?> QueryByName(string address, CancellationToken cancellationToken = default);
        Task<Guid?> QueryByNameNoLock(string address, CancellationToken cancellationToken = default);
        Task<QueryResult<TestHost>> QueryByPage(string matchAddress, int page, int pageSize, CancellationToken cancellationToken = default);
        Task DeleteTestHosts(List<Guid> ids, CancellationToken cancellationToken = default);
    }
}
