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


        public Task Add(TreeEntity entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task Delete(Guid id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<QueryResult<TreeEntity>> Query(string? matchName,int? type, int page, int pageSize, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<TreeEntity?> QueryByID(Guid id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<TreeEntity?> QueryByName(string name, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Guid?> QueryByNameNoLock(string name, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<QueryResult<TreeEntity>> QueryChildren(Guid? partentID, string? matchName,int? type, int page, int pageSize, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Guid?> QueryFirstChildren(Guid partentID, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
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

        public Task UpdateName(Guid id, string name, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task UpdateParent(Guid id, Guid? parentID, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
