using MSLibrary;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MSLibrary.DI;
using MSLibrary.Transaction;
using MSLibrary.Collections;
using FW.TestPlatform.Main.DAL;
using Microsoft.AspNetCore.Http.Features.Authentication;
using FW.TestPlatform.Main.DTOModel;

namespace FW.TestPlatform.Main.Entities.DAL
{
    [Injection(InterfaceType = typeof(ITestCaseSlaveHostStore), Scope = InjectionScope.Singleton)]
    public class TestCaseSlaveHostStore : ITestCaseSlaveHostStore
    {
        private readonly IMainDBConnectionFactory _mainDBConnectionFactory;
        private readonly IMainDBContextFactory _mainDBContextFactory;

        public TestCaseSlaveHostStore(IMainDBConnectionFactory mainDBConnectionFactory, IMainDBContextFactory mainDBContextFactory)
        {
            _mainDBConnectionFactory = mainDBConnectionFactory;
            _mainDBContextFactory = mainDBContextFactory;
        }

        public async Task Add(TestCaseSlaveHost source, CancellationToken cancellationToken = default)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, false, false, _mainDBConnectionFactory.CreateAllForMain(), async (conn, transaction) =>
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

                    await dbContext.TestCaseSlaveHosts.AddAsync(source, cancellationToken);

                    await dbContext.SaveChangesAsync(cancellationToken);
                }
            });

        }

        public async Task Delete(Guid id, CancellationToken cancellationToken = default)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, false, false, _mainDBConnectionFactory.CreateAllForMain(), async (conn, transaction) =>
            {
                await using (var dbContext = _mainDBContextFactory.CreateMainDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }

                    var deleteObj = new TestCaseSlaveHost() { ID = id };
                    dbContext.TestCaseSlaveHosts.Attach(deleteObj);
                    dbContext.TestCaseSlaveHosts.Remove(deleteObj);

                    await dbContext.SaveChangesAsync(cancellationToken);
                }
            });
        }

        public async Task Update(TestCaseSlaveHost source, CancellationToken cancellationToken = default)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, false, false, _mainDBConnectionFactory.CreateAllForMain(), async (conn, transaction) =>
            {
                await using (var dbContext = _mainDBContextFactory.CreateMainDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }

                    source.ModifyTime = DateTime.UtcNow;
                    dbContext.TestCaseSlaveHosts.Attach(source);

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

        public async Task<TestCaseSlaveHost?> QueryByCase(Guid caseId, Guid slaveId, CancellationToken cancellationToken = default)
        {
            TestCaseSlaveHost? result = null;
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, true, false, _mainDBConnectionFactory.CreateReadForMain(), async (conn, transaction) =>
            {
                await using (var dbContext = _mainDBContextFactory.CreateMainDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }


                    result = await (from item in dbContext.TestCaseSlaveHosts
                                    where item.ID == slaveId && item.TestCaseID == caseId
                                    select item).Include(entity => entity.Host).ThenInclude(entity => entity.SSHEndpoint).FirstOrDefaultAsync();
                }
            });

            return result;
        }

        public IAsyncEnumerable<TestCaseSlaveHost> QueryByCase(Guid caseId, CancellationToken cancellationToken = default)
        {

            AsyncInteration<TestCaseSlaveHost> interation = new AsyncInteration<TestCaseSlaveHost>(
                async (index) =>
                {
                    List<TestCaseSlaveHost>? datas = null;
                    await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, true, false, _mainDBConnectionFactory.CreateReadForMain(), async (conn, transaction) =>
                    {
                        await using (var dbContext = _mainDBContextFactory.CreateMainDBContext(conn))
                        {
                            if (transaction != null)
                            {
                                await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                            }

                            var ids = (from item in dbContext.TestCaseSlaveHosts
                                       where item.TestCaseID == caseId
                                       orderby EF.Property<long>(item, "Sequence")
                                       select item.ID
                                                ).Skip((index) * 500).Take(500);

                            datas = await (from item in dbContext.TestCaseSlaveHosts
                                           join idItem in ids
                                      on item.ID equals idItem
                                           orderby EF.Property<long>(item, "Sequence")
                                           select item).Include(u => u.Host).ThenInclude(u => u.SSHEndpoint).ToListAsync();
                        }
                    });

                    return datas;
                }
                );
            return interation;
        }

        public async Task<TestCaseSlaveHost?> QueryByID(Guid id, CancellationToken cancellationToken)
        {
            TestCaseSlaveHost? result = null;
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, true, false, _mainDBConnectionFactory.CreateReadForMain(), async (conn, transaction) =>
            {
                await using (var dbContext = _mainDBContextFactory.CreateMainDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }
                    result = await (from item in dbContext.TestCaseSlaveHosts
                                    where item.ID == id
                                    select item).Include(entity => entity.TestCase).Include(entity => entity.Host).FirstOrDefaultAsync();
                }
            });

            return result;
        }
        public async Task<Guid?> QueryByNameNoLock(string name, CancellationToken cancellationToken = default)
        {
            Guid? result = null;
            await using (DBTransactionScope scope = new DBTransactionScope(System.Transactions.TransactionScopeOption.RequiresNew, new System.Transactions.TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted, Timeout = new TimeSpan(0, 0, 30) }))
            {

                await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, true, false, _mainDBConnectionFactory.CreateReadForMain(), async (conn, transaction) =>
                {
                    await using (var dbContext = _mainDBContextFactory.CreateMainDBContext(conn))
                    {
                        if (transaction != null)
                        {
                            await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                        }

                        var dataSourceItem = await (from item in dbContext.TestCaseSlaveHosts
                                                    where item.SlaveName == name
                                                    orderby EF.Property<long>(item, "Sequence")
                                                    select item).FirstOrDefaultAsync();
                        if (dataSourceItem != null)
                            result = dataSourceItem.ID;
                    }
                });

                scope.Complete();
            }
            return result;
        }

        public async Task DeleteSlaveHosts(List<Guid> ids, CancellationToken cancellationToken = default)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, false, false, _mainDBConnectionFactory.CreateAllForMain(), async (conn, transaction) =>
            {
                await using (var dbContext = _mainDBContextFactory.CreateMainDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }
                    List<TestCaseSlaveHost> list = new List<TestCaseSlaveHost>();
                    foreach (Guid id in ids)
                    {
                        var deleteObj = new TestCaseSlaveHost() { ID = id };
                        list.Add(deleteObj);
                    }
                    dbContext.TestCaseSlaveHosts.AttachRange(list.ToArray());
                    dbContext.TestCaseSlaveHosts.RemoveRange(list.ToArray());
                    await dbContext.SaveChangesAsync(cancellationToken);
                }
            });
        }

        public async Task<List<TestCaseSlaveHost>> QueryByCaseIdAndSlaveHostIds(Guid caseId, List<Guid> ids, CancellationToken cancellationToken = default)
        {
            List<TestCaseSlaveHost> result = new List<TestCaseSlaveHost>();
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, true, false, _mainDBConnectionFactory.CreateReadForMain(), async (conn, transaction) =>
            {
                await using (var dbContext = _mainDBContextFactory.CreateMainDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }

                    result = await (from item in dbContext.TestCaseSlaveHosts
                                   where item.TestCaseID == caseId && ids.Contains(item.ID)
                                   select item).ToListAsync();
                }
            });

            return result;
        }
    }
}
