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
    [Injection(InterfaceType = typeof(IAppDeleteTestCaseHistory), Scope = InjectionScope.Singleton)]
    public class AppDeleteTestCaseHistory : IAppDeleteTestCaseHistory
    {
        private readonly ITestCaseRepository _testCaseRepository;
        public AppDeleteTestCaseHistory(ITestCaseRepository testCaseRepository)
        {
            _testCaseRepository = testCaseRepository;
        }
        public async Task Do(Guid caseId, Guid historyID, CancellationToken cancellationToken = default)
        {
            var queryResult = await _testCaseRepository.QueryByID(caseId, cancellationToken);
            if (queryResult == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.NotFoundTestCaseByID,
                    DefaultFormatting = "找不到ID为{0}的测试案例",
                    ReplaceParameters = new List<object>() { caseId.ToString() }
                };

                throw new UtilityException((int)TestPlatformErrorCodes.NotFoundTestCaseByID, fragment, 1, 0);
            }
            await queryResult.DeleteHistory(historyID, cancellationToken);
        }

    }
}
