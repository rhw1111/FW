using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.StreamingDB.InfluxDB.DAL;

namespace MSLibrary.StreamingDB.InfluxDB
{
    [Injection(InterfaceType = typeof(IInfluxDBEndpointRepository), Scope = InjectionScope.Singleton)]
    public class InfluxDBEndpointRepository : IInfluxDBEndpointRepository
    {
        private readonly IInfluxDBEndpointStore _influxDBEndpointStore;

        public InfluxDBEndpointRepository(IInfluxDBEndpointStore influxDBEndpointStore)
        {
            _influxDBEndpointStore = influxDBEndpointStore;
        }
        public async Task<InfluxDBEndpoint?> QueryByName(string name, CancellationToken cancellationToken = default)
        {
            return await _influxDBEndpointStore.QueryByName(name, cancellationToken);
        }
    }
}
