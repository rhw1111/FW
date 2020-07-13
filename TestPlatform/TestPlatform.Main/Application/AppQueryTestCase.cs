using FW.TestPlatform.Main.DTOModel;
using MSLibrary;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary.DI;
using FW.TestPlatform.Main.Entities;

namespace FW.TestPlatform.Main.Application
{
    [Injection(InterfaceType = typeof(IAppQueryTestCase), Scope = InjectionScope.Singleton)]
    public class AppQueryTestCase : IAppQueryTestCase
    {
        private readonly ITestCaseRepository _testCaseRepository;

        public AppQueryTestCase(ITestCaseRepository testCaseRepository)
        {
            _testCaseRepository = testCaseRepository;
        }

        public async Task<QueryResult<TestCaseListViewData>> Do(string matchName, int page, int pageSize, CancellationToken cancellationToken = default)
        {
            QueryResult<TestCaseListViewData> result = new QueryResult<TestCaseListViewData>();
            var queryResult=await _testCaseRepository.QueryByPage(matchName, page, pageSize, cancellationToken);

            result.CurrentPage = queryResult.CurrentPage;
            result.TotalCount = queryResult.TotalCount;

            foreach(var item in queryResult.Results)
            {
                result.Results.Add(
                    new TestCaseListViewData()
                    {
                        ID = item.ID,
                        Name = item.Name,
                        EngineType = item.EngineType,
                        Configuration = item.Configuration,
                        CreateTime = item.CreateTime
                    }
                    );
            }

            return result;
        }
    }
}
