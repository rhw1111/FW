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

namespace FW.TestPlatform.Main.Application
{
    [Injection(InterfaceType = typeof(IAppUpdateTestDataSource), Scope = InjectionScope.Singleton)]
    public class AppUpdateTestDataSource : IAppUpdateTestDataSource
    {
        private readonly ITestDataSourceRepository _testDataSourceRepository;
        public AppUpdateTestDataSource(ITestDataSourceRepository testDataSourceRepository)
        {
            _testDataSourceRepository = testDataSourceRepository;
        }
        public async Task<TestDataSourceViewData> Do(TestDataSourceUpdateModel model, CancellationToken cancellationToken = default)
        {
            var source = await _testDataSourceRepository.QueryByID(model.ID);
            if (source == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.NotFoundTestCaseDataSourceByID,
                    DefaultFormatting = "找不到ID为{0}的测试数据源",
                    ReplaceParameters = new List<object>() { model.ID.ToString() }
                };

                throw new UtilityException((int)TestPlatformErrorCodes.NotFoundTestCaseDataSourceByID, fragment, 1, 0);
            }
            source.Type = model.Type;
            source.Data = model.Data;
            source.Name = model.Name;
            source.ModifyTime = DateTime.UtcNow;
            await source.Update(cancellationToken);

            TestDataSourceViewData result = new TestDataSourceViewData()
            {
                ID = source.ID,
                Type = source.Type,
                Data = source.Data,
                Name = source.Name,
                CreateTime = source.CreateTime.ToCurrentUserTimeZone(),
                ModifyTime = source.ModifyTime.ToCurrentUserTimeZone()
            };

            return result;
        }
    }
}
