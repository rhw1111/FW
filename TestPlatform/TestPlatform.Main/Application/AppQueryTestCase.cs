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

        //public async Task<QueryResult<TestCaseViewData>> GetByPage(int page, int pageSize, CancellationToken cancellationToken = default)
        //{
        //    QueryResult<TestCaseViewData> result = new QueryResult<TestCaseViewData>();
        //    var queryResult = await _testCaseRepository.QueryByPage(page, pageSize, cancellationToken);

        //    result.CurrentPage = queryResult.CurrentPage;
        //    result.TotalCount = queryResult.TotalCount;

        //    foreach (var item in queryResult.Results)
        //    {
        //        result.Results.Add(
        //            new TestCaseViewData()
        //            {
        //                ID = item.ID,
        //                Name = item.Name,
        //                EngineType = item.EngineType,
        //                Configuration = item.Configuration,
        //                Status = item.Status,
        //                MasterHostID = item.MasterHostID,
        //                MasterHost = item.MasterHost,
        //                Owner = item.Owner,
        //                OwnerID = item.OwnerID,
        //                CreateTime = item.CreateTime,
        //                ModifyTime = item.ModifyTime
        //            }
        //            );
        //    }

        //    return result;
        //}

        //public async Task<TestCaseViewData?> GetCase(Guid id, CancellationToken cancellationToken = default)
        //{
        //    TestCaseViewData? result = null;
        //    var queryResult = await _testCaseRepository.QueryByID(id, cancellationToken);
        //    if(queryResult != null)
        //    {
        //        result = new TestCaseViewData
        //        {
        //            ID = queryResult.ID,
        //            Name = queryResult.Name,
        //            Configuration = queryResult.Configuration,
        //            Owner = queryResult.Owner,
        //            MasterHost = queryResult.MasterHost,
        //            Status = queryResult.Status,
        //            EngineType = queryResult.EngineType
        //        };
        //    }
        //    return result;
        //}
    }
}
