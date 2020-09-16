using MSLibrary;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary.DI;
using FW.TestPlatform.Main.Entities;
using MSLibrary.LanguageTranslate;
using FW.TestPlatform.Main.Configuration;
using FW.TestPlatform.Main.DTOModel;
using MSLibrary.Collections;

namespace FW.TestPlatform.Main.Application
{
    [Injection(InterfaceType = typeof(IAppQuerySingleTestCase), Scope = InjectionScope.Singleton)]
    public class AppQuerySingleTestCase : IAppQuerySingleTestCase
    {
        private readonly ITestCaseRepository _testCaseRepository;
        private readonly ISystemConfigurationService _systemConfigurationService;
        private readonly ITreeEntityRepository _treeEntityRepository;

        public AppQuerySingleTestCase(ITestCaseRepository testCaseRepository, ISystemConfigurationService systemConfigurationService, ITreeEntityRepository treeEntityRepository)
        {
            _testCaseRepository = testCaseRepository;
            _systemConfigurationService = systemConfigurationService;
            _treeEntityRepository = treeEntityRepository;
        }
        public async Task<TestCaseViewData> Do(Guid id, CancellationToken cancellationToken = default)
        {
            var queryResult = await _testCaseRepository.QueryByID(id, cancellationToken);
            if (queryResult == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.NotFoundTestCaseByID,
                    DefaultFormatting = "找不到ID为{0}的测试案例",
                    ReplaceParameters = new List<object>() { id }
                };

                throw new UtilityException((int)TestPlatformErrorCodes.NotFoundTestCaseByID, fragment, 1, 0);
            }

            var monitorAddress=await _systemConfigurationService.GetMonitorAddressAsync(queryResult.EngineType, cancellationToken);
            var parentName = string.Empty;
            Guid? parentId = null;
            if(queryResult.TreeID != null)
            {
                TreeEntity? entityWithParent = await _treeEntityRepository.QueryWithParentByID(queryResult.TreeID.Value, cancellationToken);
                if (entityWithParent != null && entityWithParent.Parent != null)
                {
                    parentName = entityWithParent.Parent.Name;
                    parentId = entityWithParent.ParentID;
                }
            }

            return new TestCaseViewData()
            {
                ID = queryResult.ID,
                Name = queryResult.Name,
                MonitorUrl = $"{monitorAddress}&var-CaseID={queryResult.ID.ToString().ToUrlEncode()}&from=now-15m&refresh=5s&to=now",
                Configuration = queryResult.Configuration,
                Status = queryResult.Status,
                EngineType = queryResult.EngineType,
                TreeID = queryResult.TreeID,
                MasterHostID = queryResult.MasterHostID,
                MasterHostAddress = queryResult.MasterHost.Address,
                CreateTime = queryResult.CreateTime.ToCurrentUserTimeZone(),
                ParentName = parentName,
                ParentID = parentId
            };
        }
    }
}
