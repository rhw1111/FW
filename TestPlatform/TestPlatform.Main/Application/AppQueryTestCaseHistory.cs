﻿using System;
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

namespace FW.TestPlatform.Main.Application
{
    [Injection(InterfaceType = typeof(IAppQueryTestCaseHistory), Scope = InjectionScope.Singleton)]
    public class AppQueryTestCaseHistory : IAppQueryTestCaseHistory
    {
        private readonly ITestCaseRepository _testCaseRepository;
        public AppQueryTestCaseHistory(ITestCaseRepository testCaseRepository)
        {
            _testCaseRepository = testCaseRepository;
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
                foreach (TestCaseHistory history in result.Results)
                {           
                    histories.Results.Add(new TestCaseHistoryListViewData()
                    {
                        ID = history.ID,
                        CaseID = history.CaseID,
                        CreateTime = history.CreateTime.ToCurrentUserTimeZone()
                    });
                }
                histories.TotalCount = result.TotalCount;
                histories.CurrentPage = result.CurrentPage;
            }
            return histories;
        }

    }
}
