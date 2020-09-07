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
using MSLibrary.Collections.DAL;
using MSLibrary.Collections;
using FW.TestPlatform.Main.DAL;

namespace FW.TestPlatform.Main.Collections.DAL
{
    [Injection(InterfaceType = typeof(ITreeEntityStore), Scope = InjectionScope.Singleton)]
    public class TreeEntityStore : ITreeEntityStore
    {
        private readonly ICollectionConnectionFactory _collectionConnectionFactory;
        private readonly IMainDBContextFactory _mainDBContextFactory;

        public TreeEntityStore(ICollectionConnectionFactory collectionConnectionFactory, IMainDBContextFactory mainDBContextFactory)
        {
            _collectionConnectionFactory = collectionConnectionFactory;
            _mainDBContextFactory = mainDBContextFactory;
        }


        public async Task Add(TreeEntity entity, CancellationToken cancellationToken = default)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, false, false, _collectionConnectionFactory.CreateAllForCollection(), async (conn, transaction) =>
            {
                await using (var dbContext = _mainDBContextFactory.CreateMainDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }
                    entity.ModifyTime = DateTime.UtcNow;
                    entity.CreateTime = DateTime.UtcNow;
                    if (entity.ID == Guid.Empty)
                    {
                        entity.ID = Guid.NewGuid();
                    }
                    await dbContext.TreeEntities.AddAsync(entity, cancellationToken);
                    var result = await dbContext.SaveChangesAsync(cancellationToken);
                }
            });
        }

        public async Task Delete(Guid id, CancellationToken cancellationToken = default)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, false, false, _collectionConnectionFactory.CreateAllForCollection(), async (conn, transaction) =>
            {
                await using(var dbContext = _mainDBContextFactory.CreateMainDBContext(conn))
                {
                    if(transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }
                    var deleteObj = new TreeEntity() { ID = id};
                    dbContext.TreeEntities.Attach(deleteObj);
                    dbContext.TreeEntities.Remove(deleteObj);
                    await dbContext.SaveChangesAsync(cancellationToken);
                }
            });
        }

        public async Task<QueryResult<TreeEntity>> Query(string? matchName,int? type, int page, int pageSize, CancellationToken cancellationToken = default)
        {
            QueryResult<TreeEntity> result = new QueryResult<TreeEntity>()
            {
                CurrentPage = page
            };

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, true, false, _collectionConnectionFactory.CreateAllForCollection(), async (conn, transaction) =>
            {
                await using (var dbContext = _mainDBContextFactory.CreateMainDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }

                    var strLike = $"%{matchName.ToSqlLike()}%";
                    var count = 0;
                    if (type == null)
                    {
                        count = await (from item in dbContext.TreeEntities
                                       where EF.Functions.Like(item.Name, strLike)
                                       select item.ID).CountAsync();
                    }
                    else
                    {
                        count = await (from item in dbContext.TreeEntities
                                       where EF.Functions.Like(item.Name, strLike) && item.Type == type
                                       orderby item.CreateTime descending
                                       select item.ID).CountAsync();
                    }
                    result.TotalCount = count;

                    var ids = (from item in dbContext.TreeEntities
                                 where EF.Functions.Like(item.Name, strLike) && (type == null || item.Type == type)
                                 select item.ID).Skip((page - 1) * pageSize).Take(pageSize);
                    //if(type != null)
                    //{
                    //    items = (from item in items
                    //             where item.Type ==  type
                    //           select item);
                    //}
                    //var ids = (from item in items
                    //         orderby item.CreateTime descending
                    //         select item.ID).Skip((page - 1) * pageSize).Take(pageSize);

                    var datas = await (from item in dbContext.TreeEntities
                                       join idItem in ids
                                  on item.ID equals idItem
                                       orderby EF.Property<long>(item, "Sequence") descending
                                       select item).ToListAsync();

                    result.Results.AddRange(datas);
                }
            });

            return result;
        }

        public async Task<TreeEntity?> QueryByID(Guid id, CancellationToken cancellationToken = default)
        {
            TreeEntity? result = null;
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, true, false, _collectionConnectionFactory.CreateReadForCollection(), async (conn, transaction) =>
            {
                await using (var dbContext = _mainDBContextFactory.CreateMainDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }
                    result = await (from item in dbContext.TreeEntities
                                    where item.ID == id
                                    select item).FirstOrDefaultAsync();
                }
            });

            return result;
        }

        public async Task<TreeEntity?> QueryByName(Guid? parentId, string name, CancellationToken cancellationToken = default)
        {
            TreeEntity? result = null;
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, true, false, _collectionConnectionFactory.CreateAllForCollection(), async (conn, transaction) =>
            {
                await using (var dbContext = _mainDBContextFactory.CreateMainDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }


                    result = await (from item in dbContext.TreeEntities
                                    where item.Name == name && item.ParentID == parentId
                                    select item).FirstOrDefaultAsync();
                }
            });

            return result;
        }

        public async Task<Guid?> QueryByNameNoLock(Guid? parentId, string name, CancellationToken cancellationToken = default)
        {
            Guid? result = null;
            await using (DBTransactionScope scope = new DBTransactionScope(System.Transactions.TransactionScopeOption.RequiresNew, new System.Transactions.TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted, Timeout = new TimeSpan(0, 0, 30) }))
            {
                await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, true, false, _collectionConnectionFactory.CreateAllForCollection(), async (conn, transaction) =>
                {
                    await using (var dbContext = _mainDBContextFactory.CreateMainDBContext(conn))
                    {
                        if (transaction != null)
                        {
                            await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                        }
                        var testCase = await (from item in dbContext.TreeEntities
                                              where item.Name == name && item.ParentID == parentId
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

        public async Task<QueryResult<TreeEntity>> QueryChildren(Guid? parentID, string? matchName,int? type, int page, int pageSize, CancellationToken cancellationToken = default)
        {
            QueryResult<TreeEntity> result = new QueryResult<TreeEntity>()
            {
                CurrentPage = page
            };
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, false, false, _collectionConnectionFactory.CreateAllForCollection(), async (conn, transaction) => { 
                await using(var dbContext = _mainDBContextFactory.CreateMainDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }

                    var strLike = $"%{matchName.ToSqlLike()}%";
                    var count = 0;
                    if (type == null)
                    {
                        count = await (from item in dbContext.TreeEntities
                                       where EF.Functions.Like(item.Name, strLike) && item.ParentID == parentID
                                       select item.ID).CountAsync();
                    }
                    else
                    {
                        count = await (from item in dbContext.TreeEntities
                                       where EF.Functions.Like(item.Name, strLike) && item.Type == type && item.ParentID == parentID
                                       select item.ID).CountAsync();
                    }
                    result.TotalCount = count;

                    var ids = (from item in dbContext.TreeEntities
                               where EF.Functions.Like(item.Name, strLike) && (type == null || item.Type == type) && item.ParentID == parentID
                               orderby item.CreateTime descending
                               select item.ID).Skip((page - 1) * pageSize).Take(pageSize);

                    var datas = await (from item in dbContext.TreeEntities
                                       join idItem in ids
                                  on item.ID equals idItem
                                       orderby EF.Property<long>(item, "Sequence") descending
                                       select item).ToListAsync();

                    result.Results.AddRange(datas);
                }
            });
            return result;
        }

        public async Task<Guid?> QueryFirstChildren(Guid parentID, CancellationToken cancellationToken = default)
        {
            Guid? result = null;
            await using (DBTransactionScope scope = new DBTransactionScope(System.Transactions.TransactionScopeOption.RequiresNew, new System.Transactions.TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted, Timeout = new TimeSpan(0, 0, 30) }))
            {
                await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, true, false, _collectionConnectionFactory.CreateAllForCollection(), async (conn, transaction) =>
                {
                    await using (var dbContext = _mainDBContextFactory.CreateMainDBContext(conn))
                    {
                        if (transaction != null)
                        {
                            await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                        }

                        var testCase = await (from item in dbContext.TreeEntities
                                              where item.ParentID == parentID
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

        public async Task Update(TreeEntity entity, CancellationToken cancellationToken = default)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, false, false, _collectionConnectionFactory.CreateAllForCollection(), async (conn, transaction) =>
            {
                await using (var dbContext = _mainDBContextFactory.CreateMainDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }
                    entity.ModifyTime = DateTime.UtcNow;
                    dbContext.TreeEntities.Attach(entity);

                    var entry = dbContext.Entry(entity);
                    foreach (var item in entry.Properties)
                    {
                        if (item.Metadata.Name != "ID" && item.Metadata.Name != "Parent")
                        {
                            if (entity.Attributes.ContainsKey(item.Metadata.Name))
                            {
                                entry.Property(item.Metadata.Name).IsModified = true;
                            }
                        }
                    }
                    await dbContext.SaveChangesAsync(cancellationToken);
                }
            });
        }

        public async Task UpdateName(Guid id, string name, CancellationToken cancellationToken = default)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, false, false, _collectionConnectionFactory.CreateAllForCollection(), async (conn, transaction) =>
            {
                await using (var dbContext = _mainDBContextFactory.CreateMainDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }
                    TreeEntity entity = new TreeEntity
                    {
                        ID = id,
                        Name = name,
                        ModifyTime = DateTime.UtcNow
                    };
                    dbContext.TreeEntities.Attach(entity);
                    var entry = dbContext.Entry(entity);
                    foreach (var item in entry.Properties)
                    {
                        if (item.Metadata.Name != "ID" && item.Metadata.Name != "Parent")
                        {
                            if (entity.Attributes.ContainsKey(item.Metadata.Name))
                            {
                                entry.Property(item.Metadata.Name).IsModified = true;
                            }
                        }
                    }
                    await dbContext.SaveChangesAsync(cancellationToken);
                }
            });
        }

        public async Task UpdateParent(Guid id, Guid? parentID, CancellationToken cancellationToken = default)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, false, false, _collectionConnectionFactory.CreateAllForCollection(), async (conn, transaction) =>
            {
                await using (var dbContext = _mainDBContextFactory.CreateMainDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }
                    TreeEntity entity = new TreeEntity
                    {
                        ID = id,
                        ParentID = parentID,
                        ModifyTime = DateTime.UtcNow
                    };
                    dbContext.TreeEntities.Attach(entity);
                    var entry = dbContext.Entry(entity);
                    foreach (var item in entry.Properties)
                    {
                        if (item.Metadata.Name != "ID" && item.Metadata.Name != "Parent")
                        {
                            if (entity.Attributes.ContainsKey(item.Metadata.Name))
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
