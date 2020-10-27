using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary;

namespace FW.TestPlatform.Main.Entities
{
    public interface ITestDataSourceRepository
    {
        Task<TestDataSource?> QueryByID(Guid id, CancellationToken cancellationToken = default);
        Task<TestDataSource?> QueryByName(string name, CancellationToken cancellationToken = default);
        Task<IList<TestDataSource>> QueryByNames(IList<string> names, CancellationToken cancellationToken = default);
        Task<QueryResult<TestDataSource>> QueryByPage(string matchName, int page, int pageSize, CancellationToken cancellationToken = default);
        Task DeleteMutipleTestDataSource(List<Guid> list, CancellationToken cancellationToken = default);
        IAsyncEnumerable<TestDataSource> GetDataSources(bool isJmeter, CancellationToken cancellationToken = default);
        Task<QueryResult<TestDataSource>> QueryByParentId(Guid? parentId, int page, int pageSize, CancellationToken cancellationToken = default);
        Task<TestDataSource?> QueryByTreeEntityNameAndParentID(Guid? parentId, string name, CancellationToken cancellationToken = default);
    }
}
