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
    [Injection(InterfaceType = typeof(IAppUpdateNetGatewayDataFormat), Scope = InjectionScope.Singleton)]
    public class AppUpdateNetGatewayDataFormat : IAppUpdateNetGatewayDataFormat
    {
        private readonly ITestCaseRepository _testCaseRepository;
        public AppUpdateNetGatewayDataFormat(ITestCaseRepository testCaseRepository)
        {
            _testCaseRepository = testCaseRepository;
        }
        public async Task Do(TestCaseHistoryUpdateData historyData, CancellationToken cancellationToken = default)
        {
            var testCase = await _testCaseRepository.QueryByID(historyData.CaseID, cancellationToken);
            if (testCase == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.NotFoundTestCaseByID,
                    DefaultFormatting = "找不到ID为{0}的测试案例",
                    ReplaceParameters = new List<object>() { historyData.CaseID.ToString() }
                };

                throw new UtilityException((int)TestPlatformErrorCodes.NotFoundTestCaseByID, fragment, 1, 0);
            }
            var history = await testCase.GetHistory(historyData.ID, cancellationToken);
            if (history != null)
            {
                history.NetGatewayDataFormat = historyData.NetGatewayDataFormat;
                await testCase.UpdateNetGatewayDataFormat(history, cancellationToken);
            }  
        }
    }
}
