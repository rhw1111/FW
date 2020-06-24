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

namespace FW.TestPlatform.Main.Entities.DAL
{
    [Injection(InterfaceType = typeof(IScriptTemplateStore), Scope = InjectionScope.Singleton)]
    public class ScriptTemplateStore : IScriptTemplateStore
    {
        private readonly IMainDBConnectionFactory _mainDBConnectionFactory;
        private readonly IMainDBContextFactory _mainDBContextFactory;

        public ScriptTemplateStore(IMainDBConnectionFactory mainDBConnectionFactory, IMainDBContextFactory mainDBContextFactory)
        {
            _mainDBConnectionFactory = mainDBConnectionFactory;
            _mainDBContextFactory = mainDBContextFactory;
        }

        public async Task Add(ScriptTemplate template, CancellationToken cancellationToken = default)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, false, false, _mainDBConnectionFactory.CreateAllForMain(), async (conn, transaction) =>
            {
                await using (var dbContext = _mainDBContextFactory.CreateMainDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }

                    if (template.ID == Guid.Empty)
                    {
                        template.ID = Guid.NewGuid();
                    }

                    template.CreateTime = DateTime.UtcNow;
                    template.ModifyTime = DateTime.UtcNow;

                    await dbContext.ScriptTemplates.AddAsync(template, cancellationToken);

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

                    var deleteObj = new ScriptTemplate() { ID = id };
                    dbContext.ScriptTemplates.Attach(deleteObj);
                    dbContext.ScriptTemplates.Remove(deleteObj);

                    await dbContext.SaveChangesAsync(cancellationToken);
                }
            });
        }

        public async Task Update(ScriptTemplate template, CancellationToken cancellationToken = default)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, false, false, _mainDBConnectionFactory.CreateAllForMain(), async (conn, transaction) =>
            {
                await using (var dbContext = _mainDBContextFactory.CreateMainDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }

                    template.ModifyTime = DateTime.UtcNow;
                    dbContext.ScriptTemplates.Attach(template);

                    var entry = dbContext.Entry(template);
                    foreach (var item in entry.Properties)
                    {
                        entry.Property(item.Metadata.Name).IsModified = true;
                    }
                    await dbContext.SaveChangesAsync(cancellationToken);
                }
            });
        }

        public async Task<ScriptTemplate?> QueryByID(Guid id, CancellationToken cancellationToken = default)
        {
            ScriptTemplate? result = null;
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, true, false, _mainDBConnectionFactory.CreateReadForMain(), async (conn, transaction) =>
            {
                await using (var dbContext = _mainDBContextFactory.CreateMainDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }

                    result = await (from item in dbContext.ScriptTemplates
                                    where item.ID == id
                                    select item).FirstOrDefaultAsync();
                }
            });

            return result;
        }

        public async Task<ScriptTemplate?> QueryByName(string name, CancellationToken cancellationToken = default)
        {
            ScriptTemplate? result = null;
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, true, false, _mainDBConnectionFactory.CreateReadForMain(), async (conn, transaction) =>
            {
                await using (var dbContext = _mainDBContextFactory.CreateMainDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }


                    result = await (from item in dbContext.ScriptTemplates
                                    where item.Name == name
                                    select item).FirstOrDefaultAsync();
                }
            });

            return result;
        }
    }
}
