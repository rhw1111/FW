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
using FW.TestPlatform.Main.DAL;
using Microsoft.AspNetCore.Http.Features.Authentication;
using MSLibrary.Collections;
using MongoDB.Driver;

namespace FW.TestPlatform.Main.Entities.DAL
{
    [Injection(InterfaceType = typeof(ITestHostStore), Scope = InjectionScope.Singleton)]
    public class TestHostStore : ITestHostStore
    {
        private readonly IMainDBConnectionFactory _mainDBConnectionFactory;
        private readonly IMainDBContextFactory _mainDBContextFactory;

        public TestHostStore(IMainDBConnectionFactory mainDBConnectionFactory, IMainDBContextFactory mainDBContextFactory)
        {
            _mainDBConnectionFactory = mainDBConnectionFactory;
            _mainDBContextFactory = mainDBContextFactory;
        }

        public async Task Add(TestHost source, CancellationToken cancellationToken = default)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, false, false, _mainDBConnectionFactory.CreateAllForMain(), async (conn, transaction) =>
            {
                await using (var dbContext = _mainDBContextFactory.CreateMainDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }

                    if (source.ID==Guid.Empty)
                    {
                        source.ID = Guid.NewGuid();
                    }

                    source.CreateTime = DateTime.UtcNow;
                    source.ModifyTime= DateTime.UtcNow;

                    await dbContext.TestHosts.AddAsync(source, cancellationToken);

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

                    var deleteObj = new TestHost() { ID = id };
                    dbContext.TestHosts.Attach(deleteObj);
                    dbContext.TestHosts.Remove(deleteObj);

                    await dbContext.SaveChangesAsync(cancellationToken);
                }
            });
        }

        public async Task Update(TestHost source, CancellationToken cancellationToken = default)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, false, false, _mainDBConnectionFactory.CreateAllForMain(), async (conn, transaction) =>
            {
                await using (var dbContext = _mainDBContextFactory.CreateMainDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }
                    //Guid sshEndpointId = source.SSHEndpointID;
                    source.ModifyTime = DateTime.UtcNow;
                    dbContext.TestHosts.Attach(source);
                    //source.SSHEndpointID = sshEndpointId;
                    var entry = dbContext.Entry(source);
                    foreach (var item in entry.Properties)
                    {
                        if(item.Metadata.Name != "ID")
                        {
                            if (source.Attributes.ContainsKey(item.Metadata.Name))
                            {
                                entry.Property(item.Metadata.Name).IsModified = true;
                            }
                        }
                    }
                    await dbContext.SaveChangesAsync(cancellationToken);
                }
            });
        }

        public async Task<TestHost?> QueryByName(string address, CancellationToken cancellationToken = default)
        {
            TestHost? result = null;
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, true, false, _mainDBConnectionFactory.CreateReadForMain(), async (conn, transaction) =>
            {
                await using (var dbContext = _mainDBContextFactory.CreateMainDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }

                    result = await (from item in dbContext.TestHosts
                                    where item.Address == address
                                    select item).FirstOrDefaultAsync();
                }
            });

            return result;
        }

        public async Task<Guid?> QueryByNameNoLock(string address, CancellationToken cancellationToken = default)
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


                        var testHost = await (from item in dbContext.TestHosts
                                              where item.Address == address
                                              orderby EF.Property<long>(item, "Sequence") descending
                                              select item).FirstOrDefaultAsync();
                        if (testHost != null)
                            result = testHost.ID;
                    }
                });

                scope.Complete();
            }
            return result;
        }
        public async Task<TestHost?> QueryByID(Guid id, CancellationToken cancellationToken = default)
        {
            TestHost? result = null;
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, true, false, _mainDBConnectionFactory.CreateReadForMain(), async (conn, transaction) =>
            {
                await using (var dbContext = _mainDBContextFactory.CreateMainDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }

                    result =await (from item in dbContext.TestHosts
                              where item.ID == id
                              select item).Include(item => item.SSHEndpoint).FirstOrDefaultAsync();
                }
            });

            return result;
        }

        public async Task<QueryResult<TestHost>> QueryByPage(string matchAddress, int page, int pageSize, CancellationToken cancellationToken = default)
        {
            QueryResult<TestHost> result = new QueryResult<TestHost>()
            {
                CurrentPage = page
            };

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, true, false, _mainDBConnectionFactory.CreateReadForMain(), async (conn, transaction) =>
            {
                await using (var dbContext = _mainDBContextFactory.CreateMainDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }

                    var strLike = $"{matchAddress.ToSqlLike()}%";
                    var count = await (from item in dbContext.TestHosts
                                    where EF.Functions.Like(item.Address, strLike) && item.SSHEndpoint != null
                                       select item.ID).CountAsync();

                    result.TotalCount = count;

                    var ids= (from item in dbContext.TestHosts
                              where EF.Functions.Like(item.Address, strLike) && item.SSHEndpoint != null
                                        orderby item.CreateTime descending
                                        select item.ID                                 
                                        ).Skip((page-1)*pageSize).Take(pageSize);

                    var datas =await (from item in dbContext.TestHosts
                                      join idItem in ids
                                 on item.ID equals idItem
                                 orderby item.CreateTime descending
                                 select item).Include(item => item.SSHEndpoint).ToListAsync();

                    result.Results.AddRange(datas);                    
                }
            });

            return result;
        }

        public async Task DeleteTestHosts(List<Guid> ids, CancellationToken cancellationToken = default)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, false, false, _mainDBConnectionFactory.CreateAllForMain(), async (conn, transaction) =>
            {
                await using (var dbContext = _mainDBContextFactory.CreateMainDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }
                    List<TestHost> list = new List<TestHost>();
                    foreach (Guid id in ids)
                    {
                        var deleteObj = new TestHost() { ID = id };
                        list.Add(deleteObj);
                    }
                    dbContext.TestHosts.AttachRange(list.ToArray());
                    dbContext.TestHosts.RemoveRange(list.ToArray());
                    await dbContext.SaveChangesAsync(cancellationToken);
                }
            });
        }

        public IAsyncEnumerable<TestHost> GetHosts(CancellationToken cancellationToken = default)
        {
            AsyncInteration<TestHost> interation = new AsyncInteration<TestHost>(
                async (index) =>
                {
                    List<TestHost>? datas = null;
                    await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, true, false, _mainDBConnectionFactory.CreateReadForMain(), async (conn, transaction) =>
                    {
                        await using (var dbContext = _mainDBContextFactory.CreateMainDBContext(conn))
                        {
                            if (transaction != null)
                            {
                                await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                            }

                            var ids = (from item in dbContext.TestHosts
                                       where item.SSHEndpoint != null
                                       orderby EF.Property<long>(item, "Sequence")
                                       select item.ID
                                                ).Skip((index) * 500).Take(500);
                            datas = await (from item in dbContext.TestHosts
                                           join idItem in ids
                                           on item.ID equals idItem
                                           orderby EF.Property<long>(item, "Sequence")
                                           select item).ToListAsync();
                        }
                    });

                    return datas;
                }
                );
            return interation;
        }

        public async Task<bool> IsUsedByTestCases(Guid hostId, CancellationToken cancellationToken = default)
        {
            bool result = false;
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, true, false, _mainDBConnectionFactory.CreateReadForMain(), async (conn, transaction) =>
            {
                await using (var dbContext = _mainDBContextFactory.CreateMainDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }
                    var count = await (from item in dbContext.TestCases
                                       where item.MasterHostID == hostId
                                       select item.ID).CountAsync();
                    if (count > 0)
                        result = true;
                }
            });

            return result;
        }

        public async Task<bool> IsUsedBySlaveHosts(Guid hostId, CancellationToken cancellationToken = default)
        {
            bool result = false;
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, true, false, _mainDBConnectionFactory.CreateReadForMain(), async (conn, transaction) =>
            {
                await using (var dbContext = _mainDBContextFactory.CreateMainDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }

                    var count = await (from item in dbContext.TestCaseSlaveHosts
                                       where item.HostID == hostId
                                       select item.ID).CountAsync();
                    if (count > 0)
                        result = true;
                }
            });

            return result;
        }

        public async Task<bool> GetTestHostsBySSHEndpointId(Guid id, CancellationToken cancellationToken = default)
        {
            bool result = false;
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, true, false, _mainDBConnectionFactory.CreateReadForMain(), async (conn, transaction) =>
            {
                await using (var dbContext = _mainDBContextFactory.CreateMainDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }

                    var count = await (from item in dbContext.TestHosts
                                       where item.SSHEndpointID == id
                                       select item.ID).CountAsync();
                    if (count > 0)
                        result = true;
                }
            });
            return result;
        }

        public async Task<List<TestCase>> GetRunningTestCasesByHostId(Guid id, CancellationToken cancellationToken = default)
        {
            List<TestCase> testCases = new List<TestCase>();
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, true, false, _mainDBConnectionFactory.CreateReadForMain(), async (conn, transaction) =>
            {
                await using (var dbContext = _mainDBContextFactory.CreateMainDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }
                    var testCaseIds = (from slaveItem in dbContext.TestCaseSlaveHosts
                                       where slaveItem.HostID == id
                                       select slaveItem.TestCaseID).Distinct();

                    testCases = await (from item in dbContext.TestCases
                                       where (item.MasterHostID == id || testCaseIds.Contains(item.ID)) && item.Status == TestCaseStatus.Running
                                       select item).Distinct().ToListAsync();
                }
            });
            return testCases;
        }
    }
}
