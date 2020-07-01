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
    [Injection(InterfaceType = typeof(IAppUpdateSlaveHost), Scope = InjectionScope.Singleton)]
    public class AppUpdateSlaveHost : IAppUpdateSlaveHost
    {
        
        public async Task Do(TestCaseSlaveHostAddModel slaveHost, CancellationToken cancellationToken = default)
        {
            TestCase source = new TestCase()
            {
                ID = slaveHost.TestCaseID
            };
            TestCaseSlaveHost tCaseSlaveHost = new TestCaseSlaveHost() {
                ID = slaveHost.ID,
                TestCaseID = slaveHost.TestCaseID,
                HostID = slaveHost.HostID,
                Count = slaveHost.Count,
                ExtensionInfo = slaveHost.ExtensionInfo,
                SlaveName = slaveHost.SlaveName,
                CreateTime = DateTime.UtcNow,
                ModifyTime = DateTime.UtcNow
            };
            await source.UpdateSlaveHost(tCaseSlaveHost, cancellationToken);
        }
    }
}
