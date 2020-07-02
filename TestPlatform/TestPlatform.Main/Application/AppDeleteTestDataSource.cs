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
    [Injection(InterfaceType = typeof(IAppDeleteTestDataSource), Scope = InjectionScope.Singleton)]
    public class AppDeleteTestDataSource : IAppDeleteTestDataSource
    {
        private readonly ITestDataSourceRepository _testDataSourceRepository;
        public AppDeleteTestDataSource(ITestDataSourceRepository testDataSourceRepository)
        {
            _testDataSourceRepository = testDataSourceRepository;
        }
        public async Task Do(Guid id, CancellationToken cancellationToken = default)
        {
            var source = await _testDataSourceRepository.QueryByID(id);
            if (source == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.NotFoundTestCaseDataSourceByID,
                    DefaultFormatting = "找不到ID为{0}的测试数据源",
                    ReplaceParameters = new List<object>() { id.ToString() }
                };

                throw new UtilityException((int)TestPlatformErrorCodes.NotFoundTestCaseDataSourceByID, fragment, 1, 0);
            }

            await source.Delete(cancellationToken);
        }
    }
}
