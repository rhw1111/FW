using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.DI;
using FW.TestPlatform.Main.DTOModel;
using FW.TestPlatform.Main.Entities;
using MSLibrary.Collections.DAL;
using MSLibrary.Transaction;
using MSLibrary.Collections;

namespace FW.TestPlatform.Main.Application
{
    [Injection(InterfaceType = typeof(IAppAddTestDataSource), Scope = InjectionScope.Singleton)]
    public class AppAddTestDataSource : IAppAddTestDataSource
    {
        private readonly ITreeEntityStore _treeEntityStore;
        public AppAddTestDataSource(ITreeEntityStore treeEntityStore)
        {
            _treeEntityStore = treeEntityStore;
        }
        public async Task<TestDataSourceViewData> Do(TestDataSourceAddModel model, CancellationToken cancellationToken = default)
        {
            TestDataSourceViewData result = new TestDataSourceViewData();
            await using (DBTransactionScope scope = new DBTransactionScope(System.Transactions.TransactionScopeOption.Required, new System.Transactions.TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = new TimeSpan(0, 0, 30) }))
            {
                TestDataSource source = new TestDataSource()
                {
                    ID = Guid.NewGuid(),
                    Name = model.Name,
                    Data = model.Data,
                    Type = model.Type
                };
                TreeEntity treeEntity = new TreeEntity
                {
                    ParentID = model.FolderID,
                    Value = source.ID.ToString(),
                    Name = "DS-" + source.Name,
                    ID = Guid.NewGuid(),
                    Type = TreeEntityValueServiceTypes.TestDataSource
                };
                await treeEntity.Add(cancellationToken);
                source.TreeID = treeEntity.ID;
                await source.Add(cancellationToken);

                result = new TestDataSourceViewData()
                {
                    ID = source.ID,
                    Type = source.Type,
                    Data = source.Data,
                    Name = source.Name,
                    CreateTime = source.CreateTime.ToCurrentUserTimeZone(),
                    ModifyTime = source.ModifyTime.ToCurrentUserTimeZone()
                }; 
            }
            return result;
        }
    }
}
