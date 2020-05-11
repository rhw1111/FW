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
using IdentityCenter.Main.IdentityServer.ClientBindings;

namespace IdentityCenter.Main.IdentityServer.DAL
{
    [Injection(InterfaceType = typeof(IIdentityClientOpenIDBindingStore), Scope = InjectionScope.Singleton)]
    public class IdentityClientOpenIDBindingStore : IIdentityClientOpenIDBindingStore
    {
        private readonly IDBConnectionMainFactory _dbConnectionMainFactory;

        public IdentityClientOpenIDBindingStore(IDBConnectionMainFactory dbConnectionMainFactory)
        {
            _dbConnectionMainFactory = dbConnectionMainFactory;
        }

        public async Task<IdentityClientOpenIDBinding?> QueryByName(string name, CancellationToken cancellationToken = default)
        {
            IdentityClientOpenIDBinding? binding = null;
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _dbConnectionMainFactory.CreateReadForIdentityConfiguration(), async (conn, transaction) =>
            {

                await using (var dbContext = EntityDBContextFactory.CreateConfigurationDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }

                    binding = await (from item in dbContext.IdentityClientOpenIDBindings
                                  where item.Name == name
                                  select item).FirstOrDefaultAsync(cancellationToken);
                }
            });

            return binding;
        }
    }
}
