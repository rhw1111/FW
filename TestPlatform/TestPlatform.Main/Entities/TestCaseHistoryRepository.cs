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
    [Injection(InterfaceType = typeof(ITestCaseHistoryRepository), Scope = InjectionScope.Singleton)]
    public class TestCaseHistoryRepository : ITestCaseHistoryRepository
    {
        private readonly ITestCaseHistoryStore _testCaseHistoryStore;

        public TestCaseHistoryRepository(ITestCaseHistoryStore testCaseHistoryStore)
        {
            _testCaseHistoryStore = testCaseHistoryStore;
        }

        public async Task Add(TestCaseHistory source, CancellationToken cancellationToken = default)
        {
            await _testCaseHistoryStore.Add(source,cancellationToken);
        }

        public async Task Delete(Guid id, CancellationToken cancellationToken = default)
        {
            await _testCaseHistoryStore.Delete(id, cancellationToken);
        }

        public async Task<TestCaseHistory> QueryByID(Guid caseID, Guid id, CancellationToken cancellationToken = default)
        {
           return await _testCaseHistoryStore.QueryByID(caseID,id, cancellationToken);
        }

        public async Task<QueryResult<TestCaseHistory>> QueryByPage(Guid caseID, int page, int pageSize, CancellationToken cancellationToken = default)
        {
            return await _testCaseHistoryStore.QueryByPage(caseID, page, pageSize, cancellationToken);
        }
    }
}
