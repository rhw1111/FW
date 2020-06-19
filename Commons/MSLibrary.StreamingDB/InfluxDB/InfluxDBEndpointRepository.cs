using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MSLibrary.StreamingDB.InfluxDB
{
    public class InfluxDBEndpointRepository : IInfluxDBEndpointRepository
    {
        public Task<InfluxDBEndpoint> QueryByName(string name, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
