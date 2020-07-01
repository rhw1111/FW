using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.DI;
using FW.TestPlatform.Main.DTOModel;
using FW.TestPlatform.Main.Entities;
using System.Linq;
using System.Diagnostics.Tracing;

namespace FW.TestPlatform.Main.Application
{
    [Injection(InterfaceType = typeof(IAppAddTestCaseHistory), Scope = InjectionScope.Singleton)]
    public class AppAddTestCaseHistory : IAppAddTestCaseHistory
    {
        public async Task Do(TestCaseHistorySummyAddModel model, CancellationToken cancellationToken = default)
        {
            TestCase source = new TestCase()
            {
                ID = model.CaseID
            };

            await source.AddHistory(model);
        }
    }
}
