using FW.TestPlatform.Main.DTOModel;
using MSLibrary;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary.DI;
using FW.TestPlatform.Main.Entities;
using MSLibrary.LanguageTranslate;
using MSLibrary.Collections;

namespace FW.TestPlatform.Main.Application
{
    [Injection(InterfaceType = typeof(IAppQuerySingleTestDataSource), Scope = InjectionScope.Singleton)]
    public class AppQuerySingleTestDataSource : IAppQuerySingleTestDataSource
    {
        private readonly ITestDataSourceRepository _testDataSourceRepository;
        private readonly ITreeEntityRepository _treeEntityRepository;
        public AppQuerySingleTestDataSource(ITestDataSourceRepository testDataSourceRepository, ITreeEntityRepository treeEntityRepository)
        {
            _testDataSourceRepository = testDataSourceRepository;
            _treeEntityRepository = treeEntityRepository;
        }     
        public async Task<TestDataSourceViewData> Do(Guid id, CancellationToken cancellationToken = default)
        {
            TestDataSourceViewData result = new TestDataSourceViewData();
            var item = await _testDataSourceRepository.QueryByID(id);
            if (item == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.NotFoundTestCaseDataSourceByID,
                    DefaultFormatting = "找不到ID为{0}的测试数据源",
                    ReplaceParameters = new List<object>() { id.ToString() }
                };

                throw new UtilityException((int)TestPlatformErrorCodes.NotFoundTestCaseDataSourceByID, fragment, 1, 0);
            }
            var parentName = string.Empty;
            Guid? parentId = null;
            if (item.TreeID != null)
            {
                TreeEntity? entityWithParent = await _treeEntityRepository.QueryWithParentByID(item.TreeID.Value, cancellationToken);
                if (entityWithParent != null && entityWithParent.Parent != null)
                {
                    parentName = entityWithParent.Parent.Name;
                    parentId = entityWithParent.ParentID;
                }
            }
            result = new TestDataSourceViewData()
            {
                ID = item.ID,
                Name = item.Name,
                Type = item.Type,
                Data = item.Data,
                TreeID = item.TreeID,
                CreateTime = item.CreateTime.ToCurrentUserTimeZone(),
                ModifyTime = item.ModifyTime.ToCurrentUserTimeZone(),
                ParentName = parentName,
                ParentID = parentId
            };
            return result;
        }
    }
}
