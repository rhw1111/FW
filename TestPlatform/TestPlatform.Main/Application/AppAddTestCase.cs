using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.DI;
using FW.TestPlatform.Main.DTOModel;
using FW.TestPlatform.Main.Entities;
using System.Linq;
using System.Diagnostics.Tracing;
using MSLibrary.Transaction;
using MSLibrary.Collections;
using MSLibrary.Collections.DAL;

namespace FW.TestPlatform.Main.Application
{
    [Injection(InterfaceType = typeof(IAppAddTestCase), Scope = InjectionScope.Singleton)]
    public class AppAddTestCase : IAppAddTestCase
    {
        private readonly ITreeEntityStore _treeEntityStore;
        public AppAddTestCase(ITreeEntityStore treeEntityStore)
        {
            _treeEntityStore = treeEntityStore;
        }
        public async Task<TestCaseViewData> Do(TestCaseAddModel model, CancellationToken cancellationToken = default)
        {
            TestCaseViewData result;
            await using (DBTransactionScope scope = new DBTransactionScope(System.Transactions.TransactionScopeOption.Required, new System.Transactions.TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = new TimeSpan(0, 0, 30) }))
            {
                TestCase source = new TestCase()
                {
                    ID = Guid.NewGuid(),
                    Name = model.Name,
                    EngineType = model.EngineType,
                    MasterHostID = model.MasterHostID,
                    Configuration = model.Configuration,
                    Status = TestCaseStatus.NoRun
                };
                TreeEntity treeEntity = new TreeEntity
                {
                    ParentID = model.FolderID,
                    Value = source.ID.ToString(),
                    Name = "C-" + source.Name,
                    ID = Guid.NewGuid(),
                    Type = 1
                };
                await treeEntity.Add(cancellationToken);
                source.TreeID = treeEntity.ID;
                await source.Add(cancellationToken);
                scope.Complete();
                result = new TestCaseViewData()
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
            return result;
        }
    }
}
