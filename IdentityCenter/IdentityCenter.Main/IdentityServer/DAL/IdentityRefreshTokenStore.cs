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
    [Injection(InterfaceType = typeof(IIdentityRefreshTokenStore), Scope = InjectionScope.Singleton)]
    public class IdentityRefreshTokenStore : IIdentityRefreshTokenStore
    {
        private readonly IDBConnectionMainFactory _dbConnectionMainFactory;

        public IdentityRefreshTokenStore(IDBConnectionMainFactory dbConnectionMainFactory)
        {
            _dbConnectionMainFactory = dbConnectionMainFactory;
        }

        public async Task Add(IdentityRefreshToken token, CancellationToken cancellationToken = default)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, _dbConnectionMainFactory.CreateAllForIdentityTemporary(), async (conn, transaction) =>
            {
                await using (var dbContext = EntityDBContextFactory.CreateTemporaryDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }
                    dbContext.IdentityRefreshTokens.Add(token);
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

                    IdentityRefreshToken deleteToken = new IdentityRefreshToken() { ID =id};
                    dbContext.IdentityRefreshTokens.Remove(deleteToken);
                    await dbContext.SaveChangesAsync(cancellationToken);
                }
            });
        }

        public async Task Delete(IList<Guid> idList, CancellationToken cancellationToken = default)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, _dbConnectionMainFactory.CreateAllForIdentityTemporary(), async (conn, transaction) =>
            {
                await using (var dbContext = EntityDBContextFactory.CreateTemporaryDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }

                    List<IdentityRefreshToken> deleteTokens = new List<IdentityRefreshToken>();
                    foreach(var item in idList)
                    {
                        var deleteToken = new IdentityRefreshToken() { ID = item };
                        dbContext.IdentityRefreshTokens.Attach(deleteToken);
                        deleteTokens.Add(deleteToken);
                    }

                    dbContext.IdentityRefreshTokens.RemoveRange(deleteTokens);
                    
                    await dbContext.SaveChangesAsync(cancellationToken);
                }
            });
        }

        public async Task<IdentityRefreshToken?> QueryByHandle(string handle, CancellationToken cancellationToken = default)
        {
            IdentityRefreshToken? token = null;
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _dbConnectionMainFactory.CreateAllForIdentityTemporary(), async (conn, transaction) =>
            {
                await using (var dbContext = EntityDBContextFactory.CreateTemporaryDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }
                    token = await (from item in dbContext.IdentityRefreshTokens
                                   where item.Handle == handle
                                   select item).FirstOrDefaultAsync(cancellationToken);

                }
            });

            return token;
        }

        public async Task<IList<IdentityRefreshToken>> QueryBySubjectClient(string subjecId, string clientId, int top, CancellationToken cancellationToken = default)
        {
            List<IdentityRefreshToken> tokens = new List<IdentityRefreshToken>();
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _dbConnectionMainFactory.CreateAllForIdentityTemporary(), async (conn, transaction) =>
            {
                await using (var dbContext = EntityDBContextFactory.CreateTemporaryDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }
                    tokens = await (from item in dbContext.IdentityRefreshTokens
                                   where item.SubjectId == subjecId && item.ClientId== clientId
                                    select item).Take(top).ToListAsync(cancellationToken);

                }
            });

            return tokens;
        }

        public async Task Update(IdentityRefreshToken token, CancellationToken cancellationToken = default)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, _dbConnectionMainFactory.CreateAllForIdentityTemporary(), async (conn, transaction) =>
            {
                await using (var dbContext = EntityDBContextFactory.CreateTemporaryDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }

                    dbContext.IdentityRefreshTokens.Attach(token);

                    var entry = dbContext.Entry(token);
                
                    foreach (var item in entry.Properties)
                    {
                        if (item.Metadata.Name != "ID")
                        {
                            if (token.Attributes.ContainsKey(item.Metadata.Name))
                            {
                                entry.Property(item.Metadata.Name).IsModified = true;
                            }
                        }
                    }
                    await dbContext.SaveChangesAsync(cancellationToken);
                }
            });

        }
    }
}
