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
    [Injection(InterfaceType = typeof(IAppCheckTestCaseStatus), Scope = InjectionScope.Singleton)]
    public class AppCheckTestCaseStatus : IAppCheckTestCaseStatus
    {
        private readonly ITestCaseRepository _testCaseRepository;
        public AppCheckTestCaseStatus(ITestCaseRepository testCaseRepository)
        {
            _testCaseRepository = testCaseRepository;
        }

        public async Task<bool> Do(Guid caseId, CancellationToken cancellationToken = default)
        {
            bool result= false;
            var testCase = await _testCaseRepository.QueryByID(caseId, cancellationToken);
            if (testCase == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.NotFoundTestCaseByID,
                    DefaultFormatting = "找不到ID为{0}的测试案例",
                    ReplaceParameters = new List<object>() { caseId.ToString() }
                };

                throw new UtilityException((int)TestPlatformErrorCodes.NotFoundTestCaseByID, fragment, 1, 0);
            }
            result = await testCase.IsEngineRun(cancellationToken);
            return result;
        }
       
    }
}
