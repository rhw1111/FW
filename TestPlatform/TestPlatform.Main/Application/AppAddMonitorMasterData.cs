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
    [Injection(InterfaceType = typeof(IAppAddMonitorMasterData), Scope = InjectionScope.Singleton)]
    public class AppAddMonitorMasterData : IAppAddMonitorMasterData
    {
        private readonly IInfluxDBEndpointRepository _influxDBEndpointRepository;

        public AppAddMonitorMasterData(IInfluxDBEndpointRepository influxDBEndpointRepository)
        {
            _influxDBEndpointRepository = influxDBEndpointRepository;
        }

        public async Task Do(MonitorMasterDataAddModel model, CancellationToken cancellationToken = default)
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

            InfluxDBRecord influxDBRecord = new InfluxDBRecord();
            influxDBRecord.MeasurementName = InfluxDBParameters.MasterMeasurementName;
            influxDBRecord.Tags.Add("CaseID", model.CaseID);
            influxDBRecord.Fields.Add("ConnectCount", model.ConnectCount);
            influxDBRecord.Fields.Add("ConnectFailCount", model.ConnectFailCount);
            influxDBRecord.Fields.Add("ReqCount", model.ReqCount);
            influxDBRecord.Fields.Add("ReqFailCount", model.ReqFailCount);
            influxDBRecord.Fields.Add("MaxDuration", model.MaxDuration);
            influxDBRecord.Fields.Add("MinDurartion", model.MinDurartion);
            influxDBRecord.Fields.Add("AvgDuration", model.AvgDuration);
            await influxDBEndpoint.AddData(InfluxDBParameters.DBName, influxDBRecord);
        }
    }
}
