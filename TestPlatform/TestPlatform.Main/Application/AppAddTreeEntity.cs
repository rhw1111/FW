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
    [Injection(InterfaceType = typeof(IAppAddTreeEntity), Scope = InjectionScope.Singleton)]
    public class AppAddTreeEntity : IAppAddTreeEntity
    {
        private readonly ITreeEntityStore _treeEntityStore;
        public AppAddTreeEntity(ITreeEntityStore treeEntityStore)
        {
            _treeEntityStore = treeEntityStore;
        }
        public async Task<TreeEntityViewModel> Do(TreeEntityAddModel model, CancellationToken cancellationToken = default)
        {
            TreeEntityViewModel result;
            await using (DBTransactionScope scope = new DBTransactionScope(System.Transactions.TransactionScopeOption.Required, new System.Transactions.TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = new TimeSpan(0, 0, 30) }))
            {
                TreeEntity treeEntity = new TreeEntity
                {
                    ParentID = model.ParentID,
                    Value = null,
                    Name = "C-" + model.Name,
                    ID = Guid.NewGuid(),
                    Type = 0
                };
                await treeEntity.Add(cancellationToken);
                scope.Complete();
                result = new TreeEntityViewModel()
                {
                    ID = treeEntity.ID,
                    Name = treeEntity.Name,
                    CreateTime = treeEntity.CreateTime.ToCurrentUserTimeZone()
                };
            }
            return result;
        }
    }
}
