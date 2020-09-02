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
using MSLibrary.Transaction;
using MSLibrary.Collections;

namespace FW.TestPlatform.Main.Application
{
    [Injection(InterfaceType = typeof(IAppUpdateTestDataSource), Scope = InjectionScope.Singleton)]
    public class AppUpdateTestDataSource : IAppUpdateTestDataSource
    {
        private readonly ITestDataSourceRepository _testDataSourceRepository;
        private readonly ITreeEntityRepository _treeEntityRepository;
        public AppUpdateTestDataSource(ITestDataSourceRepository testDataSourceRepository, ITreeEntityRepository treeEntityRepository)
        {
            _testDataSourceRepository = testDataSourceRepository;
            _treeEntityRepository = treeEntityRepository;
        }
        public async Task<TestDataSourceViewData> Do(TestDataSourceUpdateModel model, CancellationToken cancellationToken = default)
        {
            var source = await _testDataSourceRepository.QueryByID(model.ID);
            if (source == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.NotFoundTestCaseDataSourceByID,
                    DefaultFormatting = "找不到ID为{0}的测试数据源",
                    ReplaceParameters = new List<object>() { model.ID.ToString() }
                };

                throw new UtilityException((int)TestPlatformErrorCodes.NotFoundTestCaseDataSourceByID, fragment, 1, 0);
            }
            await using (DBTransactionScope scope = new DBTransactionScope(System.Transactions.TransactionScopeOption.Required, new System.Transactions.TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = new TimeSpan(0, 0, 30) }))
            {
                TreeEntity? tEntity = null;
                if (source.TreeID != null)
                {
                    tEntity = await _treeEntityRepository.QueryByID(source.TreeID.Value, cancellationToken);
                    if (tEntity == null)
                    {
                        var fragment = new TextFragment()
                        {
                            Code = TestPlatformTextCodes.NotFoundTreeEntityByID,
                            DefaultFormatting = "找不到ID为{0}的测试案例",
                            ReplaceParameters = new List<object>() { source.TreeID.Value.ToString() }
                        };

                        throw new UtilityException((int)TestPlatformErrorCodes.NotFoundTreeEntityByID, fragment, 1, 0);
                    }
                    if (model.Name != source.Name || (tEntity != null && model.FolderID != tEntity.ParentID))
                    {
                        await tEntity.Delete(cancellationToken);
                    }
                }
                if (source.TreeID == null || model.Name != source.Name || (tEntity != null && model.FolderID != tEntity.ParentID))
                {
                    TreeEntity treeEntity = new TreeEntity
                    {
                        ParentID = model.FolderID,
                        Value = source.ID.ToString(),
                        Name = "DS-" + source.Name,
                        ID = Guid.NewGuid(),
                        Type = TreeEntityValueServiceTypes.TestCase
                    };
                    await treeEntity.Add(cancellationToken);
                    source.TreeID = treeEntity.ID;
                }
                source.Type = model.Type;
                source.Data = model.Data;
                source.Name = model.Name;
                source.ModifyTime = DateTime.UtcNow;
                await source.Update(cancellationToken);
            }

            TestDataSourceViewData result = new TestDataSourceViewData()
            {
                ID = source.ID,
                Type = source.Type,
                Data = source.Data,
                Name = source.Name,
                CreateTime = source.CreateTime.ToCurrentUserTimeZone(),
                ModifyTime = source.ModifyTime.ToCurrentUserTimeZone()
            };

            return result;
        }
    }
}
