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
    [Injection(InterfaceType = typeof(IAppCheckTestCaseStatus), Scope = InjectionScope.Singleton)]
    public class AppCheckTestCaseStatus : IAppCheckTestCaseStatus
    {
        
        public async Task<bool> Do(Guid caseId, CancellationToken cancellationToken = default)
        {
            bool result= false;
            TestCase source = new TestCase()
            {
                ID = caseId
            };
            await source.IsEngineRun(cancellationToken);
            return result;
        }
       
    }
}
