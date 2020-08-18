using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary;
namespace FW.TestPlatform.Main.Entities.DAL
{
    public interface ITestCaseHistoryStore
    {
        Task Add(TestCaseHistory history, CancellationToken cancellationToken = default);
        Task Delete(Guid id, CancellationToken cancellationToken = default);
        //Task<TestCaseHistory> QueryByID(Guid caseID, Guid id, CancellationToken cancellationToken = default);
        Task<TestCaseHistory?> QueryByCase(Guid caseId, Guid historyId, CancellationToken cancellationToken = default);
        Task<QueryResult<TestCaseHistory>> QueryByPage(Guid caseID, int page, int pageSize, CancellationToken cancellationToken = default);
        Task DeleteHistories(List<Guid> ids, CancellationToken cancellationToken = default);
        Task<List<TestCaseHistory>> QueryByCaseIdAndHistoryIds(Guid caseId, List<Guid> ids, CancellationToken cancellationToken = default);

        Task UpdateNetGatewayDataFormat(TestCaseHistory source, CancellationToken cancellationToken = default);
    }
}
