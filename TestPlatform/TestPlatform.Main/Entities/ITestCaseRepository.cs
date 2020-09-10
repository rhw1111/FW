using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary;

namespace FW.TestPlatform.Main.Entities
{
    public interface ITestCaseRepository
    {
        Task<TestCase?> QueryByID(Guid id, CancellationToken cancellationToken = default);
        Task<TestCaseStatus?> QueryTestCaseStatusByID(Guid id, CancellationToken cancellationToken = default);
        Task<TestCase?> QueryByName(string name, CancellationToken cancellationToken = default);
        Task<IList<TestCase>> QueryByNames(IList<string> names, CancellationToken cancellationToken = default);
        Task<QueryResult<TestCase>> QueryByPage(string matchName, int page, int pageSize, CancellationToken cancellationToken = default);
        Task<QueryResult<TestCase>> QueryByPage(int page, int pageSize, CancellationToken cancellationToken = default);
        Task DeleteMutiple(List<Guid> ids, CancellationToken cancellationToken = default);
        Task<QueryResult<TestCase>> QueryByParentId(Guid? parentId, int page, int pageSize, CancellationToken cancellationToken = default);
    }
}
