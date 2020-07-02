using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Unicode;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.IO;
using System.Globalization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MSLibrary;
using MSLibrary.Transaction;
using MSLibrary.DI;
using MSLibrary.Configuration;
using FW.TestPlatform.Main;
using FW.TestPlatform.Main.Configuration;
using FW.TestPlatform.Main.Entities;
using FW.TestPlatform.Main.Entities.DAL;
using FW.TestPlatform.Main.Application;
using FW.TestPlatform.Main.DTOModel;
using MSLibrary.StreamingDB.InfluxDB;
using MSLibrary.StreamingDB.InfluxDB.DAL;

namespace TestPlatform.Test
{
    public class JasonTest
    {
        private static AsyncLocal<Dictionary<string, string>> _connections = new AsyncLocal<Dictionary<string, string>>();
        [SetUp]
        public void Setup()
        {
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
            //设置编码，解决中文问题
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            //设置应用程序工作基目录
            var baseUrl = Path.GetDirectoryName(typeof(JasonTest).Assembly.Location);
            Environment.CurrentDirectory = baseUrl ?? Environment.CurrentDirectory;


            //初始化配置容器
            MainStartupHelper.InitConfigurationContainer(string.Empty, baseUrl);


            //获取核心配置
            var coreConfiguration = ConfigurationContainer.Get<ApplicationConfiguration>(ConfigurationNames.Application);

            //初始化上下文容器
            MainStartupHelper.InitContext();

            ServiceCollection services = new ServiceCollection();


            //初始化DI容器
            MainStartupHelper.InitDI(services, coreConfiguration.DISetting);


            //初始化静态设置
            MainStartupHelper.InitStaticInfo();


            //配置日志工厂
            var loggerFactory = LoggerFactory.Create((builder) =>
            {
                MainStartupHelper.InitLogger(builder);
            });

            DIContainerContainer.Inject<ILoggerFactory>(loggerFactory);
        }

        [Test]
        public async Task Test2()
        {
            //await QueryByCaseID("c7a290e6-eddd-4126-abc9-5e129718e0fc");
            //await AddTestCase();
           // await AddTestCaseHistory();
            //await CreateMonitorDB();
            //await AddMasterData();

            await AddSlaveData();
        }

        private async Task CreateMonitorDB()
        {

            IAppCreateMonitorDB test = DIContainerContainer.Get<IAppCreateMonitorDB>();


            await test.Do();
        }

        private async Task AddMasterData()
        {

            IAppAddMonitorMasterData test = DIContainerContainer.Get<IAppAddMonitorMasterData>();

            MonitorMasterDataAddModel model = null;

            Random rondom = new Random();

            int runCount = 1000;
            int i = 0;

            while (i <= runCount)
            {
                model = new MonitorMasterDataAddModel();
                i++;
                model.CaseID = "c7a290e6-eddd-4126-abc9-5e129718e0fc";
                model.ConnectCount = rondom.Next(0, 20000).ToString();
                model.ConnectFailCount = rondom.Next(0, 100).ToString();
                model.ReqCount = rondom.Next(0, 20000).ToString();
                model.ReqFailCount = rondom.Next(0, 100).ToString();
                model.AvgDuration = rondom.Next(0, 8).ToString();
                model.MaxDuration = rondom.Next(0, 10).ToString();
                model.MinDurartion = rondom.Next(0, 5).ToString();

                await test.Do(model);

                Thread.Sleep(2000);
            }
        }

        private async Task AddSlaveData()
        {

            IAppAddMonitorSlaveData test = DIContainerContainer.Get<IAppAddMonitorSlaveData>();

            IList<MonitorSlaveDataAddModel> modelList = new List<MonitorSlaveDataAddModel>();
            MonitorSlaveDataAddModel model = new MonitorSlaveDataAddModel();

            //model.CaseID = "c7a290e6-eddd-4126-abc9-5e129718e0fc";
            //model.QPS = "100";
            //model.Time = "20200623151201";
            //model.SlaveID = "aaaaa";
            //modelList.Add(model);

            //model = new MonitorSlaveDataAddModel();
            //model.CaseID = "c7a290e6-eddd-4126-abc9-5e129718e0fc";
            //model.QPS = "100";
            //model.Time = "20200623151301";
            //model.SlaveID = "aaaaa";
            //modelList.Add(model);

            //model = new MonitorSlaveDataAddModel();
            //model.CaseID = "c7a290e6-eddd-4126-abc9-5e129718e0fc";
            //model.QPS = "100";
            //model.Time = "20200623161201";
            //model.SlaveID = "aaaaa";
            //modelList.Add(model);

            Random rondom = new Random();

            DateTime dt = DateTime.UtcNow;

            int runCount = 500;
            int i = 0;

            while (i <= runCount)
            {
                i++;
                modelList = new List<MonitorSlaveDataAddModel>();
                model = new MonitorSlaveDataAddModel();

                model.CaseID = "c7a290e6-eddd-4126-abc9-5e129718e0fc";
                model.QPS = rondom.Next(10000, 30000).ToString();
                model.Time = dt.AddSeconds(0-i).ToString("yyyyMMddHHmmss");
                model.SlaveID = "Slave-" + i.ToString();
                modelList.Add(model); 
                await test.Do(modelList);
                Thread.Sleep(1000);

            }

            
        }

        private long ConvertToTimeStamp(string time)
        {
            DateTime dtTime = DateTime.ParseExact(time, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
            TimeSpan ts = dtTime - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds);
        }

        private async Task AddTestCase()
        {

            ITestCaseStore test = DIContainerContainer.Get<ITestCaseStore>();

            TestCase testcase = new TestCase();
            testcase.Name = "testcase1";
            testcase.EngineType = "111";
            testcase.Configuration = "222";
            testcase.MasterHostID = Guid.NewGuid();
            testcase.Status = TestCaseStatus.NoRun;
            testcase.OwnerID = Guid.NewGuid();
            testcase.CreateTime = DateTime.UtcNow;
            testcase.ModifyTime = DateTime.UtcNow;



            await test.Add(testcase);
        }

        private async Task<TestCase> QueryByCaseID(string id)
        {

            ITestCaseRepository test = DIContainerContainer.Get<ITestCaseRepository>();

            return await test.QueryByID(Guid.Parse(id));
        }

        private async Task AddTestCaseHistory()
        {

            IAppAddTestCaseHistory test = DIContainerContainer.Get<IAppAddTestCaseHistory>();

            TestCaseHistorySummyAddModel model = new TestCaseHistorySummyAddModel();
            model.CaseID = Guid.Parse("d4fdc4e2-4efd-4a1c-8372-5a6eca74e381");
            model.ConnectCount = 100;
            model.AvgDuration = 100;
            model.AvgQPS = 100;
            model.ConnectFailCount = 100;
            model.MaxDuration = 100;
            model.MaxQPS = 100;
            model.MinDurartion = 100;
            model.MinQPS = 100;
            model.ReqCount = 100;
            model.ReqFailCount = 100;

            await test.Do(model);
        }

    }
}