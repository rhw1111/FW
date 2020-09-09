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
    [Injection(InterfaceType = typeof(IIdentityConsentStore), Scope = InjectionScope.Singleton)]
    public class IdentityConsentStore : IIdentityConsentStore
    {
        private readonly IDBConnectionMainFactory _dbConnectionMainFactory;

        public IdentityConsentStore(IDBConnectionMainFactory dbConnectionMainFactory)
        {
            _dbConnectionMainFactory = dbConnectionMainFactory;
        }

        public async Task Add(IdentityConsent consent, CancellationToken cancellationToken = default)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, _dbConnectionMainFactory.CreateAllForIdentityTemporary(), async (conn, transaction) =>
            {

                await using (var dbContext = EntityDBContextFactory.CreateTemporaryDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }

                    dbContext.IdentityConsents.Add(consent);
                    await dbContext.SaveChangesAsync(cancellationToken);

                }
            });

        }

        public async Task Delete(Guid id, CancellationToken cancellationToken = default)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, _dbConnectionMainFactory.CreateAllForIdentityTemporary(), async (conn, transaction) =>
            {

                await using (var dbContext = EntityDBContextFactory.CreateTemporaryDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }
                    var deleteObj = new IdentityConsent() { ID = id };
                    dbContext.IdentityConsents.Attach(deleteObj);
                    dbContext.IdentityConsents.Remove(deleteObj);
                    await dbContext.SaveChangesAsync(cancellationToken);

                }
            });
        }

        public async Task<IList<IdentityConsent>> QueryBySubject(string subjectId, CancellationToken cancellationToken = default)
        {
            List<IdentityConsent> consents = new List<IdentityConsent>();
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _dbConnectionMainFactory.CreateReadForIdentityTemporary(), async (conn, transaction) =>
            {

                await using (var dbContext = EntityDBContextFactory.CreateTemporaryDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }

                    consents =await (from item in dbContext.IdentityConsents
                                where item.SubjectId == subjectId
                                select item).ToListAsync(cancellationToken);
                }
            });

            return consents;
        }

        public async Task<IdentityConsent?> QueryBySubjectClient(string subjectId, string clientId, CancellationToken cancellationToken = default)
        {
            IdentityConsent? consent = null;
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _dbConnectionMainFactory.CreateReadForIdentityTemporary(), async (conn, transaction) =>
            {

                await using (var dbContext = EntityDBContextFactory.CreateTemporaryDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }

                    consent = await (from item in dbContext.IdentityConsents
                                      where item.SubjectId == subjectId && item.ClientId== clientId
                                     select item).FirstOrDefaultAsync(cancellationToken);
                }
            });

            return consent;
        }
    }
}
