using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.Collections;
using MSLibrary.Collections.DAL;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using MSLibrary.Transaction;

namespace FW.TestPlatform.Main.Entities.EntityTreeCopyServices
{
    [Injection(InterfaceType = typeof(EntityTreeCopyServiceForTestCase), Scope = InjectionScope.Singleton)]
    public class EntityTreeCopyServiceForTestCase : IEntityTreeCopyService
    {
        private readonly ITestCaseRepository _testCaseRepository;
        private readonly ITreeEntityStore _treeEntityStore;
        public EntityTreeCopyServiceForTestCase(ITestCaseRepository testCaseRepository, ITreeEntityStore treeEntityStore)
        {
            _testCaseRepository = testCaseRepository;
            _treeEntityStore = treeEntityStore;
        }
        public async Task<bool> Execute(string type, Guid entityID, Guid? parentTreeID)
        {
            var testCase = await _testCaseRepository.QueryByID(entityID);
            if(testCase == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.NotFoundTestCaseByID,
                    DefaultFormatting = "找不到ID为{0}的测试案例",
                    ReplaceParameters = new List<object>() { entityID.ToString() }
                };

                throw new UtilityException((int)TestPlatformErrorCodes.NotFoundTestCaseByID, fragment, 1, 0);
            }
            await using (DBTransactionScope scope = new DBTransactionScope(System.Transactions.TransactionScopeOption.Required, new System.Transactions.TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = new TimeSpan(0, 0, 30) }))
            {
                TestCase newCase = new TestCase()
                {
                    ID = Guid.NewGuid(),
                    Name = testCase.Name,
                    EngineType = testCase.EngineType,
                    MasterHostID = testCase.MasterHostID,
                    Configuration = testCase.Configuration,
                    Status = TestCaseStatus.NoRun
                };
                TreeEntity treeEntity = new TreeEntity
                {
                    ParentID = parentTreeID,
                    Value = newCase.ID.ToString(),
                    Name = newCase.Name,
                    ID = Guid.NewGuid(),
                    Type = TreeEntityValueServiceTypes.TestCase
                };
                var tree = await _treeEntityStore.QueryByName(treeEntity.ParentID, treeEntity.Name);
                if (tree != null && tree.ID != treeEntity.ID)
                {
                    return false;
                }
                else
                {
                    await treeEntity.Add();
                    newCase.TreeID = treeEntity.ID;
                    await newCase.Add();
                    scope.Complete();
                }
            }
            return true;
        }
    }
}
