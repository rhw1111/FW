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

namespace FW.TestPlatform.Main.Application
{
    [Injection(InterfaceType = typeof(IAppUpdateSlaveHost), Scope = InjectionScope.Singleton)]
    public class AppUpdateSlaveHost : IAppUpdateSlaveHost
    {
        private readonly ITestCaseRepository _testCaseRepository;
        public AppUpdateSlaveHost(ITestCaseRepository testCaseRepository)
        {
            _testCaseRepository = testCaseRepository;
        }
        public async Task<TestCaseSlaveHostViewData> Do(TestCaseSlaveHostUpdateModel slaveHost, CancellationToken cancellationToken = default)
        {
            var testCase = await _testCaseRepository.QueryByID(slaveHost.TestCaseID, cancellationToken);
            if (testCase == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.NotFoundTestCaseByID,
                    DefaultFormatting = "找不到ID为{0}的测试案例",
                    ReplaceParameters = new List<object>() { slaveHost.TestCaseID.ToString() }
                };

                throw new UtilityException((int)TestPlatformErrorCodes.NotFoundTestCaseByID, fragment, 1, 0);
            }
            TestCaseSlaveHost? tCaseSlaveHost = await testCase.GetSlaveHost(slaveHost.ID, cancellationToken);
            if (tCaseSlaveHost == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.NotFoundSlaveHostInCase,
                    DefaultFormatting = "在id为{0}的测试案例中找不到id为{1}的从测试主机",
                    ReplaceParameters = new List<object>() { slaveHost.TestCaseID.ToString(), slaveHost.HostID.ToString() }
                };

                throw new UtilityException((int)TestPlatformErrorCodes.NotFoundSlaveHostInCase, fragment, 1, 0);
            }
            TestCaseSlaveHost newSlaveHost = new TestCaseSlaveHost()
            {
                ID = slaveHost.ID,
                TestCaseID = slaveHost.TestCaseID,
                HostID = slaveHost.HostID,
                Count = slaveHost.Count,
                ExtensionInfo = slaveHost.ExtensionInfo,
                SlaveName = slaveHost.SlaveName,
                ModifyTime = DateTime.UtcNow
            };
            //tCaseSlaveHost.TestCaseID = slaveHost.TestCaseID;
            //tCaseSlaveHost.HostID = slaveHost.HostID;
            //tCaseSlaveHost.Count = slaveHost.Count;
            //tCaseSlaveHost.ExtensionInfo = slaveHost.ExtensionInfo;
            //tCaseSlaveHost.SlaveName = slaveHost.SlaveName;
            //tCaseSlaveHost.ModifyTime = DateTime.UtcNow;

            await testCase.UpdateSlaveHost(newSlaveHost, cancellationToken);

            return new TestCaseSlaveHostViewData()
            {
                ID = newSlaveHost.ID,
                TestCaseID = newSlaveHost.TestCaseID,
                HostID = newSlaveHost.HostID,
                Count = newSlaveHost.Count,
                ExtensionInfo = newSlaveHost.ExtensionInfo,
                SlaveName = newSlaveHost.SlaveName,
                CreateTime = tCaseSlaveHost.CreateTime.ToCurrentUserTimeZone()
            };
        }
    }
}
