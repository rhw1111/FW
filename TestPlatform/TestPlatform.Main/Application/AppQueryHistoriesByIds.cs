using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.DI;
using FW.TestPlatform.Main.DTOModel;
using FW.TestPlatform.Main.Entities;
using FW.TestPlatform.Main.Configuration;
using System.Linq;
using MSLibrary.LanguageTranslate;

namespace FW.TestPlatform.Main.Application
{
    [Injection(InterfaceType = typeof(IAppQueryHistoriesByIds), Scope = InjectionScope.Singleton)]
    public class AppQueryHistoriesByIds : IAppQueryHistoriesByIds
    {
        private readonly ITestCaseRepository _testCaseRepository;
        public AppQueryHistoriesByIds(ITestCaseRepository testCaseRepository)
        {
            _testCaseRepository = testCaseRepository;
        }

        public async Task<List<TestCaseHistoryDetailViewData>> Do(Guid caseId, List<Guid> historyIds, CancellationToken cancellationToken = default)
        {
            var teseCase = await _testCaseRepository.QueryByID(caseId, cancellationToken);
            if (teseCase == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.NotFoundTestCaseByID,
                    DefaultFormatting = "找不到ID为{0}的测试案例",
                    ReplaceParameters = new List<object>() { caseId.ToString() }
                };

                throw new UtilityException((int)TestPlatformErrorCodes.NotFoundTestCaseByID, fragment, 1, 0);
            }
            List<TestCaseHistoryDetailViewData> historyListViewData = new List<TestCaseHistoryDetailViewData>();
            List<TestCaseHistory> historyList = await teseCase.GetHistoriesByCaseIdAndIds(historyIds, cancellationToken);
            foreach (TestCaseHistory history in historyList)
            {
                historyListViewData.Add(new TestCaseHistoryDetailViewData()
                {
                    ID = history.ID,
                    CaseID = history.CaseID,
                    Summary = history.Summary,
                    CreateTime = history.CreateTime.ToCurrentUserTimeZone()
                }) ;
            }
            return historyListViewData;
        }
    }
}
