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

namespace FW.TestPlatform.Main.Application
{
    [Injection(InterfaceType = typeof(IAppQuerySingleTestCaseHistory), Scope = InjectionScope.Singleton)]
    public class AppQuerySingleTestCaseHistory : IAppQuerySingleTestCaseHistory
    {
        private readonly ITestCaseRepository _testCaseRepository;
        public AppQuerySingleTestCaseHistory(ITestCaseRepository testCaseRepository)
        {
            _testCaseRepository = testCaseRepository;
        }

        public async Task<TestCaseHistoryViewData> Do(Guid caseId, Guid historyId, CancellationToken cancellationToken = default)
        {
            TestCaseHistoryViewData viewHistory = new TestCaseHistoryViewData();
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
                TestCaseHistorySummyAddModel addModel = JsonSerializerHelper.Deserialize<TestCaseHistorySummyAddModel>(history.Summary);
                viewHistory.AvgDuration = addModel.AvgDuration;
                viewHistory.ConnectCount = addModel.ConnectCount;
                viewHistory.AvgQPS = addModel.AvgQPS;
                viewHistory.CaseID = addModel.CaseID;
                viewHistory.MaxDuration = addModel.MaxDuration;
                viewHistory.MinDurartion = addModel.MinDurartion;
                viewHistory.MinDurartion = addModel.MaxQPS;
                viewHistory.MinQPS = addModel.MinQPS;
                viewHistory.ReqFailCount = addModel.ReqFailCount;
                viewHistory.ReqCount = addModel.ReqCount;
                viewHistory.CreateTime = history.CreateTime;
                viewHistory.ModifyTime = history.ModifyTime;
                viewHistory.ID = history.ID;
            }

            return viewHistory;
        }
    }
}
