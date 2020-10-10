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
    [Injection(InterfaceType = typeof(IAppQueryTestCaseHostPorts), Scope = InjectionScope.Singleton)]
    public class AppQueryTestCaseHostPorts : IAppQueryTestCaseHostPorts
    {
        private readonly ITestCaseRepository _testCaseRepository;
        public AppQueryTestCaseHostPorts(ITestCaseRepository testCaseRepository)
        {
            _testCaseRepository = testCaseRepository;
        }
        public async Task<List<TestCaseHostPortCheckModel>> Do(List<Guid> caseIds, CancellationToken cancellationToken = default)
        {
            List<TestCaseHostPortCheckModel> list = new List<TestCaseHostPortCheckModel>();
            foreach (Guid id in caseIds)
            {
                var tCase = await _testCaseRepository.QueryByID(id, cancellationToken);
                if (tCase == null)
                {
                    var fragment = new TextFragment()
                    {
                        Code = TestPlatformTextCodes.NotFoundTestCaseByID,
                        DefaultFormatting = "找不到ID为{0}的测试案例",
                        ReplaceParameters = new List<object>() { id.ToString() }
                    };

                    throw new UtilityException((int)TestPlatformErrorCodes.NotFoundTestCaseByID, fragment, 1, 0);
                }
                List<TestCase> conflictedCases = await tCase.StatusCheck(cancellationToken);
                list.Add(new TestCaseHostPortCheckModel()
                {
                    IsAvailable = conflictedCases.Count > 0 ? false : true,
                    ConflictedNames = string.Join(",", conflictedCases.Select(tc => tc.Name).ToArray()),
                    ID = tCase.ID,
                    Name = tCase.Name
                });
            }
            return list;
        }
    }
}
