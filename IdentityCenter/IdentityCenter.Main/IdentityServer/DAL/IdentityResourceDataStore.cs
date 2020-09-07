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
    [Injection(InterfaceType = typeof(IIdentityResourceDataStore), Scope = InjectionScope.Singleton)]
    public class IdentityResourceDataStore : IIdentityResourceDataStore
    {
        private readonly IDBConnectionMainFactory _dbConnectionMainFactory;


        public IdentityResourceDataStore(IDBConnectionMainFactory dbConnectionMainFactory)
        {
            _dbConnectionMainFactory = dbConnectionMainFactory;
        }

        public async Task<IList<IdentityResourceData>> QueryAllEnabled(CancellationToken cancellationToken = default)
        {
            List<IdentityResourceData> result = new List<IdentityResourceData>();
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _dbConnectionMainFactory.CreateReadForIdentityConfiguration(), async (conn, transaction) =>
            {

                await using (var dbContext = EntityDBContextFactory.CreateConfigurationDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }

                    result = await (from item in dbContext.IdentityResourceDatas
                                    where item.Enabled == true
                                    select item).ToListAsync(cancellationToken);
                }
            });

            return result;
        }

        public async Task<IList<IdentityResourceData>> QueryEnabled(IList<string> names, CancellationToken cancellationToken = default)
        {
            List<IdentityResourceData> result = new List<IdentityResourceData>();
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _dbConnectionMainFactory.CreateReadForIdentityConfiguration(), async (conn, transaction) =>
            {

                await using (var dbContext = EntityDBContextFactory.CreateConfigurationDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }

                    result = await (from item in dbContext.IdentityResourceDatas
                                    where item.Enabled == true && names.Contains(item.Name)
                                    select item).ToListAsync(cancellationToken);
                }
            });

            return result;
        }
    }
}
