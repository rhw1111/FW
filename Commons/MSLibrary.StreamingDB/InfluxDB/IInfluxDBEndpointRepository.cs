using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MSLibrary.StreamingDB.InfluxDB
{
    /// <summary>
    /// InfluxDB终结点仓储
    /// </summary>
    public interface IInfluxDBEndpointRepository
    {
        Task<InfluxDBEndpoint?> QueryByName(string name, CancellationToken cancellationToken = default);
    }
}
