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
    [Injection(InterfaceType = typeof(IAppAddMonitorSlaveData), Scope = InjectionScope.Singleton)]
    public class AppAddMonitorSlaveData : IAppAddMonitorSlaveData
    {
        private readonly IInfluxDBEndpointRepository _influxDBEndpointRepository;

        public AppAddMonitorSlaveData(IInfluxDBEndpointRepository influxDBEndpointRepository)
        {
            _influxDBEndpointRepository = influxDBEndpointRepository;
        }

        public async Task Do(IList<MonitorSlaveDataAddModel> modelList, CancellationToken cancellationToken = default)
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

            IList<InfluxDBRecord> influxDBRecordList = new List<InfluxDBRecord>();
            InfluxDBRecord influxDBRecord = null;
            foreach (MonitorSlaveDataAddModel model in modelList)
            {
                influxDBRecord = new InfluxDBRecord();
                influxDBRecord.MeasurementName = InfluxDBParameters.SlaveMeasurementName;
                influxDBRecord.Tags.Add("CaseID", model.CaseID);
                influxDBRecord.Fields.Add("QPS", model.QPS);
                influxDBRecord.Fields.Add("Time", model.Time);
                influxDBRecordList.Add(influxDBRecord);
            }
            await influxDBEndpoint.AddDatas(InfluxDBParameters.MasterMeasurementName, influxDBRecordList);

        }
    }
}
