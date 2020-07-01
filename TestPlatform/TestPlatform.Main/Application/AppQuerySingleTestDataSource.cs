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
    [Injection(InterfaceType = typeof(IAppQuerySingleTestDataSource), Scope = InjectionScope.Singleton)]
    public class AppQuerySingleTestDataSource : IAppQuerySingleTestDataSource
    {
        private readonly ITestDataSourceRepository _testDataSourceRepository;
        public AppQuerySingleTestDataSource(ITestDataSourceRepository testDataSourceRepository)
        {
            _testDataSourceRepository = testDataSourceRepository;
        }     
        public async Task<TestDataSourceViewData> Do(Guid id, CancellationToken cancellationToken = default)
        {
            TestDataSourceViewData result = new TestDataSourceViewData();
            var item = await _testDataSourceRepository.QueryByID(id);
            if (item == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.NotFoundTestCaseDataSourceByID,
                    DefaultFormatting = "找不到ID为{0}的测试数据源",
                    ReplaceParameters = new List<object>() { id.ToString() }
                };

                throw new UtilityException((int)TestPlatformErrorCodes.NotFoundTestCaseDataSourceByID, fragment, 1, 0);
            }
            result = new TestDataSourceViewData()
            {
                ID = item.ID,
                Name = item.Name,
                Type = item.Type,
                Data = item.Data,
                CreateTime = item.CreateTime.ToCurrentUserTimeZone(),
                ModifyTime = item.ModifyTime.ToCurrentUserTimeZone()
            };
            return result;
        }
    }
}
