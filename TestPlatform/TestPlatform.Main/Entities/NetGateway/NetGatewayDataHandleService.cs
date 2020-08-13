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
                                var prefix = _resolveFileNamePrefixService.Resolve(fileName);

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

                                //using (var reader = IReaderFactory.GetReader("E:\\Documents\\Visual Studio Code\\TestPython\\pcapreader\\cap\\7fc391a7-dba0-11ea-b236-00ffb1d16cf9_20200729102651.cap"))
                                //{
                                //    reader.OnReadPacketEvent += (context, packet) =>
                                //    {
                                //        IPacket ipacket = packet;

                                //        //解析出基本包  
                                //        var ethernetPacket = PacketDotNet.Packet.ParsePacket(PacketDotNet.LinkLayers.Ethernet, packet.Data);

                                //        var payloadPacket = ethernetPacket;

                                //        while (payloadPacket.HasPayloadPacket)
                                //        {
                                //            payloadPacket = payloadPacket.PayloadPacket;
                                //        }

                                //        var payloadData = payloadPacket.PayloadData;
                                //    };
                                //    reader.ReadPackets(cancellationToken);
                                //}

                                await _getSourceDataFromFileService.Get(fileName,
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
                                var prefix = _resolveFileNamePrefixService.Resolve(fileName);

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

                        await Task.Delay(10000);
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
        Task<string> INetGatewayDataHandleConfigurationService.GetDataFileFolderPath(CancellationToken cancellationToken)
        {
            string path = @"E:\Documents\Visual Studio Code\TestPython\pcapreader\cap";

            return Task.FromResult(path);
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
        /// <param name="sourceDataAction"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task IGetSourceDataFromStreamService.Get(Stream stream, Func<string, Task> sourceDataAction, CancellationToken cancellationToken)
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
        /// <param name="sourceDataAction"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task IGetSourceDataFromFileService.Get(string fileName, Func<string, Task> sourceDataAction, CancellationToken cancellationToken)
        {
            if (!File.Exists(fileName))
            {
                return Task.CompletedTask;
            }

            using (var reader = IReaderFactory.GetReader(fileName))
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

    [Injection(InterfaceType = typeof(IResolveFileNamePrefixService), Scope = InjectionScope.Singleton)]
    public class ResolveFileNamePrefixService : IResolveFileNamePrefixService
    {
        string IResolveFileNamePrefixService.Resolve(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return string.Empty;
            }

            fileName = Path.GetFileNameWithoutExtension(fileName);
            string prefix = fileName.Substring(0, fileName.IndexOf("_"));

            return prefix;
        }
    }

    [Injection(InterfaceType = typeof(IConvertNetDataFromSourceService), Scope = InjectionScope.Singleton)]
    public class ConvertNetDataFromSourceService : IConvertNetDataFromSourceService
    {
        Task<NetData> IConvertNetDataFromSourceService.Convert(string prefix, string sourceData, CancellationToken cancellationToken)
        {
            NetData netData = new NetData();
            netData.Type = NetDataType.Request;
            netData.ID = prefix;
            netData.CreateTime = DateTime.Now;
            netData.RunDuration = null;

            return Task.FromResult(netData);
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
