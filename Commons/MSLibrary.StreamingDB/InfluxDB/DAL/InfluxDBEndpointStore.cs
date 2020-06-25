using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.Transaction;
using MSLibrary.StreamingDB.DAL;

namespace MSLibrary.StreamingDB.InfluxDB.DAL
{
    [Injection(InterfaceType = typeof(IInfluxDBEndpointStore), Scope = InjectionScope.Singleton)]
    public class InfluxDBEndpointStore : IInfluxDBEndpointStore
    {
        private readonly IStreamingDBConnectionFactory _streamingDBConnectionFactory;
        private readonly IStreamingDBEntityDBContextFactory _streamingDBEntityDBContextFactory;

        public InfluxDBEndpointStore(IStreamingDBConnectionFactory streamingDBConnectionFactory, IStreamingDBEntityDBContextFactory streamingDBEntityDBContextFactory)
        {
            _streamingDBConnectionFactory = streamingDBConnectionFactory;
            _streamingDBEntityDBContextFactory = streamingDBEntityDBContextFactory;
        }
        public async Task<InfluxDBEndpoint?> QueryByName(string name, CancellationToken cancellationToken = default)
        {
            InfluxDBEndpoint? result = null;
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, true, false, _streamingDBConnectionFactory.CreateReadForStreamingDB(), async (conn, transaction) =>
            {
                await using (var dbContext = _streamingDBEntityDBContextFactory.CreateStreamingDBDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }
                     //var aa = await dbContext.InfluxDBEndpoints.FirstOrDefaultAsync();
                    result = await (from item in dbContext.InfluxDBEndpoints
                                    //where item.Name == name
                                    select item).FirstOrDefaultAsync();
                }
            });

            return result;
        }
    }
}
