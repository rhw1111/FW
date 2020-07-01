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
    [Injection(InterfaceType = typeof(IAppQueryMasterLog), Scope = InjectionScope.Singleton)]
    public class AppQueryMasterLog : IAppQueryMasterLog
    {
        
        public async Task<string> Do(Guid caseId, CancellationToken cancellationToken = default)
        {
            TestCase source = new TestCase()
            {
                ID = caseId
            };
            return await source.GetMasterLog(cancellationToken);
        }
        
    }
}
