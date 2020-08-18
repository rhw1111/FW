using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.Thread;
using MSLibrary.Logger;
using MSLibrary.StreamingDB.InfluxDB;
using MSLibrary.LanguageTranslate;
using Haukcode.PcapngUtils;
using Haukcode.PcapngUtils.Common;
using Haukcode.PcapngUtils.PcapNG;
using Haukcode.PcapngUtils.PcapNG.BlockTypes;
using System.Runtime.ExceptionServices;
using Haukcode.PcapngUtils.Pcap;
using Haukcode.PcapngUtils.Extensions;
using FW.TestPlatform.Main.Entities.DAL;
using Ctrade.Message;
using FW.TestPlatform.Main.Configuration;

namespace FW.TestPlatform.Main.NetGateway
{
    [Injection(InterfaceType = typeof(INetGatewayDataHandleService), Scope = InjectionScope.Singleton)]
    public class NetGatewayDataHandleService : INetGatewayDataHandleService
    {
        private static int _maxFileCount = 10;
        private readonly INetGatewayDataHandleConfigurationService _netGatewayDataHandleConfigurationService;
        private readonly IResolveFileNamePrefixService _resolveFileNamePrefixService;
        private readonly IGetSourceDataFromStreamService _getSourceDataFromStreamService;
        private readonly IGetSourceDataFromFileService _getSourceDataFromFileService;
        private readonly IConvertNetDataFromSourceService _convertNetDataFromSourceService;
        private readonly IQPSCollectService _qpsCollectService;
        private readonly INetDurationCollectService _netDurationCollectService;

        public static string LoggerCategoryName { get; set; } = "NetGatewayDataHandle";

        public NetGatewayDataHandleService(INetGatewayDataHandleConfigurationService netGatewayDataHandleConfigurationService, IResolveFileNamePrefixService resolveFileNamePrefixService, IGetSourceDataFromStreamService getSourceDataFromStreamService, IGetSourceDataFromFileService getSourceDataFromFileService, IConvertNetDataFromSourceService convertNetDataFromSourceService, IQPSCollectService qpsCollectService, INetDurationCollectService netDurationCollectService)
        {
            _netGatewayDataHandleConfigurationService = netGatewayDataHandleConfigurationService;
            _resolveFileNamePrefixService = resolveFileNamePrefixService;
            _getSourceDataFromStreamService = getSourceDataFromStreamService;
            _getSourceDataFromFileService = getSourceDataFromFileService;
            _convertNetDataFromSourceService = convertNetDataFromSourceService;
            _qpsCollectService = qpsCollectService;
            _netDurationCollectService = netDurationCollectService;
        }

        public async Task<INetGatewayDataHandleResult> Execute(CancellationToken cancellationToken = default)
        {
            List<Task> waitTasks = new List<Task>();
            Task resultTask = new Task(async () =>
            {
                foreach(var item in waitTasks)
                {
                    await item;
                }
            });

            var netGatewayDataHandleResult = new NetGatewayDataHandleResultDefault(resultTask);

            ConcurrentDictionary<string, ConcurrentDictionary<string, DataContainer>> restDatas = new ConcurrentDictionary<string, ConcurrentDictionary<string, DataContainer>>();     

            var folderPath = await _netGatewayDataHandleConfigurationService.GetDataFileFolderPath(cancellationToken);

            Dictionary<string,FileDataInfo> completedFiles = new Dictionary<string, FileDataInfo>();

            Dictionary<string,string> completedFileNames = new Dictionary<string, string>();

            var t1 = listenFileCompleted(netGatewayDataHandleResult, folderPath, completedFileNames, (info) =>
            {
                lock (completedFiles)
                {
                    completedFiles[info.FileName] = info;
                }
            });

            waitTasks.Add(t1);

            var t2 = Task.Run(async () =>
            {
                while (true)
                {
                    try
                    {
                        if (netGatewayDataHandleResult.IsStop)
                        {
                            break;
                        }

                        ConcurrentDictionary<string, ConcurrentDictionary<string, DataContainer>> datas = restDatas;
                        restDatas = new ConcurrentDictionary<string, ConcurrentDictionary<string, DataContainer>>();

                        ConcurrentDictionary<string, List<DataContainer>> singleResponseDatas = new ConcurrentDictionary<string, List<DataContainer>>();

                        var fileNames = (from item in completedFiles
                                        orderby item.Value.CreateTime
                                        select item.Value.FileName).Take(_maxFileCount).ToList();

                        await ParallelHelper.ForEach(fileNames, 10,
                            async (fileName) =>
                            {
                                string dataformat = string.Empty;
                                var prefix = _resolveFileNamePrefixService.Resolve(fileName, out dataformat);

                                if (!datas.TryGetValue(prefix, out ConcurrentDictionary<string, DataContainer>? containerDatas))
                                {
                                    lock (datas)
                                    {
                                        if (!datas.TryGetValue(prefix, out containerDatas))
                                        {
                                            containerDatas = new ConcurrentDictionary<string, DataContainer>();
                                            datas[prefix] = containerDatas;
                                        }
                                    }
                                }

                                //await using (var stream = File.OpenRead(fileName))
                                //{
                                //    await _getSourceDataFromStreamService.Get(stream,
                                //        async (sourceData) =>
                                //        {
                                //            var data = await _convertNetDataFromSourceService.Convert(prefix, sourceData, cancellationToken);

                                //            if (!containerDatas.TryGetValue(data.ID, out DataContainer? containerData))
                                //            {
                                //                lock (containerDatas)
                                //                {
                                //                    if (!containerDatas.TryGetValue(data.ID, out containerData))
                                //                    {
                                //                        if (data.Type == NetDataType.Request)
                                //                        {
                                //                            containerData = new DataContainer()
                                //                            {
                                //                                FileName = fileName
                                //                            };
                                //                            containerDatas[data.ID] = containerData;
                                //                        }
                                //                        else
                                //                        {
                                //                            if (!singleResponseDatas.TryGetValue(prefix, out List<DataContainer>? singleDatas))
                                //                            {
                                //                                lock (singleResponseDatas)
                                //                                {
                                //                                    if (!singleResponseDatas.TryGetValue(prefix, out singleDatas))
                                //                                    {
                                //                                        singleDatas = new List<DataContainer>();
                                //                                        singleResponseDatas[prefix] = singleDatas;
                                //                                    }
                                //                                }
                                //                            }

                                //                            lock (singleDatas)
                                //                            {
                                //                                singleDatas.Add(new DataContainer() { FileName = fileName, Response = data });
                                //                            }
                                //                        }
                                //                    }
                                //                }
                                //            }

                                //            if (containerData != null)
                                //            {
                                //                if (data.Type == NetDataType.Request)
                                //                {
                                //                    containerData.Request = data;
                                //                }
                                //                else
                                //                {
                                //                    containerData.Response = data;
                                //                }
                                //            }
                                //        },
                                //        cancellationToken
                                //    );

                                //    stream.Close();
                                //}

                                await _getSourceDataFromFileService.Get(fileName, dataformat,
                                    async (sourceData) =>
                                    {
                                        var data = await _convertNetDataFromSourceService.Convert(prefix, sourceData, cancellationToken);

                                        if (!containerDatas.TryGetValue(data.ID, out DataContainer? containerData))
                                        {
                                            lock (containerDatas)
                                            {
                                                if (!containerDatas.TryGetValue(data.ID, out containerData))
                                                {
                                                    if (data.Type == NetDataType.Request)
                                                    {
                                                        containerData = new DataContainer()
                                                        {
                                                            FileName = fileName
                                                        };
                                                        containerDatas[data.ID] = containerData;
                                                    }
                                                    else
                                                    {
                                                        if (!singleResponseDatas.TryGetValue(prefix, out List<DataContainer>? singleDatas))
                                                        {
                                                            lock (singleResponseDatas)
                                                            {
                                                                if (!singleResponseDatas.TryGetValue(prefix, out singleDatas))
                                                                {
                                                                    singleDatas = new List<DataContainer>();
                                                                    singleResponseDatas[prefix] = singleDatas;
                                                                }
                                                            }
                                                        }

                                                        lock (singleDatas)
                                                        {
                                                            singleDatas.Add(new DataContainer() { FileName = fileName, Response = data });
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                        if (containerData != null)
                                        {
                                            if (data.Type == NetDataType.Request)
                                            {
                                                containerData.Request = data;
                                            }
                                            else
                                            {
                                                containerData.Response = data;
                                            }
                                        }
                                    },
                                    cancellationToken
                                );
                            }
                        );

                        //处理单独的响应数据
                        foreach (var item in singleResponseDatas)
                        {
                            var prefixDatas = datas[item.Key];
                            var deleteDatas = new List<DataContainer>();
                            foreach (var innerItem in item.Value)
                            {
                                if (prefixDatas.TryGetValue(innerItem.Response!.ID, out DataContainer? containerData))
                                {
                                    containerData.Response = innerItem.Response;
                                    deleteDatas.Add(innerItem);
                                }
                            }

                            foreach (var deleteItem in deleteDatas)
                            {
                                item.Value.Remove(deleteItem);
                            }

                            if (item.Value.Count > 0)
                            {
                                if (!restDatas.TryGetValue(item.Key, out ConcurrentDictionary<string, DataContainer>? restContainerDatas))
                                {
                                    restContainerDatas = new ConcurrentDictionary<string, DataContainer>();
                                    restDatas[item.Key] = restContainerDatas;
                                }

                                foreach (var restItem in item.Value)
                                {
                                    restContainerDatas[restItem.Response!.ID] = restItem;
                                }

                            }
                        }

                        //计算
                        await ParallelHelper.ForEach(fileNames, 10,
                            async (fileName) =>
                            {
                                string dataformat = string.Empty;
                                var prefix = _resolveFileNamePrefixService.Resolve(fileName, out dataformat);

                                if (!datas.TryGetValue(prefix, out ConcurrentDictionary<string, DataContainer>? containerDatas))
                                {
                                    lock (datas)
                                    {
                                        if (!datas.TryGetValue(prefix, out containerDatas))
                                        {
                                            containerDatas = new ConcurrentDictionary<string, DataContainer>();
                                            datas[prefix] = containerDatas;
                                        }
                                    }
                                }

                                var calculateDatas = from item in containerDatas
                                                    where item.Value.FileName == fileName && item.Value.Request != null
                                                    select item.Value;

                                DateTime? maxCreateTime = null;
                                var qps = 0;
                                var totalRequestCount = calculateDatas.Count();
                                if (totalRequestCount > 1)
                                {
                                    var minCreateTime = calculateDatas.Min((v) => v.Request!.CreateTime);
                                    maxCreateTime = calculateDatas.Max((v) => v.Request!.CreateTime);
                                    qps = (int)(totalRequestCount / (maxCreateTime.Value - minCreateTime).TotalSeconds);
                                }
                                else if (totalRequestCount == 1)
                                {
                                    qps = 1;
                                    maxCreateTime = calculateDatas.Max((v) => v.Request!.CreateTime);
                                }

                                if (qps != 0)
                                {
                                    await _qpsCollectService.Collect(prefix,qps, maxCreateTime!.Value, cancellationToken);
                                }


                                var calculateResponseDatas = from item in containerDatas
                                                            where item.Value.FileName == fileName && item.Value.Request != null && item.Value.Response != null
                                                            select (item.Value.Response!.CreateTime - item.Value.Request!.CreateTime).TotalMilliseconds;

                                maxCreateTime = (from item in containerDatas
                                                where item.Value.FileName == fileName && item.Value.Request != null && item.Value.Response != null
                                                orderby item.Value.Response!.CreateTime descending
                                                select item.Value.Response!.CreateTime).FirstOrDefault();

                                var avgResponse = calculateResponseDatas.Average();
                                var maxResponse = calculateResponseDatas.Max();
                                var minResponse = calculateResponseDatas.Min();

                                if (maxCreateTime != null)
                                {
                                    await _netDurationCollectService.Collect(prefix,minResponse, maxResponse, avgResponse, maxCreateTime!.Value, cancellationToken);
                                }

                                //处理只有request的数据

                                if (!restDatas.TryGetValue(prefix, out ConcurrentDictionary<string, DataContainer>? restContainerDatas))
                                {
                                    lock (restDatas)
                                    {
                                        if (!restDatas.TryGetValue(prefix, out restContainerDatas))
                                        {
                                            restContainerDatas = new ConcurrentDictionary<string, DataContainer>();
                                            restDatas[prefix] = restContainerDatas;
                                        }
                                    }
                                }
                                  
                                var requestDatas = (from item in containerDatas
                                                    where item.Value.Request != null && item.Value.Response == null
                                                    select item).ToList();

                                foreach (var item in requestDatas)
                                {
                                    if (!restContainerDatas.TryGetValue(item.Key, out DataContainer? dataContainer))
                                    {
                                        lock (restContainerDatas)
                                        {
                                            if (!restContainerDatas.TryGetValue(item.Key, out dataContainer))
                                            {
                                                dataContainer = new DataContainer();
                                                restContainerDatas[item.Key] = dataContainer;
                                            }
                                        }
                                    }

                                    dataContainer.FileName = fileName;
                                    dataContainer.Request = item.Value.Request;
                                }
                            }
                        );

                        //删除用过的文件
                        foreach (var item in fileNames)
                        {
                            File.Delete(item);

                            lock (completedFiles)
                            {
                                completedFiles.Remove(item);
                            }
                        }

                        if (netGatewayDataHandleResult.IsStop)
                        {
                            break;
                        }

                        //await Task.Delay(10000);
                    }
                    catch (Exception ex)
                    {
                        LoggerHelper.LogError(LoggerCategoryName, ex.ToStackTraceString());
                        await Task.Delay(10000);
                    }
                }
            });

            waitTasks.Add(t2);

            resultTask.Start();

            return netGatewayDataHandleResult;
        }

        private Task listenFileCompleted(NetGatewayDataHandleResultDefault result, string folderName, Dictionary<string, string> completedFileNames, Action<FileDataInfo> completedAction)
        {
            return Task.Run(async () =>
            {
                while (true)
                {
                    try
                    {
                        if (result.IsStop)
                        {
                            break;
                        }

                        var fileNames = Directory.GetFiles(folderName);

                        List<FileDataInfo> fileDatas = new List<FileDataInfo>();

                        foreach (var item in fileNames)
                        {
                            var fileInfo = new FileInfo(item);
                            fileDatas.Add(new FileDataInfo() { CreateTime = fileInfo.CreationTimeUtc, FileName = fileInfo.FullName });
                        }

                        fileDatas = fileDatas.OrderBy((f) => f.CreateTime).ToList();

                        List<string> deleteFileNames = new List<string>();

                        foreach (var item in completedFileNames)
                        {
                            if (!fileNames.Contains(item.Key))
                            {
                                deleteFileNames.Add(item.Key);
                            }
                        }

                        foreach (var item in deleteFileNames)
                        {
                            completedFileNames.Remove(item);
                        }

                        foreach (var item in fileDatas)
                        {
                            if (!completedFileNames.ContainsKey(item.FileName))
                            {
                                FileInfo fileInfo = new FileInfo(item.FileName);

                                long old_length;

                                do
                                {
                                    old_length = fileInfo.Length;
                                    Thread.Sleep(3000);

                                } while (old_length != fileInfo.Length);

                                completedAction(new FileDataInfo() { FileName = fileInfo.FullName, CreateTime = fileInfo.CreationTimeUtc });
                                completedFileNames[item.FileName] = item.FileName;
                            }
                        }

                        if (result.IsStop)
                        {
                            break;
                        }

                        await Task.Delay(10000);
                    }
                    catch(Exception ex)
                    {
                        LoggerHelper.LogError(LoggerCategoryName, ex.ToStackTraceString());
                        await Task.Delay(10000);
                    }

                }
            });

            /*var watcher = new FileSystemWatcher();
            watcher.Path = folderName;
            watcher.NotifyFilter = NotifyFilters.Attributes |
                                   NotifyFilters.CreationTime |
                                   NotifyFilters.DirectoryName |
                                   NotifyFilters.FileName |
                                   NotifyFilters.Size;
            watcher.Filter = "*.cap";

            watcher.Created += new FileSystemEventHandler((o, e) =>
            {
                FileInfo fileInfo = new FileInfo(e.FullPath);

                long old_length;
                do
                {
                    old_length = fileInfo.Length;
                    Thread.Sleep(3000);

                } while (old_length != fileInfo.Length);

                completedAction(new FileDataInfo() { FileName= fileInfo.FullName, CreateTime= fileInfo.CreationTimeUtc });
            });

            watcher.EnableRaisingEvents = true;*/
        }

        private class DataContainer
        {
            public string FileName { get; set; } = null!;
            public NetData? Request { get; set; }
            public NetData? Response { get; set; }
        }

        private class FileDataInfo
        {
            public string FileName { get; set; } = null!;
            public DateTime CreateTime { get; set; }
        }
    }

    [Injection(InterfaceType = typeof(INetGatewayDataHandleConfigurationService), Scope = InjectionScope.Singleton)]
    public class NetGatewayDataHandleConfigurationService : INetGatewayDataHandleConfigurationService
    {
        /// <summary>
        /// 获取要监控处理的文件目录
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        async Task<string> INetGatewayDataHandleConfigurationService.GetDataFileFolderPath(CancellationToken cancellationToken)
        {
            var systemConfigurationService = DIContainerContainer.Get<ISystemConfigurationService>();
            var netGatewayDataFolder = await systemConfigurationService.GetNetGatewayDataFolderAsync();

            return netGatewayDataFolder;
        }
    }

    [Injection(InterfaceType = typeof(IGetSourceDataFromStreamService), Scope = InjectionScope.Singleton)]
    public class GetSourceDataFromStreamService : IGetSourceDataFromStreamService
    {
        /// <summary>
        /// 获取源数据字符串
        /// sourceDataAction中可以得到从流中获取的每个源数据字符串
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="dataformat"></param>
        /// <param name="sourceDataAction"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task IGetSourceDataFromStreamService.Get(Stream stream, string dataformat, Func<string, Task> sourceDataAction, CancellationToken cancellationToken)
        {
            if (stream == null)
            {
                return Task.CompletedTask;
            }

            using (PcapNGReader reader = new PcapNGReader(stream, false))
            {
                reader.OnReadPacketEvent += (context, packet) =>
                {
                    IPacket ipacket = packet;

                    //解析出基本包  
                    var ethernetPacket = PacketDotNet.Packet.ParsePacket(PacketDotNet.LinkLayers.Ethernet, packet.Data);

                    var payloadPacket = ethernetPacket;

                    while (payloadPacket.HasPayloadPacket)
                    {
                        payloadPacket = payloadPacket.PayloadPacket;
                    }

                    var payloadData = payloadPacket.PayloadData;

                    sourceDataAction.Invoke(payloadData.ToString());
                };
                reader.ReadPackets(cancellationToken);
            }

            return Task.CompletedTask;
        }
    }

    [Injection(InterfaceType = typeof(IGetSourceDataFromFileService), Scope = InjectionScope.Singleton)]
    public class GetSourceDataFromFileService : IGetSourceDataFromFileService
    {
        /// <summary>
        /// 获取源数据字符串
        /// sourceDataAction中可以得到从流中获取的每个源数据字符串
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="dataformat"></param>
        /// <param name="sourceDataAction"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task IGetSourceDataFromFileService.Get(string fileName, string dataformat, Func<string, Task> sourceDataAction, CancellationToken cancellationToken)
        {
            if (!File.Exists(fileName) || string.IsNullOrEmpty(dataformat))
            {
                return Task.CompletedTask;
            }

            //this.OpenPcapORPcapNFFile(fileName, dataformat, sourceDataAction, cancellationToken);
            this.OpenPcapORPcapNFFile2(fileName, dataformat, sourceDataAction, cancellationToken);

            return Task.CompletedTask;
        }

        public void OpenPcapORPcapNFFile(string fileName, string dataformat, Func<string, Task> sourceDataAction, CancellationToken token = default)
        {
            using (var reader = IReaderFactory.GetReader(fileName))
            {
                reader.OnReadPacketEvent += (context, packet) =>
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

                        try 
                        {
                            var payloadData = payloadPacket.PayloadData;

                            var requestType = 0;
                            var googleData = this.GetGoogleData(payloadData, out requestType);

                            if (googleData != null)
                            {
                                object data = string.Empty;

                                switch (dataformat)
                                {
                                    case NetGatewayDataFormatTypes.APICreditUpdateReplyMsg:
                                        data = APICreditUpdateReplyMsg.Parser.ParseFrom(googleData);

                                        break;
                                    case NetGatewayDataFormatTypes.APICreditUpdateRequestMsg:
                                        data = APICreditUpdateRequestMsg.Parser.ParseFrom(googleData);

                                        break;
                                    case NetGatewayDataFormatTypes.ApiListMarketDataAck:
                                        data = ApiListMarketDataAck.Parser.ParseFrom(googleData);

                                        break;
                                    case NetGatewayDataFormatTypes.ApiMarketData:
                                        data = ApiMarketData.Parser.ParseFrom(googleData);

                                        break;
                                    case NetGatewayDataFormatTypes.ApiMarketDataRequest:
                                        data = ApiMarketDataRequest.Parser.ParseFrom(googleData);

                                        break;
                                    case NetGatewayDataFormatTypes.APIOcoOrderCancelReplyMsg:
                                        data = APIOcoOrderCancelReplyMsg.Parser.ParseFrom(googleData);

                                        break;
                                    case NetGatewayDataFormatTypes.APIOcoOrderCancelRequestMsg:
                                        data = APIOcoOrderCancelRequestMsg.Parser.ParseFrom(googleData);

                                        break;
                                    case NetGatewayDataFormatTypes.APIOcoOrderSumitReplyMsg:
                                        data = APIOcoOrderSumitReplyMsg.Parser.ParseFrom(googleData);

                                        break;
                                    case NetGatewayDataFormatTypes.APIOcoOrderSumitRequestMsg:
                                        data = APIOcoOrderSumitRequestMsg.Parser.ParseFrom(googleData);

                                        break;
                                    case NetGatewayDataFormatTypes.APIOrderCancelReplyMsg:
                                        data = APIOrderCancelReplyMsg.Parser.ParseFrom(googleData);

                                        break;
                                    case NetGatewayDataFormatTypes.APIOrderCancelRequestMsg:
                                        data = APIOrderCancelRequestMsg.Parser.ParseFrom(googleData);

                                        break;
                                    case NetGatewayDataFormatTypes.APIOrderSubmitReplyMsg:
                                        data = APIOrderSubmitReplyMsg.Parser.ParseFrom(googleData);

                                        break;
                                    case NetGatewayDataFormatTypes.APIOrderSubmitRequestMsg:
                                        data = APIOrderSubmitRequestMsg.Parser.ParseFrom(googleData);

                                        break;
                                    case NetGatewayDataFormatTypes.BridgeOrderSubmitRequestMsg:
                                        data = BridgeOrderSubmitRequestMsg.Parser.ParseFrom(googleData);

                                        break;
                                    case NetGatewayDataFormatTypes.TokenReplyMsg:
                                        data = TokenReplyMsg.Parser.ParseFrom(googleData);

                                        break;
                                    case NetGatewayDataFormatTypes.TokenRequestMsg:
                                        data = TokenRequestMsg.Parser.ParseFrom(googleData);

                                        break;
                                    case NetGatewayDataFormatTypes.EmptyMsg:
                                        data = EmptyMsg.Parser.ParseFrom(googleData);

                                        break;
                                    default:
                                        break;
                                }

                                if (!string.IsNullOrEmpty(data.ToString()))
                                {
                                    sourceDataAction.Invoke(string.Format("{0}|{1}|{2}|{3}", requestType, string.Empty, timestamp.ToOADate().ToString(), string.Empty, data.ToString())); ;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(string.Format("GoogleData Error, Exception: {0}. {1}", ex.Message, ex.StackTrace));
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(string.Format("OnReadPacketEvent Error, Exception: {0}. {1}", ex.Message, ex.StackTrace));
                    }
                };
                reader.ReadPackets(token);
            }
        }

        private void reader_OnReadPacketEvent(object context, IPacket packet)
        {
            try
            {
                DateTime timestamp = ConvertToDateTime(packet.Seconds.ToString(), packet.Microseconds.ToString());
                double d = timestamp.ToOADate();

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

                    switch (dataformat)
                    {
                        case NetGatewayDataFormatTypes.APICreditUpdateReplyMsg:
                            data = APICreditUpdateReplyMsg.Parser.ParseFrom(googleData);

                            break;
                        case NetGatewayDataFormatTypes.APICreditUpdateRequestMsg:
                            data = APICreditUpdateRequestMsg.Parser.ParseFrom(googleData);

                            break;
                        case NetGatewayDataFormatTypes.ApiListMarketDataAck:
                            data = ApiListMarketDataAck.Parser.ParseFrom(googleData);

                            break;
                        case NetGatewayDataFormatTypes.ApiMarketData:
                            data = ApiMarketData.Parser.ParseFrom(googleData);

                            break;
                        case NetGatewayDataFormatTypes.ApiMarketDataRequest:
                            data = ApiMarketDataRequest.Parser.ParseFrom(googleData);

                            break;
                        case NetGatewayDataFormatTypes.APIOcoOrderCancelReplyMsg:
                            data = APIOcoOrderCancelReplyMsg.Parser.ParseFrom(googleData);

                            break;
                        case NetGatewayDataFormatTypes.APIOcoOrderCancelRequestMsg:
                            data = APIOcoOrderCancelRequestMsg.Parser.ParseFrom(googleData);

                            break;
                        case NetGatewayDataFormatTypes.APIOcoOrderSumitReplyMsg:
                            data = APIOcoOrderSumitReplyMsg.Parser.ParseFrom(googleData);

                            break;
                        case NetGatewayDataFormatTypes.APIOcoOrderSumitRequestMsg:
                            data = APIOcoOrderSumitRequestMsg.Parser.ParseFrom(googleData);

                            break;
                        case NetGatewayDataFormatTypes.APIOrderCancelReplyMsg:
                            data = APIOrderCancelReplyMsg.Parser.ParseFrom(googleData);

                            break;
                        case NetGatewayDataFormatTypes.APIOrderCancelRequestMsg:
                            data = APIOrderCancelRequestMsg.Parser.ParseFrom(googleData);

                            break;
                        case NetGatewayDataFormatTypes.APIOrderSubmitReplyMsg:
                            data = APIOrderSubmitReplyMsg.Parser.ParseFrom(googleData);

                            break;
                        case NetGatewayDataFormatTypes.APIOrderSubmitRequestMsg:
                            data = APIOrderSubmitRequestMsg.Parser.ParseFrom(googleData);

                            break;
                        case NetGatewayDataFormatTypes.BridgeOrderSubmitRequestMsg:
                            data = BridgeOrderSubmitRequestMsg.Parser.ParseFrom(googleData);

                            break;
                        case NetGatewayDataFormatTypes.TokenReplyMsg:
                            data = TokenReplyMsg.Parser.ParseFrom(googleData);

                            break;
                        case NetGatewayDataFormatTypes.TokenRequestMsg:
                            data = TokenRequestMsg.Parser.ParseFrom(googleData);

                            break;
                        case NetGatewayDataFormatTypes.EmptyMsg:
                            data = EmptyMsg.Parser.ParseFrom(googleData);

                            break;
                        default:
                            break;
                    }

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

            binaryReader.BaseStream.Position = 0;
            SectionHeader Header = SectionHeader.Parse(binaryReader);

            uint secs, usecs, caplen, len;
            long position = 0;
            byte[] data;

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

        private List<string> sourceAddressList = new List<string>();
        private List<string> destinationAddressList = new List<string>();

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

                try
                {
                    var payloadData = payloadPacket.PayloadData;

                    var requestType = 0;
                    var googleData = this.GetGoogleData(payloadData, out requestType);

                    if (googleData != null)
                    {
                        object data = string.Empty;

                        switch (dataformat)
                        {
                            case NetGatewayDataFormatTypes.APICreditUpdateReplyMsg:
                                data = APICreditUpdateReplyMsg.Parser.ParseFrom(googleData);

                                break;
                            case NetGatewayDataFormatTypes.APICreditUpdateRequestMsg:
                                data = APICreditUpdateRequestMsg.Parser.ParseFrom(googleData);

                                break;
                            case NetGatewayDataFormatTypes.ApiListMarketDataAck:
                                data = ApiListMarketDataAck.Parser.ParseFrom(googleData);

                                break;
                            case NetGatewayDataFormatTypes.ApiMarketData:
                                data = ApiMarketData.Parser.ParseFrom(googleData);

                                break;
                            case NetGatewayDataFormatTypes.ApiMarketDataRequest:
                                data = ApiMarketDataRequest.Parser.ParseFrom(googleData);

                                break;
                            case NetGatewayDataFormatTypes.APIOcoOrderCancelReplyMsg:
                                data = APIOcoOrderCancelReplyMsg.Parser.ParseFrom(googleData);

                                break;
                            case NetGatewayDataFormatTypes.APIOcoOrderCancelRequestMsg:
                                data = APIOcoOrderCancelRequestMsg.Parser.ParseFrom(googleData);

                                break;
                            case NetGatewayDataFormatTypes.APIOcoOrderSumitReplyMsg:
                                data = APIOcoOrderSumitReplyMsg.Parser.ParseFrom(googleData);

                                break;
                            case NetGatewayDataFormatTypes.APIOcoOrderSumitRequestMsg:
                                data = APIOcoOrderSumitRequestMsg.Parser.ParseFrom(googleData);

                                break;
                            case NetGatewayDataFormatTypes.APIOrderCancelReplyMsg:
                                data = APIOrderCancelReplyMsg.Parser.ParseFrom(googleData);

                                break;
                            case NetGatewayDataFormatTypes.APIOrderCancelRequestMsg:
                                data = APIOrderCancelRequestMsg.Parser.ParseFrom(googleData);

                                break;
                            case NetGatewayDataFormatTypes.APIOrderSubmitReplyMsg:
                                data = APIOrderSubmitReplyMsg.Parser.ParseFrom(googleData);

                                break;
                            case NetGatewayDataFormatTypes.APIOrderSubmitRequestMsg:
                                data = APIOrderSubmitRequestMsg.Parser.ParseFrom(googleData);

                                break;
                            case NetGatewayDataFormatTypes.BridgeOrderSubmitRequestMsg:
                                data = BridgeOrderSubmitRequestMsg.Parser.ParseFrom(googleData);

                                break;
                            case NetGatewayDataFormatTypes.TokenReplyMsg:
                                data = TokenReplyMsg.Parser.ParseFrom(googleData);

                                break;
                            case NetGatewayDataFormatTypes.TokenRequestMsg:
                                data = TokenRequestMsg.Parser.ParseFrom(googleData);

                                break;
                            case NetGatewayDataFormatTypes.EmptyMsg:
                                data = EmptyMsg.Parser.ParseFrom(googleData);

                                break;
                            default:
                                break;
                        }

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
            catch (Exception ex)
            {
                throw new Exception(string.Format("OnReadPacket Error, Exception: {0}", ex.Message));
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
    }

    [Injection(InterfaceType = typeof(IResolveFileNamePrefixService), Scope = InjectionScope.Singleton)]
    public class ResolveFileNamePrefixService : IResolveFileNamePrefixService
    {
        string IResolveFileNamePrefixService.Resolve(string fileName, out string dataformat)
        {
            dataformat = string.Empty;

            if (string.IsNullOrEmpty(fileName))
            {
                return string.Empty;
            }

            fileName = Path.GetFileNameWithoutExtension(fileName);
            // 命名规则：01_{caseid}_{historyid}_{newguid}
            // 01为使用CaseHistory，需要通过historyid查询history，获取它的NetGatewayDataFormat属性，返回historyid和该属性
            string[] fileName_Split = fileName.Split("_");

            if (fileName_Split.Length == 4)
            {
                string type = fileName_Split[0];
                Guid caseID = new Guid(fileName_Split[1]);
                Guid historyID = new Guid(fileName_Split[2]);
                string newGuid = fileName_Split[3];

                switch (type)
                {
                    case "01":
                        dataformat = string.Empty;

                        var testCaseHistoryStore = DIContainerContainer.Get<ITestCaseHistoryStore>();
                        var testCaseHistory = testCaseHistoryStore.QueryByCase(caseID, historyID);

                        if (testCaseHistory.Result != null)
                        {
                            dataformat = testCaseHistory.Result.NetGatewayDataFormat;
                        }

                        return historyID.ToString();

                        break;
                    default:
                        break;
                }
            }

            return string.Empty;
        }
    }

    [Injection(InterfaceType = typeof(IConvertNetDataFromSourceService), Scope = InjectionScope.Singleton)]
    public class ConvertNetDataFromSourceService : IConvertNetDataFromSourceService
    {
        Task<NetData> IConvertNetDataFromSourceService.Convert(string prefix, string sourceData, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(sourceData))
            {
                return null;
            }

            string[] sourceData_split = sourceData.Split("|");

            if (sourceData_split.Length == 4)
            {
                NetData netData = new NetData();
                netData.Type = sourceData_split[0] == "0" ? NetDataType.Request : NetDataType.Response;
                netData.ID = prefix;
                //netData.CreateTime = DateTime.Now;
                netData.CreateTime = DateTime.FromOADate(double.Parse(sourceData_split[2]));
                netData.RunDuration = null;

                return Task.FromResult(netData);
            }

            return null;
        }
    }

    public class NetGatewayDataHandleResultDefault : INetGatewayDataHandleResult
    {
        private Task _action;
        public bool IsStop { get; private set; } = false;

        public NetGatewayDataHandleResultDefault(Task action)
        {
            _action = action;
        }

        public async Task Stop()
        {
            IsStop = true;
            await _action;
        }
    }

    [Injection(InterfaceType = typeof(IQPSCollectService), Scope = InjectionScope.Singleton)]
    public class QPSCollectService : IQPSCollectService
    {
        private readonly IInfluxDBEndpointRepository _influxDBEndpointRepository;

        public QPSCollectService(IInfluxDBEndpointRepository influxDBEndpointRepository)
        {
            _influxDBEndpointRepository = influxDBEndpointRepository;
        }

        public async Task Collect(string prefix, int qps, DateTime time, CancellationToken cancellationToken)
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
            influxDBRecord.MeasurementName = InfluxDBParameters.NetGatewaySlaveMeasurementName;
            TimeSpan ts = time - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            influxDBRecord.Timestamp = Convert.ToInt64(((long)(ts.TotalMilliseconds)).ToString().PadRight(19, '0'));
            influxDBRecord.Tags.Add("HistoryCaseID", prefix);
            influxDBRecord.Fields.Add("QPS", qps.ToString());
            await influxDBEndpoint.AddData(InfluxDBParameters.DBName, influxDBRecord);
        }
    }

    [Injection(InterfaceType = typeof(INetDurationCollectService), Scope = InjectionScope.Singleton)]
    public class NetDurationCollectService : INetDurationCollectService
    {
        private readonly IInfluxDBEndpointRepository _influxDBEndpointRepository;

        public NetDurationCollectService(IInfluxDBEndpointRepository influxDBEndpointRepository)
        {
            _influxDBEndpointRepository = influxDBEndpointRepository;
        }

        public async Task Collect(string prefix,double min, double max, double avg, DateTime time, CancellationToken cancellationToken = default)
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
            influxDBRecord.MeasurementName = InfluxDBParameters.NetGatewayMasterMeasurementName;
            TimeSpan ts = time - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            influxDBRecord.Timestamp = Convert.ToInt64(((long)(ts.TotalMilliseconds)).ToString().PadRight(19, '0'));
            influxDBRecord.Tags.Add("HistoryCaseID", prefix);
            influxDBRecord.Fields.Add("MaxDuration", max.ToString());
            influxDBRecord.Fields.Add("MinDurartion", min.ToString());
            influxDBRecord.Fields.Add("AvgDuration", avg.ToString());
            await influxDBEndpoint.AddData(InfluxDBParameters.DBName, influxDBRecord);
        }
    }
}
