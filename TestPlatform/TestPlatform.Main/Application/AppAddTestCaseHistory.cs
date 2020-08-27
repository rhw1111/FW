using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.DI;
using FW.TestPlatform.Main.DTOModel;
using FW.TestPlatform.Main.Entities;
using System.Linq;
using System.Diagnostics.Tracing;
using MSLibrary.LanguageTranslate;
using MSLibrary.Serializer;

namespace FW.TestPlatform.Main.Application
{
    [Injection(InterfaceType = typeof(IAppAddTestCaseHistory), Scope = InjectionScope.Singleton)]
    public class AppAddTestCaseHistory : IAppAddTestCaseHistory
    {
        private readonly ITestCaseRepository _testCaseRepository;

        public AppAddTestCaseHistory(ITestCaseRepository testCaseRepository)
        {
            _testCaseRepository = testCaseRepository;
        }

        public async Task Do(TestCaseHistorySummyAddModel model, CancellationToken cancellationToken = default)
        {
            TestCase? testCase = await _testCaseRepository.QueryByID(model.CaseID);
            if (testCase == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.NotFoundTestCaseByID,
                    DefaultFormatting = "找不到ID为{0}的测试案例",
                    ReplaceParameters = new List<object>() { model.CaseID }
                };
                throw new UtilityException((int)TestPlatformErrorCodes.NotFoundTestCaseByID, fragment, 1, 0);
            }
            if (testCase.TestCaseHistoryID == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.NotFoundTestCaseByID,
                    DefaultFormatting = "生成测试案例的历史记录，ID为{0}的测试案例，测试历史记录ID为空",
                    ReplaceParameters = new List<object>() { model.CaseID }
                };
                throw new UtilityException((int)TestPlatformErrorCodes.NotFoundTestCaseByID, fragment, 1, 0);
            }

            TestCaseHistory history = new TestCaseHistory();
            history.CaseID = model.CaseID;
            history.ID = (Guid)testCase.TestCaseHistoryID;               
            history.Summary = JsonSerializerHelper.Serializer(model);
            history.NetGatewayDataFormat = string.Empty;
            await testCase.AddHistory(history);
        }
    }
}
