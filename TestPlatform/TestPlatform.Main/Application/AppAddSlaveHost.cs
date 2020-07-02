using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.DI;
using FW.TestPlatform.Main.DTOModel;
using FW.TestPlatform.Main.Entities;
using MSLibrary.LanguageTranslate;
using Microsoft.OData.Edm;

namespace FW.TestPlatform.Main.Application
{
    [Injection(InterfaceType = typeof(IAppAddSlaveHost), Scope = InjectionScope.Singleton)]
    public class AppAddSlaveHost : IAppAddSlaveHost
    {
        private readonly ITestCaseRepository _testCaseRepository;
        public AppAddSlaveHost(ITestCaseRepository testCaseRepository)
        {
            _testCaseRepository = testCaseRepository;
        }
        public async Task<TestCaseSlaveHostViewData> Do(TestCaseSlaveHostAddModel slaveHost, CancellationToken cancellationToken = default)
        {
            var queryResult = await _testCaseRepository.QueryByID(slaveHost.TestCaseID, cancellationToken);
            if (queryResult == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.NotFoundTestCaseByID,
                    DefaultFormatting = "找不到ID为{0}的测试案例",
                    ReplaceParameters = new List<object>() { slaveHost.TestCaseID.ToString() }
                };

                throw new UtilityException((int)TestPlatformErrorCodes.NotFoundTestCaseByID, fragment, 1, 0);
            }
            TestCaseSlaveHost testCaseSlaveHost = new TestCaseSlaveHost()
            {
                SlaveName = slaveHost.SlaveName,
                ExtensionInfo = slaveHost.ExtensionInfo,
                HostID = slaveHost.HostID,
                TestCaseID = slaveHost.TestCaseID,
                Count = slaveHost.Count,
                CreateTime = DateTime.UtcNow,
                ModifyTime = DateTime.UtcNow
            };
            await queryResult.AddSlaveHost(testCaseSlaveHost, cancellationToken);
            return new TestCaseSlaveHostViewData() {
                ID = testCaseSlaveHost.ID,
                SlaveName = testCaseSlaveHost.SlaveName,
                ExtensionInfo = testCaseSlaveHost.ExtensionInfo,
                HostID = testCaseSlaveHost.HostID,
                TestCaseID = testCaseSlaveHost.TestCaseID,
                Count = testCaseSlaveHost.Count,
                CreateTime = testCaseSlaveHost.CreateTime.ToCurrentUserTimeZone()
            };
        }
        
    }
}
