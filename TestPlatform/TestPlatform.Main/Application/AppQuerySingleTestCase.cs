using MSLibrary;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.DI;
using FW.TestPlatform.Main.Entities;
using MSLibrary.LanguageTranslate;
using FW.TestPlatform.Main.Configuration;
using FW.TestPlatform.Main.DTOModel;


namespace FW.TestPlatform.Main.Application
{
    [Injection(InterfaceType = typeof(IAppQuerySingleTestCase), Scope = InjectionScope.Singleton)]
    public class AppQuerySingleTestCase : IAppQuerySingleTestCase
    {
        private readonly ITestCaseRepository _testCaseRepository;
        private readonly ISystemConfigurationService _systemConfigurationService;

        public AppQuerySingleTestCase(ITestCaseRepository testCaseRepository, ISystemConfigurationService systemConfigurationService)
        {
            _testCaseRepository = testCaseRepository;
            _systemConfigurationService = systemConfigurationService;
        }
        public async Task<TestCaseViewData> Do(Guid id, CancellationToken cancellationToken = default)
        {
            var queryResult = await _testCaseRepository.QueryByID(id, cancellationToken);
            if (queryResult == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.NotFoundTestCaseByID,
                    DefaultFormatting = "找不到ID为{0}的测试案例",
                    ReplaceParameters = new List<object>() { id }
                };

                throw new UtilityException((int)TestPlatformErrorCodes.NotFoundTestCaseByID, fragment, 1, 0);
            }

            var monitorAddress=await _systemConfigurationService.GetMonitorAddressAsync(queryResult.EngineType, cancellationToken);


            return new TestCaseViewData()
            {
                ID = queryResult.ID,
                Name = queryResult.Name,
                MonitorUrl=$"{monitorAddress}&var-CaseID={queryResult.ID.ToString().ToUrlEncode()}&from=now-15m&refresh=5s&to=now",
                Configuration = queryResult.Configuration,
                Status = queryResult.Status,
                EngineType = queryResult.EngineType,
                MasterHostID = queryResult.MasterHostID,
                MasterHostAddress = queryResult.MasterHost.Address,
                CreateTime = queryResult.CreateTime.ToCurrentUserTimeZone()
            };
        }
    }
}
