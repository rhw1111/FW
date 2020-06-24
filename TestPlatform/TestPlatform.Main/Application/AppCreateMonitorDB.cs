using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.DI;
using FW.TestPlatform.Main.DTOModel;
using FW.TestPlatform.Main.Entities;
using MSLibrary.StreamingDB.InfluxDB;
using MSLibrary.LanguageTranslate;

namespace FW.TestPlatform.Main.Application
{
    [Injection(InterfaceType = typeof(IAppCreateMonitorDB), Scope = InjectionScope.Singleton)]
    public class AppCreateMonitorDB : IAppCreateMonitorDB
    {
        private readonly IInfluxDBEndpointRepository _influxDBEndpointRepository;

        public AppCreateMonitorDB(IInfluxDBEndpointRepository influxDBEndpointRepository)
        {
            _influxDBEndpointRepository = influxDBEndpointRepository;
        }

        public async Task Do(CancellationToken cancellationToken = default)
        {
            InfluxDBEndpoint? influxDBEndpoint = await _influxDBEndpointRepository.QueryByName(InfluxDBParameters.EndpointName);
            if (influxDBEndpoint == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.NotFoundInfluxDBEndpoint,
                    DefaultFormatting = "找不到指定名称{0}的InfluxDB数据源配置",
                    ReplaceParameters = new List<object>() { InfluxDBParameters.EndpointName }
                };

                throw new UtilityException((int)TestPlatformErrorCodes.NotFoundInfluxDBEndpoint, fragment, 1, 0);
            }

            await influxDBEndpoint.CreateDataBase(InfluxDBParameters.DBName);
        }
    }
}
