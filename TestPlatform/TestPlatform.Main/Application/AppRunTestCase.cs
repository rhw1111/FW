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
    [Injection(InterfaceType = typeof(IAppRunTestCase), Scope = InjectionScope.Singleton)]
    public class AppRunTestCase : IAppRunTestCase
    {
        private readonly ITestCaseRepository _testCaseRepository;
        public AppRunTestCase(ITestCaseRepository testCaseRepository)
        {
            _testCaseRepository = testCaseRepository;
        }
        public async Task Do(TestCaseRunModel model, CancellationToken cancellationToken = default)
        {
            var testCase = await _testCaseRepository.QueryByID(model.CaseId, cancellationToken);
            if (testCase == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.NotFoundTestCaseByID,
                    DefaultFormatting = "找不到ID为{0}的测试案例",
                    ReplaceParameters = new List<object>() { model.CaseId.ToString() }
                };

                throw new UtilityException((int)TestPlatformErrorCodes.NotFoundTestCaseByID, fragment, 1, 0);
            }
            if (model.IsStop)
            {
                await testCase.Stop();
            }
            await testCase.Run();
        }
    }
}
