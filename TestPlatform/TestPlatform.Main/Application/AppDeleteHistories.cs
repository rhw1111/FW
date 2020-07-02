using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.DI;
using FW.TestPlatform.Main.DTOModel;
using FW.TestPlatform.Main.Entities;
using FW.TestPlatform.Main.Configuration;
using System.Linq;
using MSLibrary.LanguageTranslate;

namespace FW.TestPlatform.Main.Application
{
    [Injection(InterfaceType = typeof(IAppDeleteHistories), Scope = InjectionScope.Singleton)]
    public class AppDeleteHistories : IAppDeleteHistories
    {
        private readonly ITestCaseRepository _testCaseRepository;
        public AppDeleteHistories(ITestCaseRepository testCaseRepository)
        {
            _testCaseRepository = testCaseRepository;
        }

        public async Task Do(Guid caseId, List<Guid> historyIds, CancellationToken cancellationToken = default)
        {
            var teseCase = await _testCaseRepository.QueryByID(caseId, cancellationToken);
            if (teseCase == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.NotFoundTestCaseByID,
                    DefaultFormatting = "找不到ID为{0}的测试案例",
                    ReplaceParameters = new List<object>() { caseId.ToString() }
                };

                throw new UtilityException((int)TestPlatformErrorCodes.NotFoundTestCaseByID, fragment, 1, 0);
            }
            await teseCase.DeleteHistories(historyIds);
            //await _testCaseRepository.DeleteMutiple(slaveIds, cancellationToken);
        }

    }
}
