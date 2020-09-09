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
    [Injection(InterfaceType = typeof(IApiScopeDataStore), Scope = InjectionScope.Singleton)]
    public class ApiScopeDataStore : IApiScopeDataStore
    {
        private readonly IDBConnectionMainFactory _dbConnectionMainFactory;


        public ApiScopeDataStore(IDBConnectionMainFactory dbConnectionMainFactory)
        {
            _dbConnectionMainFactory = dbConnectionMainFactory;
        }
        public async Task<IList<ApiScopeData>> QueryAllEnabled(CancellationToken cancellationToken = default)
        {
            List<ApiScopeData> result=new List<ApiScopeData>();
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _dbConnectionMainFactory.CreateReadForIdentityConfiguration(), async (conn, transaction) =>
            {

                await using (var dbContext = EntityDBContextFactory.CreateConfigurationDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }

                    result = await (from item in dbContext.ApiScopeDatas
                                    where item.Enabled == true
                                    select item).ToListAsync(cancellationToken);
                }
            });

            return result;
        }

        public async Task<IList<ApiScopeData>> QueryEnabled(IList<string> names, CancellationToken cancellationToken = default)
        {
            List<ApiScopeData> result = new List<ApiScopeData>();
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _dbConnectionMainFactory.CreateReadForIdentityConfiguration(), async (conn, transaction) =>
            {

                await using (var dbContext = EntityDBContextFactory.CreateConfigurationDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }

                    result = await (from item in dbContext.ApiScopeDatas
                                    where names.Contains(item.Name) && item.Enabled == true
                                    select item).ToListAsync(cancellationToken);
                }
            });

            return result;
        }

        public async Task<ApiScopeData?> QueryEnabled(string name, CancellationToken cancellationToken = default)
        {
            ApiScopeData? result = null;
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _dbConnectionMainFactory.CreateReadForIdentityConfiguration(), async (conn, transaction) =>
            {

                await using (var dbContext = EntityDBContextFactory.CreateConfigurationDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }

                    result = await (from item in dbContext.ApiScopeDatas
                                    where item.Name==name && item.Enabled == true
                                    select item).FirstOrDefaultAsync(cancellationToken);
                }
            });

            return result;
        }
    }
}
