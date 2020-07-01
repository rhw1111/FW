﻿using System;
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

            TestCaseHistory history = new TestCaseHistory();
            history.CaseID = model.CaseID;
            //history.Case = testCase;                
            history.Summary = JsonSerializerHelper.Serializer(model);

            await testCase.AddHistory(history);
        }
    }
}