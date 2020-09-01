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
using MSLibrary.Serializer;
using FW.TestPlatform.Main.Configuration;

namespace FW.TestPlatform.Main.Application
{
    [Injection(InterfaceType = typeof(IAppQueryTestCaseHistory), Scope = InjectionScope.Singleton)]
    public class AppQueryTestCaseHistory : IAppQueryTestCaseHistory
    {
        private readonly ITestCaseRepository _testCaseRepository;
        private readonly ISystemConfigurationService _systemConfigurationService;
        public AppQueryTestCaseHistory(ITestCaseRepository testCaseRepository, ISystemConfigurationService systemConfigurationService)
        {
            _testCaseRepository = testCaseRepository;
            _systemConfigurationService = systemConfigurationService;
        }
        public async Task<QueryResult<TestCaseHistoryListViewData>> Do(Guid caseId, int page, int pageSize, CancellationToken cancellationToken = default)
        {
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
            QueryResult<TestCaseHistoryListViewData> histories = new QueryResult<TestCaseHistoryListViewData>();
            var result = await queryResult.GetHistories(caseId, page, pageSize, cancellationToken);
            if(result != null && result.Results != null && result.Results.Count > 0)
            {
                var monitorAddress = await _systemConfigurationService.GetCaseHistoryMonitorAddressAsync(cancellationToken);
                foreach (TestCaseHistory history in result.Results)
                {
                    string from = ConvertDateTimeToInt(history.CreateTime.AddHours(-1)).ToString();
                    string to = ConvertDateTimeToInt(history.CreateTime.AddHours(1)).ToString();
                    histories.Results.Add(new TestCaseHistoryListViewData()
                    {
                        ID = history.ID,
                        CaseID = history.CaseID,
                        MonitorUrl = $"{monitorAddress}&var-HistoryCaseID={history.ID.ToString().ToUrlEncode()}&from={from}&to={to}",
                        CreateTime = history.CreateTime.ToCurrentUserTimeZone()
                    });
                }
                histories.TotalCount = result.TotalCount;
                histories.CurrentPage = result.CurrentPage;
            }
            return histories;
        }

        private long ConvertDateTimeToInt(DateTime time)
        {
            DateTime Time = (new System.DateTime(1970, 1, 1, 0, 0, 0, 0)).ToCurrentUserTimeZone();
            long TimeStamp = (time.Ticks - Time.Ticks) / 10000;   //除10000调整为13位     
            return TimeStamp;
        }

    }
}
