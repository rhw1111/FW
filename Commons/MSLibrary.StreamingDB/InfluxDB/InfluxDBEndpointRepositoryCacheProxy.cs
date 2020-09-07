using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.Cache;

namespace MSLibrary.StreamingDB.InfluxDB
{
    [Injection(InterfaceType = typeof(IInfluxDBEndpointRepositoryCacheProxy), Scope = InjectionScope.Singleton)]
    public class InfluxDBEndpointRepositoryCacheProxy : IInfluxDBEndpointRepositoryCacheProxy
    {
        public static KVCacheVisitorSetting KVCacheVisitorSetting = new KVCacheVisitorSetting()
        {
            Name = "_InfluxDBEndpointRepository",
            ExpireSeconds = 600,
            MaxLength = 500
        };

        private IInfluxDBEndpointRepository _influxDBEndpointRepository;

        public InfluxDBEndpointRepositoryCacheProxy(IInfluxDBEndpointRepository influxDBEndpointRepository)
        {
            _influxDBEndpointRepository = influxDBEndpointRepository;
            _kvcacheVisitor = CacheInnerHelper.CreateKVCacheVisitor(KVCacheVisitorSetting);
        }

        private KVCacheVisitor _kvcacheVisitor;


        public async Task<InfluxDBEndpoint?> QueryByName(string name, CancellationToken cancellationToken = default)
        {
            return (await _kvcacheVisitor.Get(
                async (k) =>
                {
                    var obj= await _influxDBEndpointRepository.QueryByName(name);
                    if (obj == null)
                    {
                        return (obj, false);
                    }
                    else
                    {
                        return (obj, true);
                    }
                },
                name
                )).Item1;
        }
    }
}
