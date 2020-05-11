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

namespace IdentityCenter.Main.Entities.DAL
{
    [Injection(InterfaceType = typeof(IUserThirdPartyAccountStore), Scope = InjectionScope.Singleton)]
    public class UserThirdPartyAccountStore : IUserThirdPartyAccountStore
    {
        private readonly IDBConnectionMainFactory _dbConnectionMainFactory;

        public UserThirdPartyAccountStore(IDBConnectionMainFactory dbConnectionMainFactory)
        {
            _dbConnectionMainFactory = dbConnectionMainFactory;
        }

        public async Task Add(UserThirdPartyAccount partyAccount, CancellationToken cancellationToken = default)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _dbConnectionMainFactory.CreateAllForIdentityEntity(), async (conn, transaction) =>
            {

                await using (var dbContext = EntityDBContextFactory.CreateMainDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }

                    await dbContext.UserThirdPartyAccounts.AddAsync(partyAccount, cancellationToken);

                   
                    await dbContext.SaveChangesAsync(cancellationToken);
                }
            });
        }

        public async Task Delete(Guid partyAccountID, CancellationToken cancellationToken = default)
        {
            var deleteObj = new UserThirdPartyAccount() { ID = partyAccountID };
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, _dbConnectionMainFactory.CreateAllForIdentityEntity(), async (conn, transaction) =>
            {
                await using (var dbContext = EntityDBContextFactory.CreateMainDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }

                    dbContext.UserThirdPartyAccounts.Attach(deleteObj);
                    dbContext.UserThirdPartyAccounts.Remove(deleteObj);
                    await dbContext.SaveChangesAsync(cancellationToken);
                }
            });
        }

        public async Task<UserThirdPartyAccount?> QueryByID(Guid accountID, Guid partyAccountID, CancellationToken cancellationToken = default)
        {
            UserThirdPartyAccount? account = null;
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _dbConnectionMainFactory.CreateReadForIdentityEntity(), async (conn, transaction) =>
            {

                await using (var dbContext = EntityDBContextFactory.CreateMainDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }

                    account = await (from item in dbContext.UserThirdPartyAccounts.Include((item)=>item.Account)
                                     where item.ID == partyAccountID && item.AccountID== accountID
                                     select item).FirstOrDefaultAsync(cancellationToken);
                }
            });

            return account;
        }

        public async Task<UserThirdPartyAccount?> QueryBySource(Guid accountID, string source, string sourceID, CancellationToken cancellationToken = default)
        {
            UserThirdPartyAccount? account = null;
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _dbConnectionMainFactory.CreateReadForIdentityEntity(), async (conn, transaction) =>
            {

                await using (var dbContext = EntityDBContextFactory.CreateMainDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }

                    account = await (from item in dbContext.UserThirdPartyAccounts.Include((item) => item.Account)
                                     where item.Source == source && item.ThirdPartyID== sourceID && item.AccountID == accountID
                                     select item).FirstOrDefaultAsync(cancellationToken);
                }
            });

            return account;
        }

        public async Task Update(UserThirdPartyAccount partyAccount, CancellationToken cancellationToken = default)
        {
            partyAccount.ModifyTime = DateTime.UtcNow;
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, _dbConnectionMainFactory.CreateAllForIdentityEntity(), async (conn, transaction) =>
            {
                await using (var dbContext = EntityDBContextFactory.CreateMainDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }

                    dbContext.UserThirdPartyAccounts.Attach(partyAccount);
                    var entry = dbContext.Entry(partyAccount);
                    foreach (var item in entry.Properties)
                    {
                        if (partyAccount.Attributes.ContainsKey(item.Metadata.Name))
                        {
                            if (item.Metadata.Name != nameof(partyAccount.ID)  && item.Metadata.Name != nameof(partyAccount.AccountID)
                                && item.Metadata.Name != nameof(partyAccount.CreateTime) && item.Metadata.Name != nameof(partyAccount.Source) && item.Metadata.Name != nameof(partyAccount.ThirdPartyID)
                            )
                            {
                                entry.Property(item.Metadata.Name).IsModified = true;
                            }
                        }
                    }

                    await dbContext.SaveChangesAsync(cancellationToken);
                }
            });
        }


        public async Task<Guid> QueryFirstIDNolockBySource(string source, string sourceID, CancellationToken cancellationToken = default)
        {
            Guid id = Guid.Empty;
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _dbConnectionMainFactory.CreateAllForIdentityEntity(), async (conn, transaction) =>
            {

                await using (var dbContext = EntityDBContextFactory.CreateMainDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }

                    var queryResult = await dbContext.UserAccounts.FromSqlRaw($"select top 1 [id] from UserThirdPartyAccount WITH(NOLOCK) where [source]=@source and [thirdpartyid]=@sourceid  order by [sequence]", source, sourceID).FirstOrDefaultAsync(cancellationToken);
                    if (queryResult != null)
                    {
                        id = queryResult.ID;
                    }
                }
            });

            return id;
        }
    }
}
