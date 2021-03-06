﻿using MSLibrary;
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

namespace FW.TestPlatform.Main.Entities.DAL
{
    [Injection(InterfaceType = typeof(ITestCaseStore), Scope = InjectionScope.Singleton)]
    public class TestCaseStore : ITestCaseStore
    {
        private readonly IMainDBConnectionFactory _mainDBConnectionFactory;
        private readonly IMainDBContextFactory _mainDBContextFactory;

        public TestCaseStore(IMainDBConnectionFactory mainDBConnectionFactory, IMainDBContextFactory mainDBContextFactory)
        {
            _mainDBConnectionFactory = mainDBConnectionFactory;
            _mainDBContextFactory = mainDBContextFactory;
        }

        public async Task Add(TestCase source, CancellationToken cancellationToken = default)
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
                    var entity = await dbContext.TestCases.AddAsync(source, cancellationToken);
                    var result = await dbContext.SaveChangesAsync(cancellationToken);
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
                    var deleteObj = new TestCase() { ID = id };
                    dbContext.TestCases.Attach(deleteObj);
                    dbContext.TestCases.Remove(deleteObj);
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
                    List<TestCase> list = new List<TestCase>();
                    foreach(Guid id in ids)
                    {
                        var deleteObj = new TestCase() { ID = id };
                        list.Add(deleteObj);
                    }
                    dbContext.TestCases.AttachRange(list.ToArray());
                    dbContext.TestCases.RemoveRange(list.ToArray());
                    await dbContext.SaveChangesAsync(cancellationToken);
                }
            });
        }

        public async Task<TestCase?> QueryByID(Guid id, CancellationToken cancellationToken = default)
        {
            TestCase? result = null;
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, true, false, _mainDBConnectionFactory.CreateReadForMain(), async (conn, transaction) =>
            {
                await using (var dbContext = _mainDBContextFactory.CreateMainDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }
                    result = await (from item in dbContext.TestCases
                                    where item.ID == id
                                    select item).Include(u => u.MasterHost).ThenInclude(u => u.SSHEndpoint).FirstOrDefaultAsync();
                }
            });

            return result;
        }

        public async Task<TestCaseStatus?> QueryStatusByID(Guid id, CancellationToken cancellationToken = default)
        {
            TestCaseStatus? result = null;
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, true, false, _mainDBConnectionFactory.CreateReadForMain(), async (conn, transaction) =>
            {
                await using (var dbContext = _mainDBContextFactory.CreateMainDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }
                    TestCase testCase = await (from item in dbContext.TestCases
                                    where item.ID == id
                                    select item).FirstOrDefaultAsync();
                    if (testCase != null)
                        result = testCase.Status;
                }
            });

            return result;
        }

        public async Task<TestCase?> QueryByName(string name, CancellationToken cancellationToken = default)
        {
            TestCase? result = null;
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, true, false, _mainDBConnectionFactory.CreateReadForMain(), async (conn, transaction) =>
            {
                await using (var dbContext = _mainDBContextFactory.CreateMainDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }


                    result = await (from item in dbContext.TestCases
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


                        var testCase = await (from item in dbContext.TestCases
                                        where item.Name == name
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

        public async Task<IList<TestCase>> QueryByNames(IList<string> names, CancellationToken cancellationToken = default)
        {
            List<TestCase> result = new List<TestCase>();

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, true, false, _mainDBConnectionFactory.CreateReadForMain(), async (conn, transaction) =>
            {
                await using (var dbContext = _mainDBContextFactory.CreateMainDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }


                    result = await (from item in dbContext.TestCases
                                    where names.Contains(item.Name)
                                    select item).ToListAsync();
                }
            });


            return result;
        }

        public async Task<QueryResult<TestCase>> QueryByPage(string matchName, int page, int pageSize, CancellationToken cancellationToken = default)
        {
            QueryResult<TestCase> result = new QueryResult<TestCase>()
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
                    var count = await (from item in dbContext.TestCases
                                       where EF.Functions.Like(item.Name, strLike)
                                       select item.ID).CountAsync();

                    result.TotalCount = count;

                    var ids = (from item in dbContext.TestCases
                               where EF.Functions.Like(item.Name, strLike)
                               orderby item.CreateTime descending
                               select item.ID
                                        ).Skip((page - 1) * pageSize).Take(pageSize);

                    var datas = await (from item in dbContext.TestCases
                                       join idItem in ids
                                  on item.ID equals idItem
                                       orderby EF.Property<long>(item, "Sequence") descending
                                       select item).ToListAsync();

                    result.Results.AddRange(datas);
                }
            });

            return result;
        }

        public async Task<QueryResult<TestCase>> QueryByParentId(Guid? parentId, int page, int pageSize, CancellationToken cancellationToken = default)
        {
            QueryResult<TestCase> result = new QueryResult<TestCase>()
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
                                       where item.ParentID == parentId && item.Type == 2
                                       select item.ID).CountAsync();

                    result.TotalCount = count;

                    var ids = (from item in dbContext.TreeEntities
                               where item.ParentID == parentId && item.Type == 2
                               orderby item.CreateTime descending
                               select item.Value
                                        ).Skip((page-1)*pageSize).Take(pageSize);
                    if (ids.Count() > 0)
                    {
                        var datas = await (from item in dbContext.TestCases
                                           join idItem in ids
                                      on item.ID.ToString() equals idItem
                                           orderby EF.Property<long>(item, "Sequence") descending
                                           select item).ToListAsync();

                        result.Results.AddRange(datas);
                    }
                    //var countWithoutTree = 0;
                    //if (parentId == null)
                    //{
                    //    countWithoutTree = await (from item in dbContext.TestCases
                    //                              where item.TreeID == null
                    //                              orderby item.CreateTime descending
                    //                              select item).CountAsync();
                    //}
                    
                    //List<TestCase> datasWithoutTree = new List<TestCase>();
                    //if((page - 1) * pageSize < countWithoutTree)
                    //{
                    //    datasWithoutTree = await (from item in dbContext.TestCases
                    //                                    where item.TreeID == null
                    //                                    select item).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
                    //}
                    //if (countWithoutTree < page * pageSize)
                    //{
                    //    int skipCount = 0;
                    //    int takeCount = 0;
                    //    if (countWithoutTree <= (page - 1) * pageSize)
                    //    {
                    //        skipCount = (page - 1) * pageSize - countWithoutTree;
                    //        takeCount = pageSize;
                    //    }
                    //    else
                    //    {
                    //        skipCount = 0;
                    //        takeCount = pageSize - (countWithoutTree - (page - 1) * pageSize);
                    //    }
                    //}
                    //if(datasWithoutTree.Count() > 0)
                    //{
                    //    result.Results.AddRange(datasWithoutTree);
                    //}
                }
            });

            return result;
        }

        public async Task<QueryResult<TestCase>> QueryByPage(int page, int pageSize, CancellationToken cancellationToken = default)
        {
            QueryResult<TestCase> result = new QueryResult<TestCase>()
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
                    var count = await (from item in dbContext.TestCases
                                       select item.ID).CountAsync();

                    result.TotalCount = count;

                    var datas = await (from item in dbContext.TestCases
                                       orderby EF.Property<long>(item, "Sequence") descending
                               select item).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

                    result.Results.AddRange(datas);
                }
            });

            return result;
        }

        public async Task Update(TestCase source, CancellationToken cancellationToken = default)
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
                    dbContext.TestCases.Attach(source);
                    var entry = dbContext.Entry(source);

                    foreach (var item in entry.Properties)
                    {
                        if (item.Metadata.Name != "ID" && item.Metadata.Name != "Status" && item.Metadata.Name != "TestCaseHistoryID" && item.Metadata.Name!= "MasterHost")
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

        public async Task UpdateStatus(Guid id, TestCaseStatus status, CancellationToken cancellationToken = default)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, false, false, _mainDBConnectionFactory.CreateAllForMain(), async (conn, transaction) =>
            {
                await using (var dbContext = _mainDBContextFactory.CreateMainDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }

                    var updateCase = new TestCase()
                    {
                        ID = id,
                        Status = status,
                        ModifyTime = DateTime.UtcNow
                    };

                    dbContext.TestCases.Attach(updateCase);
                    var entry = dbContext.Entry(updateCase);

                    foreach (var item in entry.Properties)
                    {
                        if (item.Metadata.Name != "ID")
                        {
                            if (updateCase.Attributes.ContainsKey(item.Metadata.Name))
                            {
                                entry.Property(item.Metadata.Name).IsModified = true;
                            }
                        }
                    }
                    await dbContext.SaveChangesAsync(cancellationToken);
                }
            });          
        }

        public async Task UpdateHistoryId(Guid id, Guid historyId, CancellationToken cancellationToken = default)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, false, false, _mainDBConnectionFactory.CreateAllForMain(), async (conn, transaction) =>
            {
                await using (var dbContext = _mainDBContextFactory.CreateMainDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }

                    var updateCase = new TestCase()
                    {
                        ID = id,
                        TestCaseHistoryID = historyId,
                        ModifyTime = DateTime.UtcNow
                    };

                    dbContext.TestCases.Attach(updateCase);
                    var entry = dbContext.Entry(updateCase);

                    foreach (var item in entry.Properties)
                    {
                        if (item.Metadata.Name != "ID")
                        {
                            if (updateCase.Attributes.ContainsKey(item.Metadata.Name))
                            {
                                entry.Property(item.Metadata.Name).IsModified = true;
                            }
                        }
                    }
                    await dbContext.SaveChangesAsync(cancellationToken);
                }
            });

        }

        public async Task UpdateHistoryIdAndStatus(Guid id, Guid historyId, TestCaseStatus status, CancellationToken cancellationToken = default)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, false, false, _mainDBConnectionFactory.CreateAllForMain(), async (conn, transaction) =>
            {
                await using (var dbContext = _mainDBContextFactory.CreateMainDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }

                    var updateCase = new TestCase()
                    {
                        ID = id,
                        TestCaseHistoryID = historyId,
                        Status=status,
                        ModifyTime = DateTime.UtcNow
                    };

                    dbContext.TestCases.Attach(updateCase);
                    var entry = dbContext.Entry(updateCase);

                    foreach (var item in entry.Properties)
                    {
                        if (item.Metadata.Name != "ID")
                        {
                            if (updateCase.Attributes.ContainsKey(item.Metadata.Name))
                            {
                                entry.Property(item.Metadata.Name).IsModified = true;
                            }
                        }
                    }
                    await dbContext.SaveChangesAsync(cancellationToken);
                }
            });

        }
        public async Task<List<TestCase>> QueryCountNolockByStatus(TestCaseStatus status, IList<Guid> hostIds, CancellationToken cancellationToken = default)
        {
            List<TestCase> result = new List<TestCase>();

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, true, false, _mainDBConnectionFactory.CreateReadForMain(), async (conn, transaction) =>
            {
                await using (var dbContext = _mainDBContextFactory.CreateMainDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }


                    result = await (from item in dbContext.TestCases
                                    where item.Status == status && hostIds.Contains(item.MasterHostID) && item.EngineType != EngineTypes.Jmeter
                                    select item).ToListAsync();
                }
            });

            return result;
        }
    }
}
