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
    [Injection(InterfaceType = typeof(ITestCaseRepository), Scope = InjectionScope.Singleton)]
    public class TestCaseRepository : ITestCaseRepository
    {
        private readonly ITestCaseStore _testCaseStore;

        public TestCaseRepository(ITestCaseStore testCaseStore)
        {
            _testCaseStore = testCaseStore;
        }

        public async Task<TestCase?> QueryByID(Guid id, CancellationToken cancellationToken = default)
        {
            return await _testCaseStore.QueryByID(id, cancellationToken);
        }

        public async Task<TestCase?> QueryByName(string name, CancellationToken cancellationToken = default)
        {
            return await _testCaseStore.QueryByName(name, cancellationToken);
        }

        public async Task<IList<TestCase>> QueryByNames(IList<string> names, CancellationToken cancellationToken = default)
        {
            return await _testCaseStore.QueryByNames(names, cancellationToken);
        }

        public async Task<QueryResult<TestCase>> QueryByPage(string matchName, int page, int pageSize, CancellationToken cancellationToken = default)
        {
            return await _testCaseStore.QueryByPage(matchName, page, pageSize, cancellationToken);
        }

        public async Task<QueryResult<TestCase>> QueryByPage(int page, int pageSize, CancellationToken cancellationToken = default)
        {
            return await _testCaseStore.QueryByPage(page, pageSize, cancellationToken);
        }
    }
}
