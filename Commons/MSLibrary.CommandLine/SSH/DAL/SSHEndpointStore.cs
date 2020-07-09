using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MSLibrary.CommandLine.DAL;
using MSLibrary.Transaction;

namespace MSLibrary.CommandLine.SSH.DAL
{
    public class SSHEndpointStore : ISSHEndpointStore
    {
        private readonly ICommandLineConnectionFactory _dbConnectionFactory;
        private readonly ICommandLineEntityDBContextFactory _commandLineEntityDBContextFactory;

        public SSHEndpointStore(ICommandLineConnectionFactory dbConnectionFactory, ICommandLineEntityDBContextFactory commandLineEntityDBContextFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _commandLineEntityDBContextFactory = commandLineEntityDBContextFactory;
        }

        public Task Add(SSHEndpoint source, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task Delete(Guid id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task DeleteMutiple(List<Guid> ids, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<SSHEndpoint?> QueryByID(Guid id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<SSHEndpoint?> QueryByName(string name, CancellationToken cancellationToken = default)
        {
            SSHEndpoint? endpoint = null;
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _dbConnectionFactory.CreateReadForCommandLine(), async (conn, transaction) =>
            {

                await using (var dbContext = _commandLineEntityDBContextFactory.CreateCommandLineDBContext(conn))
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

        public Task<Guid?> QueryByNameNoLock(string name, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<QueryResult<SSHEndpoint>> QueryByPage(string matchName, int page, int pageSize, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task Update(SSHEndpoint source, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
