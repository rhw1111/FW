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

                    source.ModifyTime = DateTime.UtcNow;
                    dbContext.TestHosts.Attach(source);

                    var entry = dbContext.Entry(source);
                    foreach (var item in entry.Properties)
                    {
                        entry.Property(item.Metadata.Name).IsModified = true;
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


                        var testCase = await (from item in dbContext.TestHosts
                                              where item.Address == address
                                              orderby EF.Property<long>(item, "Sequence") descending
                                              select item).FirstOrDefaultAsync();
                        if (testCase != null)
                            result = testCase.ID;
                    }
                });

                scope.Complete();
            }
            return result;
        }
        public async Task<TestHost> QueryByID(Guid id, CancellationToken cancellationToken = default)
        {
            TestHost result = new TestHost();
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
                              select item).FirstOrDefaultAsync();
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
                                    where EF.Functions.Like(item.Address, strLike)
                                    select item.ID).CountAsync();

                    result.TotalCount = count;

                    var ids= (from item in dbContext.TestHosts
                              where EF.Functions.Like(item.Address, strLike)  
                                        orderby item.CreateTime descending
                                        select item.ID                                 
                                        ).Skip((page-1)*pageSize).Take(pageSize);

                    var datas =await (from item in dbContext.TestHosts
                                      join idItem in ids
                                 on item.ID equals idItem
                                 orderby item.CreateTime descending
                                 select item).ToListAsync();

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
    }
}
