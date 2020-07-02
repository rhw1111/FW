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
    [Injection(InterfaceType = typeof(IAppDeleteSlaveHosts), Scope = InjectionScope.Singleton)]
    public class AppDeleteSlaveHosts : IAppDeleteSlaveHosts
    {
        private readonly ITestCaseRepository _testCaseRepository;
        public AppDeleteSlaveHosts(ITestCaseRepository testCaseRepository)
        {
            _testCaseRepository = testCaseRepository;
        }

        public async Task Do(Guid caseId, List<Guid> slaveIds, CancellationToken cancellationToken = default)
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
            await teseCase.DeleteSlaveHosts(slaveIds);
            //await _testCaseRepository.DeleteMutiple(slaveIds, cancellationToken);
        }

    }
}
