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
    [Injection(InterfaceType = typeof(IAppQueryTestCaseHistory), Scope = InjectionScope.Singleton)]
    public class AppQueryTestCaseHistory : IAppQueryTestCaseHistory
    {
        
        public async Task<QueryResult<TestCaseHistory>> Do(Guid caseID, int page, int pageSize, CancellationToken cancellationToken = default)
        {
            TestCase source = new TestCase()
            {
                ID = caseID
            };
           return await source.GetHistories(caseID, page, pageSize, cancellationToken);
        }

    }
}
