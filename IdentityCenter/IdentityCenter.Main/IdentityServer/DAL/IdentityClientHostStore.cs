using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using IdentityCenter.Main.Entities.DAL;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.Transaction;
using MSLibrary.Collections;
using IdentityCenter.Main.DAL;


namespace IdentityCenter.Main.IdentityServer.DAL
{
    [Injection(InterfaceType = typeof(IIdentityClientHostStore), Scope = InjectionScope.Singleton)]
    public class IdentityClientHostStore : IIdentityClientHostStore
    {
        private readonly IDBConnectionMainFactory _dbConnectionMainFactory;

        public IdentityClientHostStore(IDBConnectionMainFactory dbConnectionMainFactory)
        {
            _dbConnectionMainFactory = dbConnectionMainFactory;
        }

        public async Task<IdentityClientHost?> QueryByName(string name, CancellationToken cancellationToken = default)
        {
            IdentityClientHost? host = null;
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _dbConnectionMainFactory.CreateReadForIdentityConfiguration(), async (conn, transaction) =>
            {

                await using (var dbContext = EntityDBContextFactory.CreateConfigurationDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }

                    host = await (from item in dbContext.IdentityClientHosts
                                               where item.Name==name
                                               select item).FirstOrDefaultAsync(cancellationToken);
                }
            });

            return host;
        }
    }
}
