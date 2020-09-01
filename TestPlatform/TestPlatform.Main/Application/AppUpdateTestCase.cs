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
using MSLibrary.Transaction;
using MSLibrary.Collections;
using Microsoft.AspNetCore.Http.Features.Authentication;

namespace FW.TestPlatform.Main.Application
{
    [Injection(InterfaceType = typeof(IAppUpdateTestCase), Scope = InjectionScope.Singleton)]
    public class AppUpdateTestCase : IAppUpdateTestCase
    {
        private readonly ITestCaseRepository _testCaseRepository;
        private readonly ITreeEntityRepository _treeEntityRepository;
        public AppUpdateTestCase(ITestCaseRepository testCaseRepository, ITreeEntityRepository treeEntityRepository)
        {
            _testCaseRepository = testCaseRepository;
            _treeEntityRepository = treeEntityRepository;
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
                if(source.TreeID == null || model.Name != source.Name || (tEntity != null && model.FolderID != tEntity.ParentID))
                {
                    TreeEntity treeEntity = new TreeEntity
                    {
                        ParentID = model.FolderID,
                        Value = source.ID.ToString(),
                        Name = "C-" + source.Name,
                        ID = Guid.NewGuid(),
                        Type = TreeEntityValueServiceTypes.TestCase
                    };
                    await treeEntity.Add(cancellationToken);
                    source.TreeID = treeEntity.ID;
                }
                source.Name = model.Name;
                source.EngineType = model.EngineType;
                source.MasterHostID = model.MasterHostID;
                source.Configuration = model.Configuration;
                source.ModifyTime = DateTime.UtcNow;
                await source.Update(cancellationToken);

                scope.Complete();
            }

            return new TestCaseViewData()
            {
                ID = source.ID,
                EngineType = source.EngineType,
                Configuration = source.Configuration,
                Name = source.Name,
                Status = source.Status,
                MasterHostID = source.MasterHostID,
                CreateTime = source.CreateTime.ToCurrentUserTimeZone()
            };
        }

    }
}
