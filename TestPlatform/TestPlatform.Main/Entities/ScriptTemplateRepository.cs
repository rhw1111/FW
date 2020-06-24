using MSLibrary;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary.DI;
using FW.TestPlatform.Main.Entities.DAL;
using MSLibrary.Transaction;

namespace FW.TestPlatform.Main.Entities
{
    [Injection(InterfaceType = typeof(IScriptTemplateRepository), Scope = InjectionScope.Singleton)]
    public class ScriptTemplateRepository : IScriptTemplateRepository
    {
        private readonly IScriptTemplateStore _scriptTemplateStore;

        public ScriptTemplateRepository(IScriptTemplateStore scriptTemplateStore)
        {
            _scriptTemplateStore = scriptTemplateStore;
        }

        public async Task Add(ScriptTemplate template, CancellationToken cancellationToken = default)
        {
            await using (DBTransactionScope scope = new DBTransactionScope(System.Transactions.TransactionScopeOption.Required, new System.Transactions.TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = new TimeSpan(0, 0, 30) }))
            {
                await _scriptTemplateStore.Add(template, cancellationToken);
                //检查是否有名称重复的

                scope.Complete();
            }
        }

        public async Task Delete(ScriptTemplate template, CancellationToken cancellationToken = default)
        {
            await _scriptTemplateStore.Delete(template.ID, cancellationToken);
        }

        public async Task Update(ScriptTemplate template, CancellationToken cancellationToken = default)
        {
            ScriptTemplate scriptTemplate = await _scriptTemplateStore.QueryByID(template.ID, cancellationToken);

            if (scriptTemplate != null)
            {
                await using (DBTransactionScope scope = new DBTransactionScope(System.Transactions.TransactionScopeOption.Required, new System.Transactions.TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = new TimeSpan(0, 0, 30) }))
                {
                    await _scriptTemplateStore.Update(scriptTemplate, cancellationToken);
                    //检查是否有名称重复的

                    scope.Complete();
                }
            }
        }

        public async Task<ScriptTemplate?> QueryByID(Guid id, CancellationToken cancellationToken = default)
        {
            return await _scriptTemplateStore.QueryByID(id, cancellationToken);
        }

        public async Task<ScriptTemplate?> QueryByName(string name, CancellationToken cancellationToken = default)
        {
            return await _scriptTemplateStore.QueryByName(name, cancellationToken);
        }
    }
}
