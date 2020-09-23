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
using MSLibrary.Collections;

namespace FW.TestPlatform.Main.Entities.DAL
{
    [Injection(InterfaceType = typeof(ITestDataSourceStore), Scope = InjectionScope.Singleton)]
    public class TestDataSourceStore : ITestDataSourceStore
    {
        private readonly IMainDBConnectionFactory _mainDBConnectionFactory;
        private readonly IMainDBContextFactory _mainDBContextFactory;

        public TestDataSourceStore(IMainDBConnectionFactory mainDBConnectionFactory, IMainDBContextFactory mainDBContextFactory)
        {
            _mainDBConnectionFactory = mainDBConnectionFactory;
            _mainDBContextFactory = mainDBContextFactory;
        }

        public async Task Add(TestDataSource source, CancellationToken cancellationToken = default)
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

                    await dbContext.TestDataSources.AddAsync(source, cancellationToken);

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

                    var deleteObj = new TestDataSource() { ID = id };
                    dbContext.TestDataSources.Attach(deleteObj);
                    dbContext.TestDataSources.Remove(deleteObj);

                    await dbContext.SaveChangesAsync(cancellationToken);
                }
            });
        }

        public async Task<TestDataSource?> QueryByID(Guid id, CancellationToken cancellationToken = default)
        {
            TestDataSource? result = null;
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, true, false, _mainDBConnectionFactory.CreateReadForMain(), async (conn, transaction) =>
            {
                await using (var dbContext = _mainDBContextFactory.CreateMainDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }


                    result =await (from item in dbContext.TestDataSources
                              where item.ID == id
                              select item).FirstOrDefaultAsync();
                }
            });

            return result;
        }

        public async Task<TestDataSource?> QueryByName(string name, CancellationToken cancellationToken = default)
        {
            TestDataSource? result = null;
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, true, false, _mainDBConnectionFactory.CreateReadForMain(), async (conn, transaction) =>
            {
                await using (var dbContext = _mainDBContextFactory.CreateMainDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }


                    result = await (from item in dbContext.TestDataSources
                                    where item.Name == name
                                    select item).FirstOrDefaultAsync();
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

                        var dataSourceItem = await (from item in dbContext.TestDataSources
                                          where item.Name == name
                                          orderby EF.Property<long>(item, "Sequence")
                                          select item).FirstOrDefaultAsync();
                        if (dataSourceItem != null)
                            result = dataSourceItem.ID;
                        //result = await (from item in dbContext.TestDataSources
                        //                where item.Name == name
                        //                orderby EF.Property<long>(item, "Sequence")
                        //                select item.ID).FirstOrDefaultAsync();
                    }
                });

                scope.Complete();
            }
            return result;
        }

        public async Task<IList<TestDataSource>> QueryByNames(IList<string> names, CancellationToken cancellationToken = default)
        {
            List<TestDataSource> result = new List<TestDataSource>();

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, true, false, _mainDBConnectionFactory.CreateReadForMain(), async (conn, transaction) =>
            {
                await using (var dbContext = _mainDBContextFactory.CreateMainDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }


                    result = await (from item in dbContext.TestDataSources
                                    where names.Contains(item.Name)
                                    select item).ToListAsync();
                }
            });


            return result;
        }

        public async Task<QueryResult<TestDataSource>> QueryByParentId(Guid? parentId, int page, int pageSize, CancellationToken cancellationToken = default)
        {
            QueryResult<TestDataSource> result = new QueryResult<TestDataSource>()
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

                    var count = await (from item in dbContext.TreeEntities
                                    where item.ParentID == parentId && item.Type == 3
                                    select item.ID).CountAsync();

                    //result.TotalCount = count;
                    var countWithoutTree = 0;
                    if (parentId == null)
                    {
                        countWithoutTree = await (from item in dbContext.TestDataSources
                                                    where item.TreeID == null
                                                    select item).CountAsync();
                    }
                    result.TotalCount = count + countWithoutTree;
                    List<TestDataSource> datasWithoutTree = new List<TestDataSource>();
                    if ((page - 1) * pageSize < countWithoutTree)
                    {
                        datasWithoutTree = await (from item in dbContext.TestDataSources
                                                  where item.TreeID == null
                                                  orderby item.CreateTime descending
                                                  select item).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
                    }
                    if (countWithoutTree < page* pageSize)
                    {
                        int skipCount = 0;
                        int takeCount = 0;
                        if (countWithoutTree <= (page - 1) * pageSize)
                        {
                            skipCount = (page - 1) * pageSize - countWithoutTree;
                            takeCount = pageSize;
                        }
                        else
                        {
                            skipCount = 0;
                            takeCount = pageSize - (countWithoutTree - (page - 1) * pageSize);
                        }

                        var ids = (from item in dbContext.TreeEntities
                                   where item.ParentID == parentId && item.Type == 3
                                   orderby EF.Property<long>(item, "Sequence") descending
                                   select item.Value
                                        ).Skip(skipCount).Take(takeCount);

                        if (ids.Count() > 0)
                        {
                            var datas = await (from item in dbContext.TestDataSources
                                               join idItem in ids
                                               on item.ID.ToString() equals idItem
                                               orderby item.CreateTime descending
                                               select item).ToListAsync();
                            result.Results.AddRange(datas);
                        }
                    }
                    if (datasWithoutTree.Count() > 0)
                    {
                        result.Results.AddRange(datasWithoutTree);
                    }
                }
            });

            return result;
        }

        public async Task<QueryResult<TestDataSource>> QueryByPage(string matchName, int page, int pageSize, CancellationToken cancellationToken = default)
        {
            QueryResult<TestDataSource> result = new QueryResult<TestDataSource>()
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

                    var strLike = $"%{matchName.ToMySqlLike()}%";
                    var count = await (from item in dbContext.TestDataSources
                                       where EF.Functions.Like(item.Name, strLike)
                                       select item.ID).CountAsync();

                    result.TotalCount = count;

                    var ids = (from item in dbContext.TestDataSources
                               where EF.Functions.Like(item.Name, strLike)
                               orderby EF.Property<long>(item, "Sequence") descending
                               select item.ID
                                        ).Skip((page - 1) * pageSize).Take(pageSize);

                    var datas = await (from item in dbContext.TestDataSources
                                       join idItem in ids
                                       on item.ID equals idItem
                                       orderby item.CreateTime descending
                                       select item).ToListAsync();

                    result.Results.AddRange(datas);
                }
            });

            return result;
        }

        public async Task Update(TestDataSource source, CancellationToken cancellationToken = default)
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
                        dbContext.TestDataSources.Attach(source);

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

        public async Task DeleteMutiple(List<Guid> ids, CancellationToken cancellationToken = default)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, false, false, _mainDBConnectionFactory.CreateAllForMain(), async (conn, transaction) =>
            {
                await using (var dbContext = _mainDBContextFactory.CreateMainDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }
                    List<TestDataSource> list = new List<TestDataSource>();
                    foreach (Guid id in ids)
                    {
                        var deleteObj = new TestDataSource() { ID = id };
                        list.Add(deleteObj);
                    }
                    dbContext.TestDataSources.AttachRange(list.ToArray());
                    dbContext.TestDataSources.RemoveRange(list.ToArray());
                    await dbContext.SaveChangesAsync(cancellationToken);
                }
            });
        }

        public IAsyncEnumerable<TestDataSource> GetDataSources(CancellationToken cancellationToken = default)
        {
            AsyncInteration<TestDataSource> interation = new AsyncInteration<TestDataSource>(
                async (index) =>
                {
                    List<TestDataSource>? datas = null;
                    await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, true, false, _mainDBConnectionFactory.CreateReadForMain(), async (conn, transaction) =>
                    {
                        await using (var dbContext = _mainDBContextFactory.CreateMainDBContext(conn))
                        {
                            if (transaction != null)
                            {
                                await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                            }

                            var ids = (from item in dbContext.TestDataSources
                                       orderby EF.Property<long>(item, "Sequence")
                                       select item.ID
                                                ).Skip((index) * 500).Take(500);

                            datas = await (from item in dbContext.TestDataSources
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
