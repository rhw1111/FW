using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MSLibrary.StreamingDB.InfluxDB
{
    public interface IInfluxDBEndpointRepositoryCacheProxy
    {
        Task<InfluxDBEndpoint?> QueryByName(string name, CancellationToken cancellationToken = default);
    }
}
