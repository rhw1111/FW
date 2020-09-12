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
            TestDataSource newSource = new TestDataSource()
            {
                Name = model.Name,
                Data = model.Data,
                Type = model.Type,
                ID = source.ID
            };
            await using (DBTransactionScope scope = new DBTransactionScope(System.Transactions.TransactionScopeOption.Required, new System.Transactions.TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = new TimeSpan(0, 0, 30) }))
            {
                TreeEntity? tEntity = null;
                TreeEntity newTreeEntity = new TreeEntity()
                {
                    Name = model.Name,
                    Type = TreeEntityValueServiceTypes.TestDataSource,
                    Value = source.ID.ToString(),
                    ParentID = model.FolderID
                };
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
                        newTreeEntity.ID = tEntity.ID;
                        await newTreeEntity.Update(cancellationToken);
                    }
                }
                else
                {
                    newTreeEntity.ID = Guid.NewGuid();
                    await newTreeEntity.Add(cancellationToken);
                    newSource.TreeID = newTreeEntity.ID;
                }
                await newSource.Update(cancellationToken);
                scope.Complete();
            }

            return new TestDataSourceViewData()
            {
                ID = newSource.ID,
                Type = newSource.Type,
                Data = newSource.Data,
                Name = newSource.Name,
                CreateTime = newSource.CreateTime.ToCurrentUserTimeZone(),
                ModifyTime = newSource.ModifyTime.ToCurrentUserTimeZone()
            };
        }
    }
}
