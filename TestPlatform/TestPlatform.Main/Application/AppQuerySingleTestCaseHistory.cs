using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.DI;
using FW.TestPlatform.Main.DTOModel;
using FW.TestPlatform.Main.Entities;
using MSLibrary.Serializer;
using MSLibrary.LanguageTranslate;
using FW.TestPlatform.Main.Configuration;

namespace FW.TestPlatform.Main.Application
{
    [Injection(InterfaceType = typeof(IAppQuerySingleTestCaseHistory), Scope = InjectionScope.Singleton)]
    public class AppQuerySingleTestCaseHistory : IAppQuerySingleTestCaseHistory
    {
        private readonly ITestCaseRepository _testCaseRepository;
        private readonly ISystemConfigurationService _systemConfigurationService;
        public AppQuerySingleTestCaseHistory(ITestCaseRepository testCaseRepository, ISystemConfigurationService systemConfigurationService)
        {
            _testCaseRepository = testCaseRepository;
            _systemConfigurationService = systemConfigurationService;
        }

        public async Task<TestCaseHistoryDetailViewData> Do(Guid caseId, Guid historyId, CancellationToken cancellationToken = default)
        {
            TestCaseHistoryDetailViewData viewHistory = new TestCaseHistoryDetailViewData();
            var queryResult = await _testCaseRepository.QueryByID(caseId, cancellationToken);
            if (queryResult == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.NotFoundTestCaseByID,
                    DefaultFormatting = "找不到ID为{0}的测试案例",
                    ReplaceParameters = new List<object>() { caseId.ToString() }
                };

                throw new UtilityException((int)TestPlatformErrorCodes.NotFoundTestCaseByID, fragment, 1, 0);
            }
            var history = await queryResult.GetHistory(historyId, cancellationToken);
            if (history != null)
            {
                string from = ConvertDateTimeToInt(history.CreateTime.AddHours(-1)).ToString();
                string to = ConvertDateTimeToInt(history.CreateTime.AddHours(1)).ToString();
                var monitorAddress = await _systemConfigurationService.GetCaseHistoryMonitorAddressAsync(cancellationToken);
                viewHistory.ID = history.ID;
                viewHistory.CaseID = history.CaseID;
                viewHistory.Summary = history.Summary;
                viewHistory.CreateTime = history.CreateTime.ToCurrentUserTimeZone();
                viewHistory.NetGatewayDataFormat = history.NetGatewayDataFormat == null ? "" : history.NetGatewayDataFormat;
                viewHistory.MonitorUrl = $"{monitorAddress}&var-HistoryCaseID={history.ID.ToString().ToUrlEncode()}&from={from}&to={to}";
            }

            return viewHistory;
        }
        private long ConvertDateTimeToInt(DateTime time)
        {
            DateTime Time = (new System.DateTime(1970, 1, 1, 0, 0, 0, 0)).ToCurrentUserTimeZone();
            long TimeStamp = (time.Ticks - Time.Ticks) / 10000;   //除10000调整为13位     
            return TimeStamp;
        }
    }
}
