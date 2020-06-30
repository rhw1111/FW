using FW.TestPlatform.Main.DTOModel;
using MSLibrary;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary.DI;
using FW.TestPlatform.Main.Entities;
using MSLibrary.LanguageTranslate;

namespace FW.TestPlatform.Main.Application
{
    [Injection(InterfaceType = typeof(IAppQuerySingleTestCase), Scope = InjectionScope.Singleton)]
    public class AppQuerySingleTestCase : IAppQuerySingleTestCase
    {
        private readonly ITestCaseRepository _testCaseRepository;

        public AppQuerySingleTestCase(ITestCaseRepository testCaseRepository)
        {
            _testCaseRepository = testCaseRepository;
        }

        //public async Task<QueryResult<TestCaseViewData>> Do(string matchName, int page, int pageSize, CancellationToken cancellationToken = default)
        //{
        //    QueryResult<TestCaseViewData> result = new QueryResult<TestCaseViewData>();
        //    var queryResult=await _testCaseRepository.QueryByPage(matchName, page, pageSize, cancellationToken);

        //    result.CurrentPage = queryResult.CurrentPage;
        //    result.TotalCount = queryResult.TotalCount;

        //    foreach(var item in queryResult.Results)
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
        //                ModifyTime= item.ModifyTime
        //            }
        //            );
        //    }

        //    return result;
        //}
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
            return new TestCaseViewData()
            {
                ID = queryResult.ID,
                Name = queryResult.Name,
                Configuration = queryResult.Configuration,
                Owner = queryResult.Owner,
                MasterHost = queryResult.MasterHost,
                Status = queryResult.Status,
                EngineType = queryResult.EngineType
            };
        }
    }
}
