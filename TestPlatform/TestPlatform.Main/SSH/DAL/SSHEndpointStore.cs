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
    }
}
