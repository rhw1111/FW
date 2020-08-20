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
    [Injection(InterfaceType = typeof(IAppCheckNetGatewayDataAnalysisStatus), Scope = InjectionScope.Singleton)]
    public class AppCheckNetGatewayDataAnalysisStatus : IAppCheckNetGatewayDataAnalysisStatus
    {
        private readonly ITestCaseRepository _testCaseRepository;
        public AppCheckNetGatewayDataAnalysisStatus(ITestCaseRepository testCaseRepository)
        {
            _testCaseRepository = testCaseRepository;
        }
        public async Task<NetGatewayDataFileStatus> Do(Guid caseId, Guid historyId, CancellationToken cancellationToken = default)
        {
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
            string rel = await testCase.CheckNetGatewayDataAnalysisStatus(historyId, cancellationToken);
            if (rel == string.Empty)
                return NetGatewayDataFileStatus.NoFileUnfinished;
            else
                return NetGatewayDataFileStatus.HasFileUnfinished;
        }
    }
}
