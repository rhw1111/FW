using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using MSLibrary;
using MSLibrary.CommandLine.SSH;
using MSLibrary.CommandLine.SSH.DAL;
using MSLibrary.DI;
using Microsoft.EntityFrameworkCore;
using MSLibrary.Transaction;
using FW.TestPlatform.Main.DAL;

namespace FW.TestPlatform.Main.SSH.DAL
{

    [Injection(InterfaceType = typeof(ISSHEndpointStore), Scope = InjectionScope.Singleton)]
    public class SSHEndpointStore : ISSHEndpointStore
    {

        private readonly ICommandLineConnectionFactory _commandLineConnectionFactory;
        private readonly IMainDBContextFactory _mainDBContextFactory;

        public SSHEndpointStore(ICommandLineConnectionFactory commandLineConnectionFactory, IMainDBContextFactory mainDBContextFactory)
        {
            _commandLineConnectionFactory = commandLineConnectionFactory;
            _mainDBContextFactory = mainDBContextFactory;
        }

        public async Task<SSHEndpoint?> QueryByName(string name, CancellationToken cancellationToken = default)
        {
            SSHEndpoint? endpoint = null;
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, true, false, _commandLineConnectionFactory.CreateReadForCommandLine(), async (conn, transaction) =>
            {

                await using (var dbContext = _mainDBContextFactory.CreateMainDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }



                    endpoint = await (from item in dbContext.SSHEndpoints
                                      where item.Name == name
                                      select item).FirstOrDefaultAsync(cancellationToken);
                }
            });

            return endpoint;
        }

        public async Task Add(SSHEndpoint source, CancellationToken cancellationToken = default)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, false, false, _commandLineConnectionFactory.CreateAllForCommandLine(), async (conn, transaction) =>
            {
                await using (var dbContext = _mainDBContextFactory.CreateMainDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }
                    if (source.ID == Guid.Empty)
                    {
                        source.ID = Guid.NewGuid();
                    }
                    source.CreateTime = DateTime.UtcNow;
                    source.ModifyTime = DateTime.UtcNow;
                    var entity = await dbContext.SSHEndpoints.AddAsync(source, cancellationToken);
                    var result = await dbContext.SaveChangesAsync(cancellationToken);
                }
            });

        }

        public async Task Delete(Guid id, CancellationToken cancellationToken = default)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, false, false, _commandLineConnectionFactory.CreateAllForCommandLine(), async (conn, transaction) =>
            {
                await using (var dbContext = _mainDBContextFactory.CreateMainDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }
                    var deleteObj = new SSHEndpoint() { ID = id };
                    dbContext.SSHEndpoints.Attach(deleteObj);
                    dbContext.SSHEndpoints.Remove(deleteObj);
                    await dbContext.SaveChangesAsync(cancellationToken);
                }
            });
        }

        public async Task DeleteMutiple(List<Guid> ids, CancellationToken cancellationToken = default)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, false, false, _commandLineConnectionFactory.CreateAllForCommandLine(), async (conn, transaction) =>
            {
                await using (var dbContext = _mainDBContextFactory.CreateMainDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }
                    List<SSHEndpoint> list = new List<SSHEndpoint>();
                    foreach (Guid id in ids)
                    {
                        var deleteObj = new SSHEndpoint() { ID = id };
                        list.Add(deleteObj);
                    }
                    dbContext.SSHEndpoints.AttachRange(list.ToArray());
                    dbContext.SSHEndpoints.RemoveRange(list.ToArray());
                    await dbContext.SaveChangesAsync(cancellationToken);
                }
            });
        }

        public async Task<SSHEndpoint?> QueryByID(Guid id, CancellationToken cancellationToken = default)
        {
            SSHEndpoint? result = null;
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, true, false, _commandLineConnectionFactory.CreateReadForCommandLine(), async (conn, transaction) =>
            {
                await using (var dbContext = _mainDBContextFactory.CreateMainDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }
                    result = await (from item in dbContext.SSHEndpoints
                                    where item.ID == id
                                    select item).FirstOrDefaultAsync();
                }
            });

            return result;
        }

        //public async Task<SSHEndpoint?> QueryByName(string name, CancellationToken cancellationToken = default)
        //{
        //    SSHEndpoint? result = null;
        //    await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, true, false, _commandLineConnectionFactory.CreateReadForCommandLine(), async (conn, transaction) =>
        //    {
        //        await using (var dbContext = _mainDBContextFactory.CreateMainDBContext(conn))
        //        {
        //            if (transaction != null)
        //            {
        //                await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
        //            }


        //            result = await (from item in dbContext.SSHEndpoints
        //                            where item.Name == name
        //                            select item).FirstOrDefaultAsync();
        //        }
        //    });

        //    return result;
        //}

        public async Task<Guid?> QueryByNameNoLock(string name, CancellationToken cancellationToken = default)
        {
            Guid? result = null;
            await using (DBTransactionScope scope = new DBTransactionScope(System.Transactions.TransactionScopeOption.RequiresNew, new System.Transactions.TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted, Timeout = new TimeSpan(0, 0, 30) }))
            {

                await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, true, false, _commandLineConnectionFactory.CreateReadForCommandLine(), async (conn, transaction) =>
                {
                    await using (var dbContext = _mainDBContextFactory.CreateMainDBContext(conn))
                    {
                        if (transaction != null)
                        {
                            await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                        }

                        var sshEndpoint = await (from item in dbContext.SSHEndpoints
                                              where item.Name == name
                                              orderby EF.Property<long>(item, "Sequence") descending
                                              select item).FirstOrDefaultAsync();
                        if (sshEndpoint != null)
                            result = sshEndpoint.ID;
                    }
                });

                scope.Complete();
            }
            return result;
        }

        public async Task<QueryResult<SSHEndpoint>> QueryByPage(string matchName, int page, int pageSize, CancellationToken cancellationToken = default)
        {
            QueryResult<SSHEndpoint> result = new QueryResult<SSHEndpoint>()
            {
                CurrentPage = page
            };

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, true, false, _commandLineConnectionFactory.CreateReadForCommandLine(), async (conn, transaction) =>
            {
                await using (var dbContext = _mainDBContextFactory.CreateMainDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }

                    var strLike = $"%{matchName.ToSqlLike()}%";
                    var count = await (from item in dbContext.SSHEndpoints
                                       where EF.Functions.Like(item.Name, strLike)
                                       select item.ID).CountAsync();

                    result.TotalCount = count;

                    var ids = (from item in dbContext.SSHEndpoints
                               where EF.Functions.Like(item.Name, strLike)
                               orderby item.CreateTime descending
                               select item.ID
                                        ).Skip((page - 1) * pageSize).Take(pageSize);

                    var datas = await (from item in dbContext.SSHEndpoints
                                       join idItem in ids
                                  on item.ID equals idItem
                                       orderby EF.Property<long>(item, "Sequence") descending
                                       select item).ToListAsync();

                    result.Results.AddRange(datas);
                }
            });

            return result;
        }

        public async Task Update(SSHEndpoint source, CancellationToken cancellationToken = default)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, false, false, _commandLineConnectionFactory.CreateAllForCommandLine(), async (conn, transaction) =>
            {
                await using (var dbContext = _mainDBContextFactory.CreateMainDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }
                    source.ModifyTime = DateTime.UtcNow;
                    dbContext.SSHEndpoints.Attach(source);

                    var entry = dbContext.Entry(source);
                    foreach (var item in entry.Properties)
                    {
                        if (item.Metadata.Name != "ID")
                            entry.Property(item.Metadata.Name).IsModified = true;
                    }
                    await dbContext.SaveChangesAsync(cancellationToken);
                }
            });
        }
    }
}
