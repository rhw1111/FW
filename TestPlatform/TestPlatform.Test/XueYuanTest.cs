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
using FW.TestPlatform.Main.NetGateway;
using Haukcode.PcapngUtils.PcapNG;
using MSLibrary.Schedule.DAL;
using Haukcode.PcapngUtils.PcapNG.BlockTypes;
using System.Runtime.ExceptionServices;
using Haukcode.PcapngUtils.Pcap;
using Haukcode.PcapngUtils.Extensions;

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
            var baseUrl = Path.GetDirectoryName(typeof(XueYuanTest).Assembly.Location);
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
        public async Task TestTestCaseWebSocketRun()
        {
            TestCase testCase = new TestCase()
            {
                ID = new Guid("82962da0-e83b-11ea-bf37-00ffb1d16cf9"),
            };

            var testCaseStore = DIContainerContainer.Get<ITestCaseStore>();
            var testCaseRunner = await testCaseStore.QueryByID(testCase.ID);

            if (testCaseRunner != null)
            {
                await testCaseRunner.Run();
            }

            Assert.Pass();
        }

        [Test]
        public async Task TestTestCaseJmeterRun()
        {
            TestCase testCase = new TestCase()
            {
                ID = new Guid("c9198a53-137b-11eb-bbfc-00ffb1d16cf9"),
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

        private List<string> sourceAddressList = new List<string>();
        private List<string> destinationAddressList = new List<string>();

        //[Test]
        public async Task TestCap()
        {
            //string fileName = "E:\\Documents\\Visual Studio Code\\TestPython\\pcapreader\\cap\\01_ce514456-8da9-432f-8999-1010fa94a83a_7fc391a7-dba0-11ea-b236-00ffb1d16cf9_20200729102651.cap";
            string fileName = "E:\\Documents\\Visual Studio Code\\TestPython\\pcapreader\\cap\\01_ce514456-8da9-432f-8999-1010fa94a83a_7fc391a7-dba0-11ea-b236-00ffb1d16cf9_230.3.33.20.cap";
            //this.OpenPcapORPcapNFFile(fileName);
            this.OpenPcapORPcapNFFile2(fileName, "", async (sourceData) => { });
        }

        private object syncRoot = new object();

        public void OpenPcapORPcapNFFile2(string fileName, string dataformat, Func<string, Task> sourceDataAction, CancellationToken cancellationToken = default)
        {
            using (var stream = File.OpenRead(fileName))
            //using (var stream = new FileStream(fileName, FileMode.Open))
            {
                using (BinaryReader binaryReader = new BinaryReader(stream))
                {
                    if (binaryReader.BaseStream.Length < 12)
                        throw new ArgumentException(string.Format("[IReaderFactory.GetReader] file {0} is too short ", fileName));

                    UInt32 mask = 0;
                    mask = binaryReader.ReadUInt32();

                    if (mask == (uint)BaseBlock.Types.SectionHeader)
                    {
                        binaryReader.ReadUInt32();
                        mask = binaryReader.ReadUInt32();
                    }

                    switch (mask)
                    {
                        case (uint)Haukcode.PcapngUtils.Pcap.SectionHeader.MagicNumbers.microsecondIdentical:
                        case (uint)Haukcode.PcapngUtils.Pcap.SectionHeader.MagicNumbers.microsecondSwapped:
                        case (uint)Haukcode.PcapngUtils.Pcap.SectionHeader.MagicNumbers.nanosecondSwapped:
                        case (uint)Haukcode.PcapngUtils.Pcap.SectionHeader.MagicNumbers.nanosecondIdentical:
                            this.ReadPackets_Pcap(binaryReader, dataformat, sourceDataAction, cancellationToken);

                            break;
                        case (uint)Haukcode.PcapngUtils.PcapNG.BlockTypes.SectionHeaderBlock.MagicNumbers.Identical:
                            this.ReadPackets_PcapNG(binaryReader, false, dataformat, sourceDataAction, cancellationToken);

                            break;
                        case (uint)Haukcode.PcapngUtils.PcapNG.BlockTypes.SectionHeaderBlock.MagicNumbers.Swapped:
                            this.ReadPackets_PcapNG(binaryReader, true, dataformat, sourceDataAction, cancellationToken);

                            break;
                        default:
                            throw new ArgumentException(string.Format("[IReaderFactory.GetReader] file {0} is not PCAP/PCAPNG file", fileName));
                    }
                }
            }
        }
        
        public void ReadPackets_Pcap(BinaryReader binaryReader, string dataformat, Func<string, Task> sourceDataAction, CancellationToken cancellationToken = default)
        {
            Action<Exception> ReThrowException = (exc) =>
            {
                ExceptionDispatchInfo.Capture(exc).Throw();
            };

            uint secs, usecs, caplen, len;
            long position = 0;
            byte[] data;
            SectionHeader Header = SectionHeader.Parse(binaryReader);

            while (binaryReader.BaseStream.Position < binaryReader.BaseStream.Length && !cancellationToken.IsCancellationRequested)
            {
                try
                {
                    lock (syncRoot)
                    {
                        position = binaryReader.BaseStream.Position;
                        secs = binaryReader.ReadUInt32().ReverseByteOrder(Header.ReverseByteOrder);
                        usecs = binaryReader.ReadUInt32().ReverseByteOrder(Header.ReverseByteOrder);

                        if (Header.NanoSecondResolution)
                        {
                            usecs = usecs / 1000;
                        }

                        caplen = binaryReader.ReadUInt32().ReverseByteOrder(Header.ReverseByteOrder);
                        len = binaryReader.ReadUInt32().ReverseByteOrder(Header.ReverseByteOrder);

                        data = binaryReader.ReadBytes((int)caplen);

                        if (data.Length < caplen)
                        {
                            throw new EndOfStreamException("Unable to read beyond the end of the stream");
                        }
                    }

                    PcapPacket packet = new PcapPacket((UInt64)secs, (UInt64)usecs, data, position);
                    this.OnReadPacket(packet, dataformat, sourceDataAction);
                }
                catch (Exception exc)
                {
                    ReThrowException(exc);
                }
            }
        }

        public void ReadPackets_PcapNG(BinaryReader binaryReader, bool reverseByteOrder, string dataformat, Func<string, Task> sourceDataAction, CancellationToken cancellationToken = default)
        {
            Action<Exception> ReThrowException = (exc) =>
            {
                ExceptionDispatchInfo.Capture(exc).Throw();
            };

            AbstractBlock block;
            long prevPosition = 0;
            binaryReader.BaseStream.Position = 0;

            while (binaryReader.BaseStream.Position < binaryReader.BaseStream.Length && !cancellationToken.IsCancellationRequested)
            {
                try
                {
                    lock (syncRoot)
                    {
                        prevPosition = binaryReader.BaseStream.Position;
                        block = AbstractBlockFactory.ReadNextBlock(binaryReader, reverseByteOrder, ReThrowException);
                    }

                    if (block == null)
                    {
                        throw new Exception(string.Format("[ReadPackets] AbstractBlockFactory cannot read packet on position {0}", prevPosition));
                    }

                    switch (block.BlockType)
                    {
                        case BaseBlock.Types.EnhancedPacket:
                            {
                                EnchantedPacketBlock enchantedBlock = block as EnchantedPacketBlock;

                                if (enchantedBlock == null)
                                {
                                    throw new Exception(string.Format("[ReadPackets] system cannot cast block to EnchantedPacketBlock. Block start on position: {0}.", prevPosition));
                                }
                                else
                                {
                                    this.OnReadPacket(enchantedBlock, dataformat, sourceDataAction);
                                }
                            }
                            break;
                        //case BaseBlock.Types.Packet:
                        //    {
                        //        PacketBlock packetBlock = block as PacketBlock;

                        //        if (packetBlock == null)
                        //        {
                        //            throw new Exception(string.Format("[ReadPackets] system cannot cast block to PacketBlock. Block start on position: {0}.", prevPosition));
                        //        }
                        //        else
                        //        {
                        //            this.OnReadPacket(packetBlock);
                        //        }
                        //    }
                        //    break;
                        //case BaseBlock.Types.SimplePacket:
                        //    {
                        //        SimplePacketBlock simpleBlock = block as SimplePacketBlock;

                        //        if (simpleBlock == null)
                        //        {
                        //            throw new Exception(string.Format("[ReadPackets] system cannot cast block to SimplePacketBlock. Block start on position: {0}.", prevPosition));
                        //        }
                        //        else
                        //        {
                        //            this.OnReadPacket(simpleBlock);
                        //        }
                        //    }
                        //    break;
                        default:
                            break;
                    }
                }
                catch (Exception exc)
                {
                    ReThrowException(exc);

                    lock (syncRoot)
                    {
                        if (prevPosition == binaryReader.BaseStream.Position)
                            break;
                    }

                    continue;
                }
            }
        }

        private void OnReadPacket(IPacket packet, string dataformat, Func<string, Task> sourceDataAction)
        {
            if (packet == null)
            {
                return;
            }

            try
            {
                DateTime timestamp = ConvertToDateTime(packet.Seconds.ToString(), packet.Microseconds.ToString());

                ////解析出基本包  
                var ethernetPacket = PacketDotNet.Packet.ParsePacket(PacketDotNet.LinkLayers.Ethernet, packet.Data);

                if (ethernetPacket == null)
                {
                    return;
                }

                var payloadPacket = ethernetPacket;
                bool isSourceAddress = false;
                bool isDestinationAddress = false;

                while (payloadPacket.HasPayloadPacket)
                {
                    payloadPacket = payloadPacket.PayloadPacket;

                    if (payloadPacket.GetType() == typeof(PacketDotNet.IPv4Packet))
                    {
                        var ipv4Packet = (PacketDotNet.IPv4Packet)payloadPacket;

                        if (sourceAddressList.Count == 0)
                        {
                            isSourceAddress = true;
                        }
                        else if (sourceAddressList.Contains(ipv4Packet.SourceAddress.ToString()))
                        {
                            isSourceAddress = true;
                        }

                        if (destinationAddressList.Count == 0)
                        {
                            isDestinationAddress = true;
                        }
                        else if (destinationAddressList.Contains(ipv4Packet.DestinationAddress.ToString()))
                        {
                            isDestinationAddress = true;
                        }
                    }
                }

                if (!isSourceAddress || !isDestinationAddress)
                {
                    return;
                }

                var payloadData = payloadPacket.PayloadData;

                var requestType = 0;
                var googleData = this.GetGoogleData(payloadData, out requestType);

                if (googleData != null)
                {
                    object data = string.Empty;

                    //switch (dataformat)
                    //{
                    //    case NetGatewayDataFormatTypes.APICreditUpdateReplyMsg:
                    //        data = APICreditUpdateReplyMsg.Parser.ParseFrom(googleData);

                    //        break;
                    //    case NetGatewayDataFormatTypes.APICreditUpdateRequestMsg:
                    //        data = APICreditUpdateRequestMsg.Parser.ParseFrom(googleData);

                    //        break;
                    //    case NetGatewayDataFormatTypes.ApiListMarketDataAck:
                    //        data = ApiListMarketDataAck.Parser.ParseFrom(googleData);

                    //        break;
                    //    case NetGatewayDataFormatTypes.ApiMarketData:
                    //        data = ApiMarketData.Parser.ParseFrom(googleData);

                    //        break;
                    //    case NetGatewayDataFormatTypes.ApiMarketDataRequest:
                    //        data = ApiMarketDataRequest.Parser.ParseFrom(googleData);

                    //        break;
                    //    case NetGatewayDataFormatTypes.APIOcoOrderCancelReplyMsg:
                    //        data = APIOcoOrderCancelReplyMsg.Parser.ParseFrom(googleData);

                    //        break;
                    //    case NetGatewayDataFormatTypes.APIOcoOrderCancelRequestMsg:
                    //        data = APIOcoOrderCancelRequestMsg.Parser.ParseFrom(googleData);

                    //        break;
                    //    case NetGatewayDataFormatTypes.APIOcoOrderSumitReplyMsg:
                    //        data = APIOcoOrderSumitReplyMsg.Parser.ParseFrom(googleData);

                    //        break;
                    //    case NetGatewayDataFormatTypes.APIOcoOrderSumitRequestMsg:
                    //        data = APIOcoOrderSumitRequestMsg.Parser.ParseFrom(googleData);

                    //        break;
                    //    case NetGatewayDataFormatTypes.APIOrderCancelReplyMsg:
                    //        data = APIOrderCancelReplyMsg.Parser.ParseFrom(googleData);

                    //        break;
                    //    case NetGatewayDataFormatTypes.APIOrderCancelRequestMsg:
                    //        data = APIOrderCancelRequestMsg.Parser.ParseFrom(googleData);

                    //        break;
                    //    case NetGatewayDataFormatTypes.APIOrderSubmitReplyMsg:
                    //        data = APIOrderSubmitReplyMsg.Parser.ParseFrom(googleData);

                    //        break;
                    //    case NetGatewayDataFormatTypes.APIOrderSubmitRequestMsg:
                    //        data = APIOrderSubmitRequestMsg.Parser.ParseFrom(googleData);

                    //        break;
                    //    case NetGatewayDataFormatTypes.BridgeOrderSubmitRequestMsg:
                    //        data = BridgeOrderSubmitRequestMsg.Parser.ParseFrom(googleData);

                    //        break;
                    //    case NetGatewayDataFormatTypes.TokenReplyMsg:
                    //        data = TokenReplyMsg.Parser.ParseFrom(googleData);

                    //        break;
                    //    case NetGatewayDataFormatTypes.TokenRequestMsg:
                    //        data = TokenRequestMsg.Parser.ParseFrom(googleData);

                    //        break;
                    //    case NetGatewayDataFormatTypes.EmptyMsg:
                    //        data = EmptyMsg.Parser.ParseFrom(googleData);

                    //        break;
                    //    default:
                    //        break;
                    //}

                    if (!string.IsNullOrEmpty(data.ToString()))
                    {
                        sourceDataAction.Invoke(string.Format("{0}|{1}|{2}|{3}", requestType, string.Empty, timestamp.ToOADate().ToString(), string.Empty, data.ToString())); ;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("GoogleData Error, Exception: {0}", ex.Message));
            }
        }

        public void OpenPcapORPcapNFFile(string fileName, CancellationToken token = default)
        {
            using (var reader = IReaderFactory.GetReader(fileName))
            {
                reader.OnReadPacketEvent += reader_OnReadPacketEvent;
                reader.ReadPackets(token);
                reader.OnReadPacketEvent -= reader_OnReadPacketEvent;
            }
        }

        void reader_OnReadPacketEvent(object context, IPacket packet)
        {
            try
            {
                DateTime timestamp = ConvertToDateTime(packet.Seconds.ToString(), packet.Microseconds.ToString());

                IPacket ipacket = packet;

                //解析出基本包  
                var ethernetPacket = PacketDotNet.Packet.ParsePacket(PacketDotNet.LinkLayers.Ethernet, packet.Data);

                if (ethernetPacket == null)
                {
                    return;
                }

                var payloadPacket = ethernetPacket;
                bool isSourceAddress = false;
                bool isDestinationAddress = false;

                while (payloadPacket.HasPayloadPacket)
                {
                    payloadPacket = payloadPacket.PayloadPacket;

                    if (payloadPacket.GetType() == typeof(PacketDotNet.IPv4Packet))
                    {
                        var ipv4Packet = (PacketDotNet.IPv4Packet)payloadPacket;

                        if (sourceAddressList.Count == 0)
                        {
                            isSourceAddress = true;
                        }
                        else if (sourceAddressList.Contains(ipv4Packet.SourceAddress.ToString()))
                        {
                            isSourceAddress = true;
                        }

                        if (destinationAddressList.Count == 0)
                        {
                            isDestinationAddress = true;
                        }
                        else if (destinationAddressList.Contains(ipv4Packet.DestinationAddress.ToString()))
                        {
                            isDestinationAddress = true;
                        }
                    }
                }

                if (!isSourceAddress || !isDestinationAddress)
                {
                    return;
                }

                var payloadData = payloadPacket.PayloadData;

                var requestType = 0;
                var googleData = this.GetGoogleData(payloadData, out requestType);

                if (googleData != null)
                {
                    object data = string.Empty;
                    string dataformat = string.Empty;

                    //switch (dataformat)
                    //{
                    //    case NetGatewayDataFormatTypes.APICreditUpdateReplyMsg:
                    //        data = APICreditUpdateReplyMsg.Parser.ParseFrom(googleData);

                    //        break;
                    //    case NetGatewayDataFormatTypes.APICreditUpdateRequestMsg:
                    //        data = APICreditUpdateRequestMsg.Parser.ParseFrom(googleData);

                    //        break;
                    //    case NetGatewayDataFormatTypes.ApiListMarketDataAck:
                    //        data = ApiListMarketDataAck.Parser.ParseFrom(googleData);

                    //        break;
                    //    case NetGatewayDataFormatTypes.ApiMarketData:
                    //        data = ApiMarketData.Parser.ParseFrom(googleData);

                    //        break;
                    //    case NetGatewayDataFormatTypes.ApiMarketDataRequest:
                    //        data = ApiMarketDataRequest.Parser.ParseFrom(googleData);

                    //        break;
                    //    case NetGatewayDataFormatTypes.APIOcoOrderCancelReplyMsg:
                    //        data = APIOcoOrderCancelReplyMsg.Parser.ParseFrom(googleData);

                    //        break;
                    //    case NetGatewayDataFormatTypes.APIOcoOrderCancelRequestMsg:
                    //        data = APIOcoOrderCancelRequestMsg.Parser.ParseFrom(googleData);

                    //        break;
                    //    case NetGatewayDataFormatTypes.APIOcoOrderSumitReplyMsg:
                    //        data = APIOcoOrderSumitReplyMsg.Parser.ParseFrom(googleData);

                    //        break;
                    //    case NetGatewayDataFormatTypes.APIOcoOrderSumitRequestMsg:
                    //        data = APIOcoOrderSumitRequestMsg.Parser.ParseFrom(googleData);

                    //        break;
                    //    case NetGatewayDataFormatTypes.APIOrderCancelReplyMsg:
                    //        data = APIOrderCancelReplyMsg.Parser.ParseFrom(googleData);

                    //        break;
                    //    case NetGatewayDataFormatTypes.APIOrderCancelRequestMsg:
                    //        data = APIOrderCancelRequestMsg.Parser.ParseFrom(googleData);

                    //        break;
                    //    case NetGatewayDataFormatTypes.APIOrderSubmitReplyMsg:
                    //        data = APIOrderSubmitReplyMsg.Parser.ParseFrom(googleData);

                    //        break;
                    //    case NetGatewayDataFormatTypes.APIOrderSubmitRequestMsg:
                    //        data = APIOrderSubmitRequestMsg.Parser.ParseFrom(googleData);

                    //        break;
                    //    case NetGatewayDataFormatTypes.BridgeOrderSubmitRequestMsg:
                    //        data = BridgeOrderSubmitRequestMsg.Parser.ParseFrom(googleData);

                    //        break;
                    //    case NetGatewayDataFormatTypes.TokenReplyMsg:
                    //        data = TokenReplyMsg.Parser.ParseFrom(googleData);

                    //        break;
                    //    case NetGatewayDataFormatTypes.TokenRequestMsg:
                    //        data = TokenRequestMsg.Parser.ParseFrom(googleData);

                    //        break;
                    //    case NetGatewayDataFormatTypes.EmptyMsg:
                    //        data = EmptyMsg.Parser.ParseFrom(googleData);

                    //        break;
                    //    default:
                    //        break;
                    //}

                    if (!string.IsNullOrEmpty(data.ToString()))
                    {
                        //sourceDataAction.Invoke(string.Format("{0}|{1}|{2}|{3}", requestType, string.Empty, string.Empty, string.Empty, data.ToString())); ;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("GoogleData Error, Exception: {0}", ex.Message));
            }
        }

        private byte[] GetGoogleData(byte[] data, out int requestType)
        {
            requestType = 0;

            if (data == null || data.Length < 82)
            {
                return null;
            }

            int packetType = data[0];
            int messageType = data[59];
            requestType = data[82];

            if (packetType == 3 && messageType == 7 && (requestType == 0 || requestType == 1))
            {
                byte[] length_osin_byte = data.Skip(87).Take(2).ToArray();
                int length_osin = Byte2Int(length_osin_byte);

                int dsp_begin = 89 + length_osin + 5;
                int dsp_end = dsp_begin + 4;
                byte[] length_dspm_byte = data.Skip(dsp_begin).Take(4).ToArray();
                int length_dspm = Byte4Int(length_dspm_byte);

                int dspapi_begin = 89 + length_osin;
                int dspapi_end = dspapi_begin + length_dspm;

                byte[] body = data.Skip(dspapi_end).ToArray();

                return body;
            }

            return null;
        }

        //2位byte转为int
        private int Byte2Int(byte[] b)
        {
            return ((b[0] & 0xff) << 8) | (b[1] & 0xff);
        }

        //4位byte转为int
        private int Byte4Int(byte[] b)
        {
            return ((b[0] & 0xff) << 24) | ((b[1] & 0xff) << 16) | ((b[2] & 0xff) << 8) | (b[3] & 0xff);
        }

        /// <summary>
        /// Unix时间戳转DateTime
        /// </summary>
        /// <param name="timestamp">时间戳</param>
        /// <returns></returns>
        public static DateTime ConvertToDateTime(string timestamp, string timestampMicroseconds)
        {
            DateTime time = DateTime.MinValue;
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));

            if (timestamp.Length == 10)        //精确到秒
            {
                time = startTime.AddSeconds(double.Parse(timestamp));
            }
            else if (timestamp.Length == 13)   //精确到毫秒
            {
                time = startTime.AddMilliseconds(double.Parse(timestamp));
            }

            double microseconds = double.Parse(timestampMicroseconds) / 1000000000;
            time = time.AddSeconds(microseconds);

            return time;
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

        //[Test]
        public async Task TestNetGateway()
        {
            //var netGatewayDataHandleService = DIContainerContainer.Get<INetGatewayDataHandleService>();

            //var netGatewayDataHandleResult = await netGatewayDataHandleService.Execute();
            //await netGatewayDataHandleResult.Stop();

            ////string fileName = "E:\\Documents\\Visual Studio Code\\TestPython\\pcapreader\\cap\\01_ce514456-8da9-432f-8999-1010fa94a83a_7fc391a7-dba0-11ea-b236-00ffb1d16cf9_20200729102651.cap";
            ////string fileName = "E:\\Documents\\Visual Studio Code\\TestPython\\pcapreader\\cap\\01_ce514456-8da9-432f-8999-1010fa94a83a_7fc391a7-dba0-11ea-b236-00ffb1d16cf9_230.3.33.20.cap";
            string dataformat = "TokenRequestMsg";

            string[] fileNames = Directory.GetFiles(@"E:\Documents\MyProjects\GitHub\xueyuangithub\FW\Document\cap\test");

            foreach (string fileName in fileNames)
            {
                var getSourceDataFromFileService = DIContainerContainer.Get<IGetSourceDataFromFileService>();

                await getSourceDataFromFileService.Get(fileName, dataformat, async (sourceData) => { });
            }
        }
    }
}
