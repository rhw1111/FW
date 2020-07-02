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
    [Injection(InterfaceType = typeof(IAppQuerySlaveHost), Scope = InjectionScope.Singleton)]
    public class AppQuerySlaveHost : IAppQuerySlaveHost
    {
        private readonly ITestCaseRepository _testCaseRepository;
        public AppQuerySlaveHost(ITestCaseRepository testCaseRepository)
        {
            _testCaseRepository = testCaseRepository;
        }
        public async Task<List<TestCaseSlaveHostViewData>> Do(Guid caseId, CancellationToken cancellationToken = default)
        {
            var testCase = await _testCaseRepository.QueryByID(caseId, cancellationToken);
            if (testCase == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.NotFoundTestCaseByID,
                    DefaultFormatting = "找不到ID为{0}的测试案例",
                    ReplaceParameters = new List<object>() { caseId.ToString() }
                };

                throw new UtilityException((int)TestPlatformErrorCodes.NotFoundTestCaseByID, fragment, 1, 0);
            }
            //return source.GetAllSlaveHosts(cancellationToken);
            List<TestCaseSlaveHostViewData> list = new List<TestCaseSlaveHostViewData>();
            var slaveHosts = testCase.GetAllSlaveHosts(cancellationToken);
            await foreach (var item in slaveHosts)
            {
                list.Add(new TestCaseSlaveHostViewData()
                {
                    ID = item.ID,
                    TestCaseID = item.TestCaseID,
                    HostID = item.HostID,
                    Count = item.Count,
                    ExtensionInfo = item.ExtensionInfo,
                    SlaveName = item.SlaveName
                });
            }
            return list;
        }

    }
}
