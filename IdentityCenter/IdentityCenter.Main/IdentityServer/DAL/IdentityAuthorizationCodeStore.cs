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
    [Injection(InterfaceType = typeof(IIdentityAuthorizationCodeStore), Scope = InjectionScope.Singleton)]
    public class IdentityAuthorizationCodeStore : IIdentityAuthorizationCodeStore
    {
        private readonly IDBConnectionMainFactory _dbConnectionMainFactory;

        public IdentityAuthorizationCodeStore(IDBConnectionMainFactory dbConnectionMainFactory)
        {
            _dbConnectionMainFactory = dbConnectionMainFactory;
        }

        public async Task Add(IdentityAuthorizationCode authorizationCode, CancellationToken cancellationToken = default)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _dbConnectionMainFactory.CreateAllForIdentityTemporary(), async (conn, transaction) =>
            {

                await using (var dbContext = EntityDBContextFactory.CreateTemporaryDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }

                    await dbContext.IdentityAuthorizationCodes.AddAsync(authorizationCode, cancellationToken);

                    await dbContext.SaveChangesAsync(cancellationToken);
                }
            });
        }

        public async Task Delete(Guid id, CancellationToken cancellationToken = default)
        {
            var deleteObj = new IdentityAuthorizationCode() { ID = id };
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, _dbConnectionMainFactory.CreateAllForIdentityTemporary(), async (conn, transaction) =>
            {
                await using (var dbContext = EntityDBContextFactory.CreateTemporaryDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }

                    dbContext.IdentityAuthorizationCodes.Attach(deleteObj);
                    dbContext.IdentityAuthorizationCodes.Remove(deleteObj);
                    await dbContext.SaveChangesAsync(cancellationToken);
                }
            });
        }

        public async Task<IdentityAuthorizationCode?> QueryByCode(string code, CancellationToken cancellationToken = default)
        {
            IdentityAuthorizationCode? authorizationCode = null;
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _dbConnectionMainFactory.CreateReadForIdentityTemporary(), async (conn, transaction) =>
            {

                await using (var dbContext = EntityDBContextFactory.CreateTemporaryDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }

                    authorizationCode = await (from item in dbContext.IdentityAuthorizationCodes
                                     where item.Code == code
                                     select item).FirstOrDefaultAsync(cancellationToken);
                }
            });

            return authorizationCode;
        }
    }
}
