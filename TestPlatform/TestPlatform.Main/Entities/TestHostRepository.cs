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
    [Injection(InterfaceType = typeof(ITestHostRepository), Scope = InjectionScope.Singleton)]
    public class TestHostRepository : ITestHostRepository
    {
        private readonly ITestHostStore _testHostStore;

        public TestHostRepository(ITestHostStore testHostStore)
        {
            _testHostStore = testHostStore;
        }

        public async Task<TestHost?> QueryByID(Guid id, CancellationToken cancellationToken = default)
        {
            return await _testHostStore.QueryByID(id, cancellationToken);
        }
    }
}
