using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary;

namespace FW.TestPlatform.Main.Entities.DAL
{
    public interface ITestHostStore
    {
        Task Add(TestHost host, CancellationToken cancellationToken = default);
        Task Update(TestHost host, CancellationToken cancellationToken = default);
        Task Delete(Guid id, CancellationToken cancellationToken = default);
        Task<TestHost> QueryByID(Guid id, CancellationToken cancellationToken = default);
        Task<QueryResult<TestHost>> QueryByPage(string matchAddress, int page, int pageSize, CancellationToken cancellationToken = default);

    }
}
