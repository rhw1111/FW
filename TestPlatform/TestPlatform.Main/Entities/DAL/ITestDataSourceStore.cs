using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary;

namespace FW.TestPlatform.Main.Entities.DAL
{
    public interface ITestDataSourceStore
    {
        Task Add(TestDataSource source, CancellationToken cancellationToken = default);
        Task Update(TestDataSource source, CancellationToken cancellationToken = default);
        Task Delete(Guid id, CancellationToken cancellationToken = default);
        Task<TestDataSource?> QueryByID(Guid id, CancellationToken cancellationToken = default);
        Task<TestDataSource?> QueryByName(string name, CancellationToken cancellationToken = default);
        Task<IList<TestDataSource>> QueryByNames(IList<string> names, CancellationToken cancellationToken = default);
        Task<QueryResult<TestDataSource>> QueryByPage(string matchName, int page, int pageSize, CancellationToken cancellationToken = default);
    }
}
