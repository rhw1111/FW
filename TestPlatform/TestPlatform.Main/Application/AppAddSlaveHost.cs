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
    [Injection(InterfaceType = typeof(IAppAddSlaveHost), Scope = InjectionScope.Singleton)]
    public class AppAddSlaveHost : IAppAddSlaveHost
    {      
        public async Task<TestCaseSlaveHost> Do(TestCaseSlaveHostAddModel slaveHost, CancellationToken cancellationToken = default)
        {
            TestCase source = new TestCase()
            {
                ID = slaveHost.TestCaseID
            };
            TestCaseSlaveHost testCaseSlaveHost = new TestCaseSlaveHost()
            {
                SlaveName = slaveHost.SlaveName,
                ExtensionInfo = slaveHost.ExtensionInfo,
                HostID = slaveHost.HostID,
                TestCaseID = slaveHost.TestCaseID,
                Count = slaveHost.Count
            };
            await source.AddSlaveHost(testCaseSlaveHost, cancellationToken);
            return testCaseSlaveHost;
        }
        
    }
}
