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
    [Injection(InterfaceType = typeof(IAppUpdateTestCase), Scope = InjectionScope.Singleton)]
    public class AppUpdateTestCase : IAppUpdateTestCase
    {
        private readonly ITestCaseRepository _testCaseRepository;
        public AppUpdateTestCase(ITestCaseRepository testCaseRepository)
        {
            _testCaseRepository = testCaseRepository;
        }

        public async Task<TestCaseViewData> Do(TestCaseUpdateModel model, CancellationToken cancellationToken = default)
        {
            var source = await _testCaseRepository.QueryByID(model.ID, cancellationToken);
            if (source == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.NotFoundTestCaseByID,
                    DefaultFormatting = "找不到ID为{0}的测试案例",
                    ReplaceParameters = new List<object>() { model.ID.ToString() }
                };

                throw new UtilityException((int)TestPlatformErrorCodes.NotFoundTestCaseByID, fragment, 1, 0);
            }
            source.Name = model.Name;
            //queryResult.OwnerID = model.OwnerID;
            source.EngineType = model.EngineType;
            source.MasterHostID = model.MasterHostID;
            source.Configuration = model.Configuration;
            source.ModifyTime = DateTime.UtcNow;
            await source.Update(cancellationToken);

            return new TestCaseViewData()
            {
                ID = source.ID,
                EngineType = source.EngineType,
                Configuration = source.Configuration,
                OwnerID = source.OwnerID,
                Name = source.Name,
                Status = source.Status,
                MasterHostID = source.MasterHostID,
                CreateTime = source.CreateTime.ToCurrentUserTimeZone(),
                ModifyTime = source.ModifyTime.ToCurrentUserTimeZone()
            };
        }

    }
}
