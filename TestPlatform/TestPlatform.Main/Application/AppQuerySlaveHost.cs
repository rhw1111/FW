using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.DI;
using FW.TestPlatform.Main.DTOModel;
using FW.TestPlatform.Main.Entities;

namespace FW.TestPlatform.Main.Application
{
    [Injection(InterfaceType = typeof(IAppQuerySlaveHost), Scope = InjectionScope.Singleton)]
    public class AppQuerySlaveHost : IAppQuerySlaveHost
    {
        public IAsyncEnumerable<TestCaseSlaveHost> Do(Guid caseId, CancellationToken cancellationToken = default)
        {
            TestCase source = new TestCase()
            {
                ID = caseId
            };
            return source.GetAllSlaveHosts(cancellationToken);
        }

    }
}
