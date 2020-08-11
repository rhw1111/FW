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
using MSLibrary.LanguageTranslate;
using MSLibrary.Template;
using MSLibrary.NetCap;
using Ctrade.Message;
using Haukcode.PcapngUtils;
using Haukcode.PcapngUtils.Common;
using PacketDotNet;

namespace TestPlatform.Test
{
    public class XueYuanTest
    {
        private static AsyncLocal<Dictionary<string, string>> _connections = new AsyncLocal<Dictionary<string, string>>();

        [SetUp]
        public void Setup()
        {
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
            //设置编码，解决中文问题
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            //设置应用程序工作基目录
            var baseUrl = Path.GetDirectoryName(typeof(RenHaoTest).Assembly.Location);
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

        //[Test]
        public async Task TestTestDataSourceAdd()
        {
            TestDataSource testDataSource = new TestDataSource()
            {
                ID = new Guid("dae3d35b-f618-47b9-b852-4ebee4b4e046"),
                Name = "dataSource1",
                Type = DataSourceTypes.Json,
                Data = "{}"
            };

            await testDataSource.Add();

            //var testDataSourceStore = DIContainerContainer.Get<ITestDataSourceStore>();

            //var newId = await testDataSourceStore.QueryByNameNoLock(testDataSource.Name);

            //if (newId != null)
            //{
            //    var fragment = new TextFragment()
            //    {
            //        Code = TestPlatformTextCodes.ExistTestDataSourceByName,
            //        DefaultFormatting = "已经存在名称为{0}的测试数据源",
            //        ReplaceParameters = new List<object>() { dataSource.Name }
            //    };

            //    throw new UtilityException((int)TestPlatformErrorCodes.ExistTestDataSourceByName, fragment, 1, 0);
            //}

            //await using (DBTransactionScope scope = new DBTransactionScope(System.Transactions.TransactionScopeOption.Required, new System.Transactions.TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = new TimeSpan(0, 0, 30) }))
            //{
            //    await testDataSourceStore.Add(dataSource);

            //    //检查是否有名称重复的
            //    newId = await testDataSourceStore.QueryByNameNoLock(testDataSource.Name);

            //    if (testDataSource.ID != newId)
            //    {
            //        var fragment = new TextFragment()
            //        {
            //            Code = TestPlatformTextCodes.ExistTestDataSourceByName,
            //            DefaultFormatting = "已经存在名称为{0}的测试数据源",
            //            ReplaceParameters = new List<object>() { dataSource.Name }
            //        };

            //        throw new UtilityException((int)TestPlatformErrorCodes.ExistTestDataSourceByName, fragment, 1, 0);

            //    }
            //    scope.Complete();
            //}

            Assert.Pass();
        }

        //[Test]
        public async Task TestTestCaseAdd()
        {
            TestCase testCase = new TestCase()
            {
                ID = new Guid("ce514456-8da9-432f-8999-1010fa94a83a"),
                MasterHostID = new Guid("822114cf-5277-4667-961f-e231f9e67e4d"),
                OwnerID = new Guid("46f8bcca-af6e-11ea-8e6a-0242ac110002"),
                EngineType = EngineTypes.Tcp,
                Name = "Case1",
                Configuration = "",
                Status = TestCaseStatus.NoRun
            };

            await testCase.Add();

            //var testCaseStore = DIContainerContainer.Get<ITestCaseStore>();
            //var testHostStore = DIContainerContainer.Get<ITestHostStore>();
            //TestHostRepository testHostRepository = new TestHostRepository(testHostStore);

            ////var handleService = getHandleService(tCase.EngineType);
            //var host = await testHostRepository.QueryByID(testCase.MasterHostID);
            //if (host == null)
            //{
            //    var fragment = new TextFragment()
            //    {
            //        Code = TestPlatformTextCodes.NotFoundTestHostByID,
            //        DefaultFormatting = "找不到Id为{0}的测试主机",
            //        ReplaceParameters = new List<object>() { testCase.MasterHostID.ToString() }
            //    };

            //    throw new UtilityException((int)TestPlatformErrorCodes.NotFoundTestHostByID, fragment, 1, 0);
            //}

            //await testCaseStore.Add(testCase);

            Assert.Pass();
        }


        //[Test]
        public async Task TestTestHostAdd()
        {
            TestHost testHost = new TestHost()
            {
                ID = new Guid("822114cf-5277-4667-961f-e231f9e67e4d"),
                Address = "127.0.0.1",
                SSHEndpointID = new Guid("1b846704-5449-4585-bb15-8b13388cb68b")
            };

            await testHost.Add();

            Assert.Pass();
        }

        //[Test]
        public async Task TestTestCaseSlaveHostAdd()
        {
            TestCaseSlaveHost testCaseSlaveHost = new TestCaseSlaveHost()
            {
                ID = new Guid("17c5a79c-0b05-4329-92fb-e83108d67831"),
                HostID = new Guid("822114cf-5277-4667-961f-e231f9e67e4d"),
                TestCaseID = new Guid("ce514456-8da9-432f-8999-1010fa94a83a"),
                SlaveName = "slave1",
                Count = 100,
                ExtensionInfo = ""
            };

            //await testCaseSlaveHost.Add();

            //Assert.Pass();
        }

        //[Test]
        public async Task TestScriptTemplateAdd()
        {
            ScriptTemplate scriptTemplate = new ScriptTemplate()
            {
                ID = new Guid("9adb8033-f28a-43a1-b396-0f36307b213b"),
                Name = "LocustTcp",
                Content = ""
            };

            await scriptTemplate.Add();

            Assert.Pass();
        }

        //[Test]
        public async Task TestTestCaseRun()
        {
            TestCase testCase = new TestCase()
            {
                ID = new Guid("ce514456-8da9-432f-8999-1010fa94a83a"),
            };

            var testCaseStore = DIContainerContainer.Get<ITestCaseStore>();
            var testCaseRunner = await testCaseStore.QueryByID(testCase.ID);

            if (testCaseRunner != null)
            {
                await testCaseRunner.Run();
            }

            Assert.Pass();
        }

        //[Test]
        public async Task TestTestCaseHttpRun()
        {
            TestCase testCase = new TestCase()
            {
                ID = new Guid("b4c2acd0-cd7a-11ea-852b-00ffb1d16cf9"),
            };

            var testCaseStore = DIContainerContainer.Get<ITestCaseStore>();
            var testCaseRunner = await testCaseStore.QueryByID(testCase.ID);

            if (testCaseRunner != null)
            {
                await testCaseRunner.Run();
            }

            Assert.Pass();
        }


        //[Test]
        public async Task TestTestCaseStop()
        {
            TestCase testCase = new TestCase()
            {
                ID = new Guid("ce514456-8da9-432f-8999-1010fa94a83a"),
            };

            var testCaseStore = DIContainerContainer.Get<ITestCaseStore>();
            var testCaseRunner = await testCaseStore.QueryByID(testCase.ID);

            if (testCaseRunner != null)
            {
                await testCaseRunner.Stop();
            }

            Assert.Pass();
        }

        //[Test]
        //public async Task GetLabelParameterHandlers()
        //{
        //    List<ILabelParameterHandler> list = new List<ILabelParameterHandler>();

        //    foreach (IFactory<ILabelParameterHandler> factory in LabelParameterIMP.HandlerFactories.Values)
        //    {
        //        var handler = factory.Create();

        //        if (await handler.IsOpenUser())
        //        {
        //            list.Add(handler);
        //        }

        //        string fromat = await handler.Formate();
        //    }

        //    list.Sort((x, y) => x.SerialNo.CompareTo(y.SerialNo));

        //    Assert.Pass();
        //}

        [Test]
        public async Task TestCap()
        {
            this.OpenPcapORPcapNFFile("E:\\Documents\\Visual Studio Code\\TestPython\\pcapreader\\cap\\abc_00000_20200729102649.cap");
        }

        public void OpenPcapORPcapNFFile(string filename, CancellationToken token = default)
        {
            using (var reader = IReaderFactory.GetReader(filename))
            {
                reader.OnReadPacketEvent += reader_OnReadPacketEvent;
                reader.ReadPackets(token);
                reader.OnReadPacketEvent -= reader_OnReadPacketEvent;
            }
        }

        void reader_OnReadPacketEvent(object context, IPacket packet)
        {
            Console.WriteLine(string.Format("Packet received {0}.{1}", packet.Seconds, packet.Microseconds));

            ////解析出基本包  
            var ethernetPacket = PacketDotNet.Packet.ParsePacket(PacketDotNet.LinkLayers.Ethernet, packet.Data);
            string ret = "";
            PrintPacket(ref ret, ethernetPacket);
            Console.WriteLine(string.Format("PayloadData {0}", ret));

            var payloadPacket = ethernetPacket;

            while (payloadPacket.HasPayloadPacket)
            {
                payloadPacket = payloadPacket.PayloadPacket;
            }

            var payloadData = payloadPacket.PayloadData;

            try
            {
                //var data = APIOrderCancelReplyMsg.Parser.ParseFrom(payloadData);
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>  
        /// 过滤条件关键字  
        /// </summary>  
        public string filter;

        /// <summary>  
        /// 打印包信息，组合包太复杂了，所以直接把hex字符串打出来了  
        /// </summary>  
        /// <param name="str"></param>  
        /// <param name="p"></param>  
        private void PrintPacket(ref string str, Packet p)
        {
            if (p != null)
            {
                string s = p.ToString();

                if (!string.IsNullOrEmpty(filter) && !s.Contains(filter))
                {
                    return;
                }

                str += "\r\n" + s + "\r\n";
                ////尝试创建新的TCP/IP数据包对象，  
                ////第一个参数为以太头长度，第二个为数据包数据块  
                str += p.PrintHex() + "\r\n";
            }
        }
    }
}
