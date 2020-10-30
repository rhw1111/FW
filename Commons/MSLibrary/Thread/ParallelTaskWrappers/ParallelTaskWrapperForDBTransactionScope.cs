using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.Transaction;

namespace MSLibrary.Thread.ParallelTaskWrappers
{
    public class ParallelTaskWrapperForDBTransactionScope : IParallelTaskWrapper
    {
        public async Task Execute(Func<Task> action)
        {
            if (DBTransactionScope.InScope())
            {
                await using (var scope = new DBTransactionScope(System.Transactions.TransactionScopeOption.Suppress, new System.Transactions.TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = new TimeSpan(0, 1, 0) }))
                {
                    await action();
                    scope.Complete();
                }
                    
            }
            else
            {
                await action();
            }
        }
    }
}
