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
        Task<QueryResult<TestHost>> GetHosts(CancellationToken cancellationToken = default);
    }
}
