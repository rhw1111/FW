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
    [Injection(InterfaceType = typeof(ITestCaseSlaveHostRepository), Scope = InjectionScope.Singleton)]
    public class TestCaseSlaveHostRepository : ITestCaseSlaveHostRepository
    {
        private readonly ITestCaseSlaveHostStore _testCaseSlaveHostStore;

        public TestCaseSlaveHostRepository(ITestCaseSlaveHostStore testCaseSlaveHostStore)
        {
            _testCaseSlaveHostStore = testCaseSlaveHostStore;
        }

        public Task Add(TestCaseSlaveHost slaveHost, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task Delete(Guid id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public IAsyncEnumerable<TestCaseSlaveHost> QueryByCase(Guid caseID, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<TestCaseSlaveHost?> QueryByCase(Guid caseID, Guid slaveHostID, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<TestCaseSlaveHost?> QueryByID(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task Update(TestCaseSlaveHost slaveHost, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
