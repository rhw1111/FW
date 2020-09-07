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
using IdentityCenter.Main.DAL;

namespace IdentityCenter.Main.IdentityServer.DAL
{
    [Injection(InterfaceType = typeof(IIdentityHostStore), Scope = InjectionScope.Singleton)]
    public class IdentityHostStore : IIdentityHostStore
    {
        private readonly IDBConnectionMainFactory _dbConnectionMainFactory;

        public IdentityHostStore(IDBConnectionMainFactory dbConnectionMainFactory)
        {
            _dbConnectionMainFactory = dbConnectionMainFactory;
        }

        public async Task<IdentityHost?> QueryByName(string name, CancellationToken cancellationToken = default)
        {
            IdentityHost? host = null;
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _dbConnectionMainFactory.CreateReadForIdentityConfiguration(), async (conn, transaction) =>
            {
                await using (var dbContext = EntityDBContextFactory.CreateConfigurationDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }
                    host = await (from item in dbContext.IdentityHosts
                                    where item.Name == name
                                    select item).FirstOrDefaultAsync();
                }
            });

            return host;
        }
    }
}
