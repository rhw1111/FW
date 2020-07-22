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
                viewHistory.ID = history.ID;
                viewHistory.CaseID = history.CaseID;
                viewHistory.Summary = history.Summary;
                viewHistory.CreateTime = history.CreateTime.ToCurrentUserTimeZone();
            }

            return viewHistory;
        }
    }
}
