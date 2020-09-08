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
    [Injection(InterfaceType = typeof(EntityTreeCopyServiceForTestDataSource), Scope = InjectionScope.Singleton)]
    public class EntityTreeCopyServiceForTestDataSource : IEntityTreeCopyService
    {
        private readonly ITestDataSourceRepository _testDataSourceRepository;
        private readonly ITreeEntityStore _treeEntityStore;
        public EntityTreeCopyServiceForTestDataSource(ITestDataSourceRepository testDataSourceRepository, ITreeEntityStore treeEntityStore)
        {
            _testDataSourceRepository = testDataSourceRepository;
            _treeEntityStore = treeEntityStore;
        }
        public async Task<bool> Execute(string type, Guid entityID, Guid? parentTreeID)
        {
            var source = await _testDataSourceRepository.QueryByID(entityID);
            if (source == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.NotFoundTestCaseDataSourceByID,
                    DefaultFormatting = "找不到ID为{0}的测试数据源",
                    ReplaceParameters = new List<object>() { entityID.ToString() }
                };
                throw new UtilityException((int)TestPlatformErrorCodes.NotFoundTestCaseDataSourceByID, fragment, 1, 0);
            }
            await using (DBTransactionScope scope = new DBTransactionScope(System.Transactions.TransactionScopeOption.Required, new System.Transactions.TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = new TimeSpan(0, 0, 30) }))
            {
                TestDataSource newSource = new TestDataSource()
                {
                    ID = Guid.NewGuid(),
                    Name = source.Name,
                    Data = source.Data,
                    Type = source.Type
                };
                TreeEntity treeEntity = new TreeEntity
                {
                    ParentID = parentTreeID,
                    Value = newSource.ID.ToString(),
                    Name = newSource.Name,
                    ID = Guid.NewGuid(),
                    Type = TreeEntityValueServiceTypes.TestDataSource
                };               
                var tree = await _treeEntityStore.QueryByName(treeEntity.ParentID, treeEntity.Name);
                if (tree != null && tree.ID != treeEntity.ID)
                {
                    return false;
                }
                else
                {
                    await treeEntity.Add();
                    newSource.TreeID = treeEntity.ID;
                    await newSource.Add();
                    scope.Complete();
                }
            }
            return true;
        }
    }
}
