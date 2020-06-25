using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.StreamingDB.DAL;
using MSLibrary.StreamingDB.InfluxDB;
using MSLibrary.StreamingDB.InfluxDB.DAL;
using MSLibrary.Transaction;
using FW.TestPlatform.Main.DAL;

namespace FW.TestPlatform.Main.InfluxDB.DAL
{
    [Injection(InterfaceType = typeof(IInfluxDBEndpointStore), Scope = InjectionScope.Singleton)]
    public class InfluxDBEndpointStore : IInfluxDBEndpointStore
    {
        private readonly IStreamingDBConnectionFactory _streamingDBConnectionFactory;
        private readonly IMainDBContextFactory _mainDBContextFactory;

        public InfluxDBEndpointStore(IStreamingDBConnectionFactory streamingDBConnectionFactory, IMainDBContextFactory mainDBContextFactory)
        {
            _streamingDBConnectionFactory = streamingDBConnectionFactory;
            _mainDBContextFactory = mainDBContextFactory;
        }
        public async Task<InfluxDBEndpoint?> QueryByName(string name, CancellationToken cancellationToken = default)
        {
            InfluxDBEndpoint? result = null;
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, true, false, _streamingDBConnectionFactory.CreateReadForStreamingDB(), async (conn, transaction) =>
            {
                await using (var dbContext = _mainDBContextFactory.CreateConfigurationDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }


                    result = await (from item in dbContext.InfluxDBEndpoints
                                    where item.Name == name
                                    select item).FirstOrDefaultAsync();
                }
            });

            return result;
        }
    }
}
