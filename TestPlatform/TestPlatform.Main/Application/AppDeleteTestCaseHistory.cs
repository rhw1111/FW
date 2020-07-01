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
    [Injection(InterfaceType = typeof(IAppDeleteTestCaseHistory), Scope = InjectionScope.Singleton)]
    public class AppDeleteTestCaseHistory : IAppDeleteTestCaseHistory
    {
        
        public async Task Do(Guid historyID, CancellationToken cancellationToken = default)
        {
            TestCase source = new TestCase();
            await source.DeleteHistory(historyID, cancellationToken);
        }

    }
}
