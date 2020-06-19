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
        public Task<InfluxDBEndpoint> QueryByName(string name, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
