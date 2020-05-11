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
    [Injection(InterfaceType = typeof(IUserAccountStore), Scope = InjectionScope.Singleton)]
    public class UserAccountStore : IUserAccountStore
    {
        private readonly IDBConnectionMainFactory _dbConnectionMainFactory;

        public UserAccountStore(IDBConnectionMainFactory dbConnectionMainFactory)
        {
            _dbConnectionMainFactory = dbConnectionMainFactory;
        }

        public async Task Add(UserAccount account, CancellationToken cancellationToken = default)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _dbConnectionMainFactory.CreateAllForIdentityEntity(), async (conn, transaction) =>
            {

                await using (var dbContext = EntityDBContextFactory.CreateMainDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }

                    await dbContext.UserAccounts.AddAsync(account, cancellationToken);

                    await dbContext.SaveChangesAsync(cancellationToken);
                }
            });
        }

        public async Task Delete(Guid id, CancellationToken cancellationToken = default)
        {
            var deleteObj = new UserAccount() { ID = id };
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, _dbConnectionMainFactory.CreateAllForIdentityEntity(), async (conn, transaction) =>
            {
                await using (var dbContext = EntityDBContextFactory.CreateMainDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }

                    dbContext.UserAccounts.Attach(deleteObj);
                    dbContext.UserAccounts.Remove(deleteObj);
                    await dbContext.SaveChangesAsync(cancellationToken);
                }
            });
        }

        public async Task<UserAccount?> QueryByID(Guid id, CancellationToken cancellationToken = default)
        {
            UserAccount? account = null;
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _dbConnectionMainFactory.CreateReadForIdentityEntity(), async (conn, transaction) =>
            {

                await using (var dbContext = EntityDBContextFactory.CreateMainDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }

                    account = await (from item in dbContext.UserAccounts
                                  where item.ID == id
                                  select item).FirstOrDefaultAsync(cancellationToken);
                }
            });

            return account;
        }

        public async Task<UserAccount?> QueryByName(string name, CancellationToken cancellationToken = default)
        {
            UserAccount? account = null;
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _dbConnectionMainFactory.CreateReadForIdentityEntity(), async (conn, transaction) =>
            {

                await using (var dbContext = EntityDBContextFactory.CreateMainDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }

                    account = await (from item in dbContext.UserAccounts
                                     where item.Name == name
                                     select item).FirstOrDefaultAsync(cancellationToken);
                }
            });

            return account;
        }

        public async Task<UserAccount?> QueryByThirdParty(string source, string sourceID, CancellationToken cancellationToken = default)
        {
            UserAccount? account = null;
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _dbConnectionMainFactory.CreateReadForIdentityEntity(), async (conn, transaction) =>
            {

                await using (var dbContext = EntityDBContextFactory.CreateMainDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }

                    account = await (from item in dbContext.UserAccounts
                                     join thirdPart in dbContext.UserThirdPartyAccounts
                                     on item.ID equals thirdPart.AccountID
                                     where thirdPart.Source==source && thirdPart.ThirdPartyID== sourceID
                                     select item).FirstOrDefaultAsync(cancellationToken);
                }
            });

            return account;
        }

        public async Task<Guid> QueryFirstIDNolockByName(string name, CancellationToken cancellationToken = default)
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


                    var queryResult = await dbContext.UserAccounts.FromSqlRaw($"select top 1 [id] from UserAccount WITH(NOLOCK) where [name]=@name  order by [sequence]", name).FirstOrDefaultAsync(cancellationToken);
                    if (queryResult != null)
                    {
                        id = queryResult.ID;
                    }
                }
            });

            return id;
        }

        public async Task Update(UserAccount account, CancellationToken cancellationToken = default)
        {
            account.ModifyTime = DateTime.UtcNow;
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, _dbConnectionMainFactory.CreateAllForIdentityEntity(), async (conn, transaction) =>
            {
                await using (var dbContext = EntityDBContextFactory.CreateMainDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }

                    dbContext.UserAccounts.Attach(account);
                    var entry = dbContext.Entry(account);
                    foreach (var item in entry.Properties)
                    {
                        if (account.Attributes.ContainsKey(item.Metadata.Name))
                        {
                            if (item.Metadata.Name != nameof(account.ID) && item.Metadata.Name != nameof(account.Password)
                                && item.Metadata.Name != nameof(account.CreateTime) && item.Metadata.Name != nameof(account.Active)
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

        public async Task UpdateActive(Guid id, bool active, CancellationToken cancellationToken = default)
        {
            var modifyObj = new UserAccount() { ID = id, Active= active, ModifyTime = DateTime.UtcNow };
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, _dbConnectionMainFactory.CreateAllForIdentityEntity(), async (conn, transaction) =>
            {
                await using (var dbContext = EntityDBContextFactory.CreateMainDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }

                    dbContext.UserAccounts.Attach(modifyObj);
                    var entry = dbContext.Entry(modifyObj);
                    foreach (var item in entry.Properties)
                    {
                        if ( item.Metadata.Name == nameof(modifyObj.Active) && item.Metadata.Name == nameof(modifyObj.ModifyTime)
                        )
                        {
                            entry.Property(item.Metadata.Name).IsModified = true;
                        }
                    }

                    await dbContext.SaveChangesAsync(cancellationToken);
                }
            });
        }

        public async Task UpdatePassword(Guid id, string password, CancellationToken cancellationToken = default)
        {
            var modifyObj = new UserAccount() { ID = id, Password = password, ModifyTime = DateTime.UtcNow };
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, _dbConnectionMainFactory.CreateAllForIdentityEntity(), async (conn, transaction) =>
            {
                await using (var dbContext = EntityDBContextFactory.CreateMainDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }

                    dbContext.UserAccounts.Attach(modifyObj);
                    var entry = dbContext.Entry(modifyObj);
                    foreach (var item in entry.Properties)
                    {
                        if (item.Metadata.Name == nameof(modifyObj.Password) && item.Metadata.Name == nameof(modifyObj.ModifyTime)
                        )
                        {
                            entry.Property(item.Metadata.Name).IsModified = true;
                        }
                    }

                    await dbContext.SaveChangesAsync(cancellationToken);
                }
            });
        }
    }
}
