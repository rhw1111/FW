using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.DI;
using FW.TestPlatform.Main.DTOModel;
using FW.TestPlatform.Main.Entities;
using System.Linq;
using System.Diagnostics.Tracing;
using MSLibrary.LanguageTranslate;
using MSLibrary.Serializer;
using MSLibrary.StreamingDB.InfluxDB;

namespace FW.TestPlatform.Main.Application
{
    [Injection(InterfaceType = typeof(IAppAddTestCaseHistory), Scope = InjectionScope.Singleton)]
    public class AppAddTestCaseHistory : IAppAddTestCaseHistory
    {
        private readonly ITestCaseRepository _testCaseRepository;
        private readonly IInfluxDBEndpointRepository _influxDBEndpointRepository;

        public AppAddTestCaseHistory(ITestCaseRepository testCaseRepository, IInfluxDBEndpointRepository influxDBEndpointRepository)
        {
            _testCaseRepository = testCaseRepository;
            _influxDBEndpointRepository = influxDBEndpointRepository;
        }

        public async Task Do(TestCaseHistorySummyAddModel model, CancellationToken cancellationToken = default)
        {
            TestCase? testCase = await _testCaseRepository.QueryByID(model.CaseID);
            if (testCase == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.NotFoundTestCaseByID,
                    DefaultFormatting = "找不到ID为{0}的测试案例",
                    ReplaceParameters = new List<object>() { model.CaseID }
                };
                throw new UtilityException((int)TestPlatformErrorCodes.NotFoundTestCaseByID, fragment, 1, 0);
            }
            if (testCase.TestCaseHistoryID == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.NotFoundTestCaseByID,
                    DefaultFormatting = "生成测试案例的历史记录，ID为{0}的测试案例，测试历史记录ID为空",
                    ReplaceParameters = new List<object>() { model.CaseID }
                };
                throw new UtilityException((int)TestPlatformErrorCodes.NotFoundTestCaseByID, fragment, 1, 0);
            }

            TestCaseHistory history = new TestCaseHistory();
            history.CaseID = model.CaseID;
            history.ID = (Guid)testCase.TestCaseHistoryID;
            history.Summary = JsonSerializerHelper.Serializer(model);
            history.NetGatewayDataFormat = string.Empty;
            await testCase.AddHistory(history);
        }

        public async Task DoForJmeter(TestCaseHistorySummyAddModel model, CancellationToken cancellationToken = default)
        {
            TestCase? testCase = await _testCaseRepository.QueryByID(model.CaseID);
            if (testCase == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.NotFoundTestCaseByID,
                    DefaultFormatting = "找不到ID为{0}的测试案例",
                    ReplaceParameters = new List<object>() { model.CaseID }
                };
                throw new UtilityException((int)TestPlatformErrorCodes.NotFoundTestCaseByID, fragment, 1, 0);
            }
            if (testCase.TestCaseHistoryID == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.NotFoundTestCaseByID,
                    DefaultFormatting = "生成测试案例的历史记录，ID为{0}的测试案例，测试历史记录ID为空",
                    ReplaceParameters = new List<object>() { model.CaseID }
                };
                throw new UtilityException((int)TestPlatformErrorCodes.NotFoundTestCaseByID, fragment, 1, 0);
            }

            TestCaseHistory history = new TestCaseHistory();
            history.CaseID = model.CaseID;
            history.ID = (Guid)testCase.TestCaseHistoryID;
            //从influxdb中汇总数据
            await SummaryData(model, testCase.TestCaseHistoryID);

            history.Summary = JsonSerializerHelper.Serializer(model);
            history.NetGatewayDataFormat = string.Empty;
            await testCase.AddHistory(history);
        }

        public async Task<TestCaseHistorySummyAddModel> SummaryData(TestCaseHistorySummyAddModel model,Guid? historyCaseID, CancellationToken cancellationToken = default)
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

            string sql = string.Format("select sum(count),sum(countError),mean(avg),min(min),max(max),mean(count)/5,max(count)/5,min(count)/5 from jmeter WHERE transaction = 'all' and  application = '{0}:{1}'", model.CaseID, historyCaseID);

            InfluxDBQueryData queryData = await influxDBEndpoint.GetData(InfluxDBParameters.DBName, sql, string.Empty);

            if (queryData != null && queryData.Results != null && queryData.Results.Count == 1 && queryData.Results[0].Series != null && queryData.Results[0].Series.Count == 1 && queryData.Results[0].Series[0].Columns.Count >= 9)
            {
                model.ReqCount = int.Parse(queryData.Results[0].Series[0].Values[0][1].Value.ToString()); // 请求数
                model.ReqFailCount = int.Parse(queryData.Results[0].Series[0].Values[0][2].Value.ToString());
                model.AvgDuration = float.Parse(queryData.Results[0].Series[0].Values[0][3].Value.ToString());
                model.MinDurartion = float.Parse(queryData.Results[0].Series[0].Values[0][4].Value.ToString());
                model.MaxDuration = float.Parse(queryData.Results[0].Series[0].Values[0][5].Value.ToString());
                model.AvgQPS = float.Parse(queryData.Results[0].Series[0].Values[0][6].Value.ToString());
                model.MaxQPS = float.Parse(queryData.Results[0].Series[0].Values[0][7].Value.ToString());
                model.MinQPS = float.Parse(queryData.Results[0].Series[0].Values[0][8].Value.ToString());
            }

            return model;
        }
    }
}
