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
    [Injection(InterfaceType = typeof(IAppQueryTestDataSource), Scope = InjectionScope.Singleton)]
    public class AppQueryTestDataSource : IAppQueryTestDataSource
    {
        private readonly ITestDataSourceRepository _testDataSourceRepository;

        public AppQueryTestDataSource(ITestDataSourceRepository testDataSourceRepository)
        {
            _testDataSourceRepository = testDataSourceRepository;
        }
        public async Task<QueryResult<TestDataSourceViewData>> Do(Guid? parentId, string matchName, int page, int pageSize, CancellationToken cancellationToken = default)
        {
            QueryResult<TestDataSourceViewData> result = new QueryResult<TestDataSourceViewData>();
            var queryResult=await _testDataSourceRepository.QueryByParentId(parentId, page, pageSize, cancellationToken);

            result.CurrentPage = queryResult.CurrentPage;
            result.TotalCount = queryResult.TotalCount;
            if (queryResult.Results != null && queryResult.Results.Count > 0)
            {
                foreach (var item in queryResult.Results)
                {
                    result.Results.Add(
                        new TestDataSourceViewData()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Type = item.Type,
                            Data = item.Data,
                            TreeID = item.TreeID,
                            CreateTime = item.CreateTime.ToCurrentUserTimeZone(),
                            ModifyTime = item.ModifyTime.ToCurrentUserTimeZone()
                        }
                        );
                }
            }

            return result;
        }
    }
}
