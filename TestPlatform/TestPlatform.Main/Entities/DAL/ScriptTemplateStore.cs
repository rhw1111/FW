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


                    //result = await (from item in dbContext.TestCases
                    //                where item.Name == name
                    //                select item).FirstOrDefaultAsync();
                }
            });

            return result;
        }
    }
}
