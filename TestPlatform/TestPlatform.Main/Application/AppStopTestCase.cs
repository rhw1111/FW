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
    [Injection(InterfaceType = typeof(IAppStopTestCase), Scope = InjectionScope.Singleton)]
    public class AppStopTestCase : IAppStopTestCase
    {
        private readonly ITestCaseRepository _testCaseRepository;
        public AppStopTestCase(ITestCaseRepository testCaseRepository)
        {
            _testCaseRepository = testCaseRepository;
        }
        public async Task Do(Guid id, CancellationToken cancellationToken = default)
        {
            var testCase = await _testCaseRepository.QueryByID(id, cancellationToken);
            if (testCase == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.NotFoundTestCaseByID,
                    DefaultFormatting = "找不到ID为{0}的测试案例",
                    ReplaceParameters = new List<object>() { id.ToString() }
                };

                throw new UtilityException((int)TestPlatformErrorCodes.NotFoundTestCaseByID, fragment, 1, 0);
            }
            await testCase.Stop();
        }
    }
}
