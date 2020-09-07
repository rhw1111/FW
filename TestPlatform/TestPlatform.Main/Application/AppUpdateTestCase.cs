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
            var tCase = await _testCaseRepository.QueryByID(model.ID, cancellationToken);
            if (tCase == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.NotFoundTestCaseByID,
                    DefaultFormatting = "找不到ID为{0}的测试案例",
                    ReplaceParameters = new List<object>() { model.ID.ToString() }
                };

                throw new UtilityException((int)TestPlatformErrorCodes.NotFoundTestCaseByID, fragment, 1, 0);
            }
            TestCase newCase = new TestCase()
            {
                ID = tCase.ID,
                Name = model.Name,
                EngineType = model.EngineType,
                MasterHostID = model.MasterHostID,
                Configuration = model.Configuration,
                ModifyTime = DateTime.UtcNow
            };
            TreeEntity newTreeEntity = new TreeEntity()
            {
                Name = model.Name,
                Type = TreeEntityValueServiceTypes.TestCase,
                Value = tCase.ID.ToString(),
                ParentID = model.FolderID
            };
            await using (DBTransactionScope scope = new DBTransactionScope(System.Transactions.TransactionScopeOption.Required, new System.Transactions.TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = new TimeSpan(0, 0, 30) }))
            {
                TreeEntity? tEntity = null;
                if (tCase.TreeID != null)
                {
                    tEntity = await _treeEntityRepository.QueryByID(tCase.TreeID.Value, cancellationToken);
                    if (tEntity == null)
                    {
                        var fragment = new TextFragment()
                        {
                            Code = TestPlatformTextCodes.NotFoundTreeEntityByID,
                            DefaultFormatting = "找不到ID为{0}的节点",
                            ReplaceParameters = new List<object>() { tCase.TreeID.Value.ToString() }
                        };

                        throw new UtilityException((int)TestPlatformErrorCodes.NotFoundTreeEntityByID, fragment, 1, 0);
                    }
                    if (model.Name != tCase.Name || (tEntity != null && model.FolderID != tEntity.ParentID))
                    {
                        newTreeEntity.ID = tEntity.ID;
                        await newTreeEntity.Update(cancellationToken);
                    }
                }
                else
                {
                    newTreeEntity.ID = Guid.NewGuid();
                    await newTreeEntity.Add(cancellationToken);
                    newCase.TreeID = newTreeEntity.ID;
                }
                await newCase.Update(cancellationToken);
                scope.Complete();
            }

            return new TestCaseViewData()
            {
                ID = newCase.ID,
                EngineType = newCase.EngineType,
                Configuration = newCase.Configuration,
                Name = newCase.Name,
                Status = newCase.Status,
                MasterHostID = newCase.MasterHostID,
                CreateTime = newCase.CreateTime.ToCurrentUserTimeZone()
            };
        }

    }
}
