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
using MSLibrary.Configuration;
using FW.TestPlatform.Main.Entities;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.Internal;
using System.Data;
using System.Text.RegularExpressions;

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
        private readonly ITotalCollectService _totalCollectService;

        public static string LoggerCategoryName { get; set; } = "NetGatewayDataHandle";

        public NetGatewayDataHandleService(INetGatewayDataHandleConfigurationService netGatewayDataHandleConfigurationService, IResolveFileNamePrefixService resolveFileNamePrefixService, IGetSourceDataFromStreamService getSourceDataFromStreamService, IGetSourceDataFromFileService getSourceDataFromFileService, IConvertNetDataFromSourceService convertNetDataFromSourceService, IQPSCollectService qpsCollectService, INetDurationCollectService netDurationCollectService,ITotalCollectService totalCollectService)
        {
            _netGatewayDataHandleConfigurationService = netGatewayDataHandleConfigurationService;
            _resolveFileNamePrefixService = resolveFileNamePrefixService;
            _getSourceDataFromStreamService = getSourceDataFromStreamService;
            _getSourceDataFromFileService = getSourceDataFromFileService;
            _convertNetDataFromSourceService = convertNetDataFromSourceService;
            _qpsCollectService = qpsCollectService;
            _netDurationCollectService = netDurationCollectService;
            _totalCollectService = totalCollectService;
        }

        public async Task<INetGatewayDataHandleResult> Execute(CancellationToken cancellationToken = default)
        {
            var applicationConfiguration = ConfigurationContainer.Get<ApplicationConfiguration>(ConfigurationNames.Application);
            LoggerHelper.LogInformation($"{applicationConfiguration.ApplicationName}", $"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffff")}] {nameof(NetGatewayDataHandleService)} Execute has been called.");

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

            ConcurrentDictionary<string, ConcurrentDictionary<DateTime, QPSContainer>> qpsDatas = new ConcurrentDictionary<string, ConcurrentDictionary<DateTime, QPSContainer>>();

            var t1 = listenFileCompleted(netGatewayDataHandleResult, folderPath, completedFileNames, (infos) =>
            {
                lock (completedFiles)
                {
                    completedFiles.Merge(infos);
                }
            });

            waitTasks.Add(t1);

            var t2 = Task.Run(async () =>
            {
                LoggerHelper.LogInformation($"{applicationConfiguration.ApplicationName}", $"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffff")}] {nameof(NetGatewayDataHandleService)} t2 Task.Run.");

                while (true)
                {
                    LoggerHelper.LogInformation($"{applicationConfiguration.ApplicationName}", $"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffff")}] {nameof(NetGatewayDataHandleService)} t2 Task.Run while (true).");

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

                        if (fileNames.Count > 0)
                        {
                            LoggerHelper.LogInformation($"{applicationConfiguration.ApplicationName}", $"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffff")}] {nameof(NetGatewayDataHandleService)} t2 Task.Run ForEach fileNames. 找到{fileNames.Count}个文件：{string.Join(", ", fileNames)}.");
                        }
                        else
                        {
                            qpsDatas = new ConcurrentDictionary<string, ConcurrentDictionary<DateTime, QPSContainer>>(); ;
                            LoggerHelper.LogInformation($"{applicationConfiguration.ApplicationName}", $"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffff")}] {nameof(NetGatewayDataHandleService)} t2 Task.Run ForEach fileNames. 找到{fileNames.Count}个文件.");
                        }

                        #region 解析文件
                        await ParallelHelper.ForEach(fileNames, 10,
                            async (fileName) =>
                            {
                                try
                                {
                                    var testCaseHistory = await _resolveFileNamePrefixService.Resolve(fileName);

                                    string dataformat = string.Empty;
                                    var prefix = string.Empty;

                                    if (testCaseHistory != null)
                                    {
                                        dataformat = testCaseHistory.NetGatewayDataFormat;
                                        prefix = testCaseHistory.ID.ToString().ToLower();
                                    }
                                    else
                                    {
                                        return;
                                    }

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

                                    await _getSourceDataFromFileService.Get(fileName, dataformat,
                                        async (sourceData) =>
                                        {
                                            var data = await _convertNetDataFromSourceService.Convert(prefix, sourceData, cancellationToken);
                                            if (data != null)
                                            {
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
                                                                    FileName = fileName,
                                                                    Timestamp = data.CreateTime
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
                                                                    singleDatas.Add(new DataContainer() { FileName = fileName, Timestamp = data.CreateTime, Response = data });
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
                                            }
                                        },
                                        cancellationToken
                                    );
                                }
                                catch (Exception ex)
                                {
                                    LoggerHelper.LogError($"[{fileName}] {LoggerCategoryName}", ex.ToStackTraceString());
                                }
                            }
                        );
                        #endregion

                        if (fileNames.Count > 0)
                        {
                            LoggerHelper.LogInformation($"{applicationConfiguration.ApplicationName}", $"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffff")}] {nameof(NetGatewayDataHandleService)} t2 Task.Run 处理单独的响应数据.");
                        }

                        #region 处理单独的响应数据
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
                                else
                                {
                                    if (!restDatas.TryGetValue(item.Key, out ConcurrentDictionary<string, DataContainer>? restContainerDatas))
                                    {
                                        restContainerDatas = new ConcurrentDictionary<string, DataContainer>();
                                        restDatas[item.Key] = restContainerDatas;
                                    }

                                    restContainerDatas[innerItem.Response!.ID] = innerItem;
                                }
                            }

                            // 此处阀值用来判断，如果需要删除的数据太多，就不删了，是由于坏包太多所致
                            //if (deleteDatas.Count < 100)
                            //{
                            //    foreach (var deleteItem in deleteDatas)
                            //    {
                            //        item.Value.Remove(deleteItem);
                            //    }
                            //}

                            //if (item.Value.Count > 0)
                            //{
                            //    if (!restDatas.TryGetValue(item.Key, out ConcurrentDictionary<string, DataContainer>? restContainerDatas))
                            //    {
                            //        restContainerDatas = new ConcurrentDictionary<string, DataContainer>();
                            //        restDatas[item.Key] = restContainerDatas;
                            //    }

                            //    foreach (var restItem in item.Value)
                            //    {
                            //        restContainerDatas[restItem.Response!.ID] = restItem;
                            //    }

                            //}
                        }
                        #endregion

                        if (fileNames.Count > 0)
                        {
                            LoggerHelper.LogInformation($"{applicationConfiguration.ApplicationName}", $"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffff")}] {nameof(NetGatewayDataHandleService)} t2 Task.Run 计算.");
                        }

                        #region 计算
                        //计算
                        await ParallelHelper.ForEach(fileNames, 10,
                            async (fileName) =>
                            {
                                try
                                {
                                    var testCaseHistory = await _resolveFileNamePrefixService.Resolve(fileName);

                                    string dataformat = string.Empty;
                                    var prefix = string.Empty;

                                    if (testCaseHistory != null)
                                    {
                                        dataformat = testCaseHistory.NetGatewayDataFormat;
                                        prefix = testCaseHistory.ID.ToString().ToLower();
                                    }
                                    else
                                    {
                                        return;
                                    }

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

                                    //List<DateTime> times = new List<DateTime>();
                                    //List<int> counts = new List<int>();
                                    //List<double> avgQPSs = new List<double>();
                                    //List<double> avgDurations = new List<double>();
                                    //List<double> maxDurations = new List<double>();
                                    //List<double> minDurations = new List<double>();

                                    #region QPS
                                    //var calculateDatas = from item in containerDatas
                                    //                    where item.Value.FileName == fileName && item.Value.Request != null
                                    //                    select item.Value;

                                    //DateTime? maxCreateTime = null;
                                    //var qps = 0;
                                    //var totalRequestCount = calculateDatas.Count();
                                    //if (totalRequestCount > 1)
                                    //{
                                    //    var minCreateTime = calculateDatas.Min((v) => v.Request!.CreateTime);
                                    //    maxCreateTime = calculateDatas.Max((v) => v.Request!.CreateTime);
                                    //    qps = (int)(totalRequestCount / (maxCreateTime.Value - minCreateTime).TotalSeconds);
                                    //}
                                    //else if (totalRequestCount == 1)
                                    //{
                                    //    qps = 1;
                                    //    maxCreateTime = calculateDatas.Max((v) => v.Request!.CreateTime);
                                    //}

                                    //if (qps != 0)
                                    //{
                                    //    await _qpsCollectService.Collect(prefix,qps, maxCreateTime!.Value, cancellationToken);
                                    //}

                                    var calculateDatas = from item in containerDatas
                                                         where item.Value.FileName == fileName && item.Value.Request != null
                                                         group item by item.Value.Timestamp.ToString("yyyy-MM-dd HH:mm:ss") into g
                                                         select new { g.Key, Total = g.Count() };

                                    if (calculateDatas != null)
                                    {
                                        LoggerHelper.LogInformation($"{applicationConfiguration.ApplicationName}", $"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffff")}] {nameof(NetGatewayDataHandleService)} {fileName} QPS.Count = {calculateDatas.Count()}.");
                                    }

                                    foreach (var row in calculateDatas)
                                    {
                                        var qps = row.Total;
                                        DateTime maxCreateTime = Convert.ToDateTime(row.Key);

                                        //times.Add(maxCreateTime);
                                        //counts.Add(qps);
                                        //avgQPSs.Add(qps);

                                        if (!qpsDatas.TryGetValue(prefix, out ConcurrentDictionary<DateTime, QPSContainer>? qpsData))
                                        {
                                            lock (qpsDatas)
                                            {
                                                if (!qpsDatas.TryGetValue(prefix, out qpsData))
                                                {
                                                    qpsData = new ConcurrentDictionary<DateTime, QPSContainer>();
                                                    qpsDatas[prefix] = qpsData;
                                                    qpsData.TryAdd(maxCreateTime, new QPSContainer() { Prefix = prefix, Timestamp = maxCreateTime, QPS = qps });
                                                }
                                            }
                                        }
                                        else
                                        {
                                            lock (qpsData)
                                            {
                                                if (!qpsData.TryGetValue(maxCreateTime, out QPSContainer? qpsD))
                                                {
                                                    if (!qpsData.TryGetValue(maxCreateTime, out qpsD))
                                                    {
                                                        qpsD = new QPSContainer() { Prefix = prefix, Timestamp = maxCreateTime, QPS = qps };
                                                        qpsData[maxCreateTime] = qpsD;
                                                    }
                                                }
                                                else
                                                {
                                                    qps = qps + qpsD.QPS;
                                                    qpsD.QPS = qps;
                                                }
                                            }
                                        }

                                        await _qpsCollectService.Collect(prefix, qps, maxCreateTime, cancellationToken);
                                    }
                                    #endregion

                                    #region Duration
                                    //var calculateResponseDatas = from item in containerDatas
                                    //                            where item.Value.FileName == fileName && item.Value.Request != null && item.Value.Response != null
                                    //                            select (item.Value.Response!.CreateTime - item.Value.Request!.CreateTime).TotalMilliseconds;

                                    //maxCreateTime = (from item in containerDatas
                                    //                where item.Value.FileName == fileName && item.Value.Request != null && item.Value.Response != null
                                    //                orderby item.Value.Response!.CreateTime descending
                                    //                select item.Value.Response!.CreateTime).FirstOrDefault();

                                    //if (calculateResponseDatas.Count() == 0)
                                    //{
                                    //    return;
                                    //}

                                    //var avgResponse = calculateResponseDatas.Average();
                                    //var maxResponse = calculateResponseDatas.Max();
                                    //var minResponse = calculateResponseDatas.Min();

                                    //if (maxCreateTime != null)
                                    //{
                                    //    await _netDurationCollectService.Collect(prefix,minResponse, maxResponse, avgResponse, maxCreateTime!.Value, cancellationToken);
                                    //}

                                    var calculateResponseDatas = from item in containerDatas
                                                                 where item.Value.FileName == fileName && item.Value.Request != null && item.Value.Response != null
                                                                 group item by item.Value.Timestamp.ToString("yyyy-MM-dd HH:mm:ss") into g
                                                                 select new
                                                                 {
                                                                     g.Key,
                                                                     Total = g.Count()
                                                                    ,
                                                                     AvgDuration = g.Average(g => (g.Value.Response!.CreateTime - g.Value.Request!.CreateTime).TotalMilliseconds)
                                                                    ,
                                                                     MaxDuration = g.Max(g => (g.Value.Response!.CreateTime - g.Value.Request!.CreateTime).TotalMilliseconds)
                                                                    ,
                                                                     MinDuration = g.Min(g => (g.Value.Response!.CreateTime - g.Value.Request!.CreateTime).TotalMilliseconds)
                                                                 };

                                    if (calculateResponseDatas != null)
                                    {
                                        LoggerHelper.LogInformation($"{applicationConfiguration.ApplicationName}", $"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffff")}] {nameof(NetGatewayDataHandleService)} {fileName} Duration.Count = {calculateResponseDatas.Count()}.");
                                    }

                                    foreach (var row in calculateResponseDatas)
                                    {
                                        DateTime? maxCreateTime = Convert.ToDateTime(row.Key);

                                        var avgResponse = row.AvgDuration;
                                        var maxResponse = row.MaxDuration;
                                        var minResponse = row.MinDuration;

                                        //avgDurations.Add(avgResponse);
                                        //maxDurations.Add(maxResponse);
                                        //minDurations.Add(minResponse);

                                        await _netDurationCollectService.Collect(prefix, minResponse, maxResponse, avgResponse, maxCreateTime!.Value, cancellationToken);
                                    }
                                    #endregion

                                    #region Total
                                    //DateTime time = times.Max();
                                    //int count = counts.Sum();
                                    //double avgQPS = avgQPSs.Average();
                                    //double avgDuration = avgDurations.Average();
                                    //double maxDuration = maxDurations.Max();
                                    //double minDuration = minDurations.Min();

                                    if (containerDatas.Count > 0 && calculateDatas.Count() > 0 && calculateResponseDatas.Count() > 0)
                                    {
                                        DateTime time = Convert.ToDateTime(calculateDatas.Max(g => g.Key));
                                        int count = containerDatas.Count();
                                        double avgQPS = calculateDatas.Average(g => g.Total);
                                        double avgDuration = calculateResponseDatas.Average(g => g.AvgDuration);
                                        double maxDuration = calculateResponseDatas.Max(g => g.MaxDuration);
                                        double minDuration = calculateResponseDatas.Min(g => g.MinDuration);

                                        await _totalCollectService.Collect(prefix, count, minDuration, maxDuration, avgDuration, avgQPS, time, cancellationToken);
                                    }
                                    #endregion

                                    #region 处理只有request的数据
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
                                                        where item.Value.FileName == fileName && item.Value.Request != null && item.Value.Response == null
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
                                        dataContainer.Timestamp = item.Value.Request.CreateTime;
                                        dataContainer.Request = item.Value.Request;
                                    }
                                    #endregion
                                }
                                catch (Exception ex)
                                {
                                    LoggerHelper.LogError($"[{fileName}] {LoggerCategoryName}", ex.ToStackTraceString());
                                }
                            }
                        );
                        #endregion

                        #region 删除用过的文件
                        //删除用过的文件
                        foreach (var item in fileNames)
                        {
                            LoggerHelper.LogInformation($"{applicationConfiguration.ApplicationName}", $"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffff")}] {nameof(NetGatewayDataHandleService)} t2 Task.Run 删除用过的文件，{item}.");

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
                        #endregion

                        LoggerHelper.LogInformation($"{applicationConfiguration.ApplicationName}", $"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffff")}] {nameof(NetGatewayDataHandleService)} t2 Task.Run Task.Delay(10000).");

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

        private Task listenFileCompleted(NetGatewayDataHandleResultDefault result, string folderName, Dictionary<string, string> completedFileNames, Action<Dictionary<string,FileDataInfo>> completedAction)
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

                        int repeat = 3;
                        Dictionary<string,FileDataInfo> currentCompleteFileDataInfos = new Dictionary<string,FileDataInfo>();


                        for (var index = 0; index <= repeat - 1; index++)
                        {

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
                                        await Task.Delay(1000);

                                    } while (old_length != fileInfo.Length);

                                    currentCompleteFileDataInfos[fileInfo.FullName]=new FileDataInfo() { FileName = fileInfo.FullName, CreateTime = fileInfo.CreationTimeUtc };
                                    //completedAction(new FileDataInfo() { FileName = fileInfo.FullName, CreateTime = fileInfo.CreationTimeUtc });
                                    completedFileNames[item.FileName] = item.FileName;
                                    if (currentCompleteFileDataInfos.Count>=10)
                                    {
                                        completedAction(currentCompleteFileDataInfos);
                                        currentCompleteFileDataInfos.Clear();
                                    }
                                }
                            }

                            await Task.Delay(1000);
                        }


                        //foreach(var item in currentCompleteFileDataInfos)
                        //{
                            completedAction(currentCompleteFileDataInfos);
                            //completedFileNames[item.FileName] = item.FileName;
                        //}

                        if (result.IsStop)
                        {
                            break;
                        }

                        await Task.Delay(1000);
                    }
                    catch(Exception ex)
                    {
                        LoggerHelper.LogError(LoggerCategoryName, ex.ToStackTraceString());
                        await Task.Delay(1000);
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
            public DateTime Timestamp { get; set; }
            public NetData? Request { get; set; }
            public NetData? Response { get; set; }
        }

        private class FileDataInfo
        {
            public string FileName { get; set; } = null!;
            public DateTime CreateTime { get; set; }
        }

        private class QPSContainer
        {
            public string Prefix { get; set; } = null!;
            public DateTime Timestamp { get; set; }
            public int QPS { get; set; }
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
                    var strData = payloadData.ToString();
                    if (strData != null)
                    {
                        sourceDataAction.Invoke(strData);
                    }

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

            this.OpenPcapORPcapNFFile(fileName, dataformat, sourceDataAction, cancellationToken);
            //this.OpenPcapORPcapNFFile2(fileName, dataformat, sourceDataAction, cancellationToken);

            return Task.CompletedTask;
        }

        private List<string> sourceAddressList = new List<string>();
        private List<string> destinationAddressList = new List<string>();

        private class DataContainer
        {
            public uint Ack { get; set; }
            public uint Seq { get; set; }
            public DateTime Timestamp { get; set; }
            public byte[] Data { get; set; }
        }

        public void OpenPcapORPcapNFFile(string fileName, string dataformat, Func<string, Task> sourceDataAction, CancellationToken token = default)
        {
            ConcurrentDictionary<uint, List<DataContainer>> badDatas = new ConcurrentDictionary<uint, List<DataContainer>>();
            int index = 0;
            int indexException = 0;

            using (var reader = IReaderFactory.GetReader(fileName))
            {
                reader.OnReadPacketEvent += (context, packet) =>
                {
                    try
                    {
                        index++;

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

                            #region One Packet
                            //var requestType = 0;
                            //var googleData = this.GetGoogleData_TCP(payloadData, out requestType);

                            //if (googleData != null)
                            //{
                            //    object data = string.Empty;
                            //    string id = string.Empty;

                            //    data = EmptyMsg.Parser.ParseFrom(googleData);
                            //    id = ((Ctrade.Message.EmptyMsg)data).Header.MsgCd.ToString();

                            //    //string strPayloadData = System.Text.Encoding.Default.GetString(payloadData);

                            //    //if (strPayloadData.IndexOf(NetGatewayDataFormatTypes.APICreditUpdateReplyMsg) > -1)
                            //    //{
                            //    //    data = APICreditUpdateReplyMsg.Parser.ParseFrom(googleData);
                            //    //    id = ((Ctrade.Message.APICreditUpdateReplyMsg)data).Header.MsgCd.ToString();
                            //    //}
                            //    //else if (strPayloadData.IndexOf(NetGatewayDataFormatTypes.APICreditUpdateRequestMsg) > -1)
                            //    //{
                            //    //    data = APICreditUpdateRequestMsg.Parser.ParseFrom(googleData);
                            //    //    id = ((Ctrade.Message.APICreditUpdateRequestMsg)data).Header.MsgCd.ToString();
                            //    //}
                            //    //else if (strPayloadData.IndexOf(NetGatewayDataFormatTypes.ApiListMarketDataAck) > -1)
                            //    //{
                            //    //    data = ApiListMarketDataAck.Parser.ParseFrom(googleData);
                            //    //    id = ((Ctrade.Message.ApiListMarketDataAck)data).Header.MsgCd.ToString();
                            //    //}
                            //    //else if (strPayloadData.IndexOf(NetGatewayDataFormatTypes.ApiMarketData) > -1)
                            //    //{
                            //    //    data = ApiMarketData.Parser.ParseFrom(googleData);
                            //    //    id = ((Ctrade.Message.ApiMarketData)data).Header.MsgCd.ToString();
                            //    //}
                            //    //else if (strPayloadData.IndexOf(NetGatewayDataFormatTypes.ApiMarketDataRequest) > -1)
                            //    //{
                            //    //    data = ApiMarketDataRequest.Parser.ParseFrom(googleData);
                            //    //    id = ((Ctrade.Message.ApiMarketDataRequest)data).Header.MsgCd.ToString();
                            //    //}
                            //    //else if (strPayloadData.IndexOf(NetGatewayDataFormatTypes.APIOcoOrderCancelReplyMsg) > -1)
                            //    //{
                            //    //    data = APIOcoOrderCancelReplyMsg.Parser.ParseFrom(googleData);
                            //    //    id = ((Ctrade.Message.APIOcoOrderCancelReplyMsg)data).Header.MsgCd.ToString();
                            //    //}
                            //    //else if (strPayloadData.IndexOf(NetGatewayDataFormatTypes.APIOcoOrderCancelRequestMsg) > -1)
                            //    //{
                            //    //    data = APIOcoOrderCancelRequestMsg.Parser.ParseFrom(googleData);
                            //    //    id = ((Ctrade.Message.APIOcoOrderCancelRequestMsg)data).Header.MsgCd.ToString();
                            //    //}
                            //    //else if (strPayloadData.IndexOf(NetGatewayDataFormatTypes.APIOcoOrderSumitReplyMsg) > -1)
                            //    //{
                            //    //    data = APIOcoOrderSumitReplyMsg.Parser.ParseFrom(googleData);
                            //    //    id = ((Ctrade.Message.APIOcoOrderSumitReplyMsg)data).Header.MsgCd.ToString();
                            //    //}
                            //    //else if (strPayloadData.IndexOf(NetGatewayDataFormatTypes.APIOcoOrderSumitRequestMsg) > -1)
                            //    //{
                            //    //    data = APIOcoOrderSumitRequestMsg.Parser.ParseFrom(googleData);
                            //    //    id = ((Ctrade.Message.APIOcoOrderSumitRequestMsg)data).Header.MsgCd.ToString();
                            //    //}
                            //    //else if (strPayloadData.IndexOf(NetGatewayDataFormatTypes.APIOrderCancelReplyMsg) > -1)
                            //    //{
                            //    //    data = APIOrderCancelReplyMsg.Parser.ParseFrom(googleData);
                            //    //    id = ((Ctrade.Message.APIOrderCancelReplyMsg)data).Header.MsgCd.ToString();
                            //    //}
                            //    //else if (strPayloadData.IndexOf(NetGatewayDataFormatTypes.APIOrderCancelRequestMsg) > -1)
                            //    //{
                            //    //    data = APIOrderCancelRequestMsg.Parser.ParseFrom(googleData);
                            //    //    id = ((Ctrade.Message.APIOrderCancelRequestMsg)data).Header.MsgCd.ToString();
                            //    //}
                            //    //else if (strPayloadData.IndexOf(NetGatewayDataFormatTypes.APIOrderSubmitReplyMsg) > -1)
                            //    //{
                            //    //    data = APIOrderSubmitReplyMsg.Parser.ParseFrom(googleData);
                            //    //    id = ((Ctrade.Message.APIOrderSubmitReplyMsg)data).Header.MsgCd.ToString();
                            //    //}
                            //    //else if (strPayloadData.IndexOf(NetGatewayDataFormatTypes.APIOrderSubmitRequestMsg) > -1)
                            //    //{
                            //    //    data = APIOrderSubmitRequestMsg.Parser.ParseFrom(googleData);
                            //    //    id = ((Ctrade.Message.APIOrderSubmitRequestMsg)data).Header.MsgCd.ToString();
                            //    //}
                            //    //else if (strPayloadData.IndexOf(NetGatewayDataFormatTypes.BridgeOrderSubmitRequestMsg) > -1)
                            //    //{
                            //    //    data = BridgeOrderSubmitRequestMsg.Parser.ParseFrom(googleData);
                            //    //    id = ((Ctrade.Message.BridgeOrderSubmitRequestMsg)data).Header.MsgCd.ToString();
                            //    //}
                            //    //else if (strPayloadData.IndexOf(NetGatewayDataFormatTypes.TokenReplyMsg) > -1)
                            //    //{
                            //    //    data = TokenReplyMsg.Parser.ParseFrom(googleData);
                            //    //    id = ((Ctrade.Message.TokenReplyMsg)data).Header.MsgCd.ToString();
                            //    //}
                            //    //else if (strPayloadData.IndexOf(NetGatewayDataFormatTypes.TokenRequestMsg) > -1)
                            //    //{
                            //    //    data = TokenRequestMsg.Parser.ParseFrom(googleData);
                            //    //    id = ((Ctrade.Message.TokenRequestMsg)data).Header.MsgCd.ToString();
                            //    //}
                            //    //else if (strPayloadData.IndexOf(NetGatewayDataFormatTypes.EmptyMsg) > -1)
                            //    //{
                            //    //    data = EmptyMsg.Parser.ParseFrom(googleData);
                            //    //    id = ((Ctrade.Message.EmptyMsg)data).Header.MsgCd.ToString();
                            //    //}
                            //    //else
                            //    //{
                            //    //    switch (dataformat)
                            //    //    {
                            //    //        case NetGatewayDataFormatTypes.APICreditUpdateReplyMsg:
                            //    //            data = APICreditUpdateReplyMsg.Parser.ParseFrom(googleData);
                            //    //            id = ((Ctrade.Message.APICreditUpdateReplyMsg)data).Header.MsgCd.ToString();

                            //    //            break;
                            //    //        case NetGatewayDataFormatTypes.APICreditUpdateRequestMsg:
                            //    //            data = APICreditUpdateRequestMsg.Parser.ParseFrom(googleData);
                            //    //            id = ((Ctrade.Message.APICreditUpdateRequestMsg)data).Header.MsgCd.ToString();

                            //    //            break;
                            //    //        case NetGatewayDataFormatTypes.ApiListMarketDataAck:
                            //    //            data = ApiListMarketDataAck.Parser.ParseFrom(googleData);
                            //    //            id = ((Ctrade.Message.ApiListMarketDataAck)data).Header.MsgCd.ToString();

                            //    //            break;
                            //    //        case NetGatewayDataFormatTypes.ApiMarketData:
                            //    //            data = ApiMarketData.Parser.ParseFrom(googleData);
                            //    //            id = ((Ctrade.Message.ApiMarketData)data).Header.MsgCd.ToString();

                            //    //            break;
                            //    //        case NetGatewayDataFormatTypes.ApiMarketDataRequest:
                            //    //            data = ApiMarketDataRequest.Parser.ParseFrom(googleData);
                            //    //            id = ((Ctrade.Message.ApiMarketDataRequest)data).Header.MsgCd.ToString();

                            //    //            break;
                            //    //        case NetGatewayDataFormatTypes.APIOcoOrderCancelReplyMsg:
                            //    //            data = APIOcoOrderCancelReplyMsg.Parser.ParseFrom(googleData);
                            //    //            id = ((Ctrade.Message.APIOcoOrderCancelReplyMsg)data).Header.MsgCd.ToString();

                            //    //            break;
                            //    //        case NetGatewayDataFormatTypes.APIOcoOrderCancelRequestMsg:
                            //    //            data = APIOcoOrderCancelRequestMsg.Parser.ParseFrom(googleData);
                            //    //            id = ((Ctrade.Message.APIOcoOrderCancelRequestMsg)data).Header.MsgCd.ToString();

                            //    //            break;
                            //    //        case NetGatewayDataFormatTypes.APIOcoOrderSumitReplyMsg:
                            //    //            data = APIOcoOrderSumitReplyMsg.Parser.ParseFrom(googleData);
                            //    //            id = ((Ctrade.Message.APIOcoOrderSumitReplyMsg)data).Header.MsgCd.ToString();

                            //    //            break;
                            //    //        case NetGatewayDataFormatTypes.APIOcoOrderSumitRequestMsg:
                            //    //            data = APIOcoOrderSumitRequestMsg.Parser.ParseFrom(googleData);
                            //    //            id = ((Ctrade.Message.APIOcoOrderSumitRequestMsg)data).Header.MsgCd.ToString();

                            //    //            break;
                            //    //        case NetGatewayDataFormatTypes.APIOrderCancelReplyMsg:
                            //    //            data = APIOrderCancelReplyMsg.Parser.ParseFrom(googleData);
                            //    //            id = ((Ctrade.Message.APIOrderCancelReplyMsg)data).Header.MsgCd.ToString();

                            //    //            break;
                            //    //        case NetGatewayDataFormatTypes.APIOrderCancelRequestMsg:
                            //    //            data = APIOrderCancelRequestMsg.Parser.ParseFrom(googleData);
                            //    //            id = ((Ctrade.Message.APIOrderCancelRequestMsg)data).Header.MsgCd.ToString();

                            //    //            break;
                            //    //        case NetGatewayDataFormatTypes.APIOrderSubmitReplyMsg:
                            //    //            data = APIOrderSubmitReplyMsg.Parser.ParseFrom(googleData);
                            //    //            id = ((Ctrade.Message.APIOrderSubmitReplyMsg)data).Header.MsgCd.ToString();

                            //    //            break;
                            //    //        case NetGatewayDataFormatTypes.APIOrderSubmitRequestMsg:
                            //    //            data = APIOrderSubmitRequestMsg.Parser.ParseFrom(googleData);
                            //    //            id = ((Ctrade.Message.APIOrderSubmitRequestMsg)data).Header.MsgCd.ToString();

                            //    //            break;
                            //    //        case NetGatewayDataFormatTypes.BridgeOrderSubmitRequestMsg:
                            //    //            data = BridgeOrderSubmitRequestMsg.Parser.ParseFrom(googleData);
                            //    //            id = ((Ctrade.Message.BridgeOrderSubmitRequestMsg)data).Header.MsgCd.ToString();

                            //    //            break;
                            //    //        case NetGatewayDataFormatTypes.TokenReplyMsg:
                            //    //            data = TokenReplyMsg.Parser.ParseFrom(googleData);
                            //    //            id = ((Ctrade.Message.TokenReplyMsg)data).Header.MsgCd.ToString();

                            //    //            break;
                            //    //        case NetGatewayDataFormatTypes.TokenRequestMsg:
                            //    //            data = TokenRequestMsg.Parser.ParseFrom(googleData);
                            //    //            id = ((Ctrade.Message.TokenRequestMsg)data).Header.MsgCd.ToString();

                            //    //            break;
                            //    //        case NetGatewayDataFormatTypes.EmptyMsg:
                            //    //            data = EmptyMsg.Parser.ParseFrom(googleData);
                            //    //            id = ((Ctrade.Message.EmptyMsg)data).Header.MsgCd.ToString();

                            //    //            break;
                            //    //        default:
                            //    //            break;
                            //    //    }
                            //    //}

                            //    if (!string.IsNullOrEmpty(data.ToString()))
                            //    {
                            //        sourceDataAction.Invoke(string.Format("{0}|{1}|{2}|{3}", requestType, id, timestamp.ToOADate().ToString(), string.Empty, data.ToString())); ;
                            //    }
                            //}
                            #endregion

                            List<int> requestTypes = new List<int>();
                            List<byte[]> datas = new List<byte[]>();

                            switch (dataformat)
                            {
                                case NetGatewayDataFormatTypes.DEP:
                                    datas = this.GetGoogleData_TCP_DEP_List(payloadData, timestamp, payloadPacket, out requestTypes, ref badDatas, sourceDataAction, token);

                                    break;
                                case NetGatewayDataFormatTypes.IMIX:
                                    datas = this.GetGoogleData_TCP_IMIX_List(payloadData, timestamp, payloadPacket, out requestTypes, ref badDatas, sourceDataAction, token);

                                    break;
                                default:
                                    datas = this.GetGoogleData_TCP_List(payloadData, timestamp, payloadPacket, out requestTypes, ref badDatas, sourceDataAction, token);

                                    break;
                            }
                        }
                        catch (Exception ex)
                        {
                            indexException++;
                            //throw new Exception(string.Format("GoogleData Error, Exception: {0}. {1}", ex.Message, ex.StackTrace));
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(string.Format("OnReadPacketEvent Error, Exception: {0}. {1}", ex.Message, ex.StackTrace));
                    }
                };
                reader.ReadPackets(token);
            }

            var applicationConfiguration = ConfigurationContainer.Get<ApplicationConfiguration>(ConfigurationNames.Application);
            LoggerHelper.LogInformation($"{applicationConfiguration.ApplicationName}", $"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffff")}] {nameof(GetSourceDataFromFileService)} {Path.GetFileName(fileName)} 网络数据包解析完毕，总数有{index.ToString()}，解析错误有{indexException}，坏包有{badDatas.Count().ToString()}.");
            LoggerHelper.LogInformation($"{applicationConfiguration.ApplicationName}", $"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffff")}] {nameof(GetSourceDataFromFileService)} {Path.GetFileName(fileName)} 开始处理坏包...");

            // 处理坏包
            int count = badDatas.Count();
            index = 0;
            indexException = 0;

            foreach (List<DataContainer> ackDatas in badDatas.Values)
            {
                index++;

                if (ackDatas.Count == 0)
                {
                    continue;
                }

                try
                {
                    DateTime timestamp = ackDatas[0].Timestamp;
                    var payloadData = MergeData(ackDatas);

                    List<int> requestTypes = new List<int>();
                    List<byte[]> datas = new List<byte[]>();

                    switch (dataformat)
                    {
                        case NetGatewayDataFormatTypes.DEP:
                            datas = this.GetGoogleData_TCP_DEP_List(payloadData, timestamp, out requestTypes, sourceDataAction, token);

                            break;
                        case NetGatewayDataFormatTypes.IMIX:
                            datas = this.GetGoogleData_TCP_IMIX_List(payloadData, timestamp, out requestTypes, sourceDataAction, token);

                            break;
                        default:
                            datas = this.GetGoogleData_TCP_List(payloadData, timestamp, out requestTypes, sourceDataAction, token);

                            break;
                    }
                }
                catch (Exception ex)
                {
                    indexException++;
                    //throw new Exception(string.Format("GoogleData Error, Exception: {0}. {1}", ex.Message, ex.StackTrace));
                }
            }

            LoggerHelper.LogInformation($"{applicationConfiguration.ApplicationName}", $"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffff")}] {nameof(GetSourceDataFromFileService)} {Path.GetFileName(fileName)} 坏包处理完毕.");
        }

        private List<byte[]>? GetGoogleData_TCP_List(byte[] data, DateTime timestamp, PacketDotNet.Packet packet, out List<int> requestTypes, ref ConcurrentDictionary<uint, List<DataContainer>> badDatas, Func<string, Task> sourceDataAction, CancellationToken token = default)
        {
            List<byte[]> datas = new List<byte[]>();

            requestTypes = new List<int>();

            if (data == null || data.Length < 10)
            {
                return datas;
            }

            int packetType = data[0];
            int tcpMessageType = data[5];
            int tcpEnd = data[data.Length - 1];

            int imixType_0 = data[0];
            int imixType_1 = data[1];
            int imixType_2 = data[2];
            int imixType_3 = data[3];
            int imixType_4 = data[4];
            int imixType_5 = data[5];
            int imixEnd_8 = data[data.Length - 8];
            int imixEnd_7 = data[data.Length - 7];
            int imixEnd_6 = data[data.Length - 6];
            int imixEnd_5 = data[data.Length - 5];
            int imixEnd_1 = data[data.Length - 1];

            if (packetType == 2 && tcpMessageType == 85 && tcpEnd == 3)
            {
                datas = this.GetGoogleData_TCP_DEP_List(data, timestamp, packet, out requestTypes, ref badDatas, sourceDataAction, token);
            }
            else if (packetType == 2 && tcpMessageType == 85 && tcpEnd != 3 || packetType != 2 && tcpMessageType != 85 && tcpEnd == 3)
            {
                datas = this.GetGoogleData_TCP_DEP_List(data, timestamp, packet, out requestTypes, ref badDatas, sourceDataAction, token);
            }
            else if (imixType_0 == 56 && imixType_1 == 61 && imixType_2 == 73 && imixType_3 == 77 && imixType_4 == 73 && imixType_5 == 88
                 && imixEnd_8 == 1 && imixEnd_7 == 49 && imixEnd_6 == 48 && imixEnd_5 == 61 && imixEnd_1 == 1)
            {
                datas = this.GetGoogleData_TCP_IMIX_List(data, timestamp, packet, out requestTypes, ref badDatas, sourceDataAction, token);
            }
            else if ((imixType_0 == 56 && imixType_1 == 61 && imixType_2 == 73 && imixType_3 == 77 && imixType_4 == 73 && imixType_5 == 88
                && imixEnd_8 != 1 && imixEnd_7 != 49 && imixEnd_6 != 48 && imixEnd_5 != 61 && imixEnd_1 != 1)
                || (imixType_0 != 56 && imixType_1 != 61 && imixType_2 != 73 && imixType_3 != 77 && imixType_4 != 73 && imixType_5 != 88
                && imixEnd_8 == 1 && imixEnd_7 == 49 && imixEnd_6 == 48 && imixEnd_5 == 61 && imixEnd_1 == 1))
            {
                datas = this.GetGoogleData_TCP_IMIX_List(data, timestamp, packet, out requestTypes, ref badDatas, sourceDataAction, token);
            }

            return datas;
        }

        private List<byte[]>? GetGoogleData_TCP_List(byte[] data, DateTime timestamp, out List<int> requestTypes, Func<string, Task> sourceDataAction, CancellationToken token = default)
        {
            List<byte[]> datas = new List<byte[]>();

            requestTypes = new List<int>();

            if (data == null || data.Length < 10)
            {
                return datas;
            }

            int packetType = data[0];
            int tcpMessageType = data[5];
            int tcpEnd = data[data.Length - 1];

            int imixType_0 = data[0];
            int imixType_1 = data[1];
            int imixType_2 = data[2];
            int imixType_3 = data[3];
            int imixType_4 = data[4];
            int imixType_5 = data[5];
            int imixEnd_8 = data[data.Length - 8];
            int imixEnd_7 = data[data.Length - 7];
            int imixEnd_6 = data[data.Length - 6];
            int imixEnd_5 = data[data.Length - 5];
            int imixEnd_1 = data[data.Length - 1];

            if (packetType == 2 && tcpMessageType == 85 && tcpEnd == 3)
            {
                datas = this.GetGoogleData_TCP_DEP_List(data, timestamp, out requestTypes, sourceDataAction, token);
            }
            else if (imixType_0 == 56 && imixType_1 == 61 && imixType_2 == 73 && imixType_3 == 77 && imixType_4 == 73 && imixType_5 == 88
                && imixEnd_8 == 1 && imixEnd_7 == 49 && imixEnd_6 == 48 && imixEnd_5 == 61 && imixEnd_1 == 1)
            {
                datas = this.GetGoogleData_TCP_IMIX_List(data, timestamp, out requestTypes, sourceDataAction, token);
            }

            return datas;
        }

        private List<byte[]>? GetGoogleData_TCP_DEP_List(byte[] data, DateTime timestamp, PacketDotNet.Packet packet, out List<int> requestTypes, ref ConcurrentDictionary<uint, List<DataContainer>> badDatas, Func<string, Task> sourceDataAction, CancellationToken token = default)
        {
            List<byte[]> datas = new List<byte[]>();

            requestTypes = new List<int>();

            if (data == null || data.Length < 10)
            {
                return datas;
            }

            int packetType = data[0];
            int tcpMessageType = data[5];
            int tcpEnd = data[data.Length - 1];

            if (packetType == 2 && tcpMessageType == 85 && tcpEnd == 3)
            {
                List<byte[]> goodDatas = this.GetGoodData_DEP(data);

                foreach (byte[] goodData in goodDatas)
                {
                    packetType = goodData[0];
                    tcpMessageType = goodData[5];
                    tcpEnd = goodData[goodData.Length - 1];

                    if (packetType == 2 && tcpMessageType == 85 && tcpEnd == 3)
                    {
                        int index = 0;
                        int tcp_start = 6;
                        int sizeLengthOfChannelName = 2;
                        int sizeChannelName = Byte2Int(goodData.Skip(tcp_start).Take(sizeLengthOfChannelName).ToArray());
                        int sizeLengthOfTargetInstanceName = 1;
                        index = tcp_start + sizeLengthOfChannelName + sizeChannelName;
                        int sizeTargetInstanceName = goodData[index];
                        //int sizeTargetInstanceName = goodData[tcp_start + sizeLengthOfChannelName + sizeChannelName];
                        int sizeLengthOfData = 4;
                        //int sizeData = Byte4Int(goodData.Skip(tcp_start + sizeLengthOfChannelName + sizeChannelName + sizeLengthOfTargetInstanceName + sizeTargetInstanceName).Take(sizeLengthOfData).ToArray());

                        index = index + sizeLengthOfTargetInstanceName + sizeTargetInstanceName + sizeLengthOfData;
                        int depapi_start = index;
                        //int depapi_start = tcp_start + sizeLengthOfChannelName + sizeChannelName + sizeLengthOfTargetInstanceName + sizeTargetInstanceName + sizeLengthOfData;
                        int messageType = goodData[depapi_start];
                        int requestType = goodData[depapi_start + 23];

                        if (packetType == 2 && messageType == 7 && tcpEnd == 3 && (requestType == 0 || requestType == 1))
                        {
                            //byte[] length_osin_byte = goodData.Skip(depapi_start + 23 + 5).Take(2).ToArray();
                            byte[] length_osin_byte = goodData.Skip(depapi_start + 28).Take(2).ToArray();
                            int length_osin = Byte2Int(length_osin_byte);

                            index = depapi_start + 30 + length_osin;
                            //int dsp_begin = depapi_start + 23 + 5 + 2 + length_osin + 5;
                            int dsp_begin = index + 5;
                            //int dsp_end = dsp_begin + 4;
                            byte[] length_dspm_byte = goodData.Skip(dsp_begin).Take(4).ToArray();
                            int length_dspm = Byte4Int(length_dspm_byte);

                            //int dspapi_begin = depapi_start + 23 + 5 + 2 + length_osin;
                            int dspapi_begin = index;
                            int dspapi_end = dspapi_begin + length_dspm;

                            byte[] body = goodData.Skip(dspapi_end).Take(goodData.Length - dspapi_end - 1).ToArray();
                            datas.Add(body);
                            requestTypes.Add(requestType);

                            var googleData = EmptyMsg.Parser.ParseFrom(body);
                            string id = ((Ctrade.Message.EmptyMsg)googleData).Header.MsgCd.ToString();

                            if (!string.IsNullOrEmpty(googleData.ToString()))
                            {
                                sourceDataAction.Invoke(string.Format("{0}|{1}|{2}|{3}", requestType, id, timestamp.ToOADate().ToString(), string.Empty, data.ToString())); ;
                            }
                        }
                    }
                    else if (packetType == 2 && tcpMessageType == 85 && tcpEnd != 3 || packetType != 2 && tcpMessageType != 85 && tcpEnd == 3)
                    {
                        // Bad Data
                        uint ack = ((PacketDotNet.TcpPacket)packet).AcknowledgmentNumber;
                        uint seq = ((PacketDotNet.TcpPacket)packet).SequenceNumber;

                        if (!badDatas.TryGetValue(ack, out List<DataContainer>? ackDatas))
                        {
                            lock (badDatas)
                            {
                                if (!badDatas.TryGetValue(ack, out ackDatas))
                                {
                                    ackDatas = new List<DataContainer>();
                                    badDatas[ack] = ackDatas;
                                }
                            }
                        }

                        lock (ackDatas)
                        {
                            //ackDatas.Add(new DataContainer() { Ack = ack, Seq = seq, Timestamp = timestamp, Data = goodData });
                            ackDatas.Insert(0, new DataContainer() { Ack = ack, Seq = seq, Timestamp = timestamp, Data = goodData });
                        }
                    }
                }
            }
            else if (packetType == 2 && tcpMessageType == 85 && tcpEnd != 3 || packetType != 2 && tcpMessageType != 85 && tcpEnd == 3)
            {
                // Bad Data
                uint ack = ((PacketDotNet.TcpPacket)packet).AcknowledgmentNumber;
                uint seq = ((PacketDotNet.TcpPacket)packet).SequenceNumber;

                if (!badDatas.TryGetValue(ack, out List<DataContainer>? ackDatas))
                {
                    lock (badDatas)
                    {
                        if (!badDatas.TryGetValue(ack, out ackDatas))
                        {
                            ackDatas = new List<DataContainer>();
                            badDatas[ack] = ackDatas;
                        }
                    }
                }

                lock (ackDatas)
                {
                    //ackDatas.Add(new DataContainer() { Ack = ack, Seq = seq, Timestamp = timestamp, Data = data });
                    ackDatas.Insert(0, new DataContainer() { Ack = ack, Seq = seq, Timestamp = timestamp, Data = data });
                }
            }

            return datas;
        }

        private List<byte[]>? GetGoogleData_TCP_DEP_List(byte[] data, DateTime timestamp, out List<int> requestTypes, Func<string, Task> sourceDataAction, CancellationToken token = default)
        {
            List<byte[]> datas = new List<byte[]>();

            requestTypes = new List<int>();

            if (data == null || data.Length < 10)
            {
                return datas;
            }

            int packetType = data[0];
            int tcpMessageType = data[5];
            int tcpEnd = data[data.Length - 1];

            if (packetType == 2 && tcpMessageType == 85 && tcpEnd == 3)
            {
                List<byte[]> goodDatas = this.GetGoodData_DEP(data);

                foreach (byte[] goodData in goodDatas)
                {
                    packetType = goodData[0];
                    tcpMessageType = goodData[5];
                    tcpEnd = goodData[goodData.Length - 1];

                    if (packetType == 2 && tcpMessageType == 85 && tcpEnd == 3)
                    {
                        int index = 0;
                        int tcp_start = 6;
                        int sizeLengthOfChannelName = 2;
                        int sizeChannelName = Byte2Int(goodData.Skip(tcp_start).Take(sizeLengthOfChannelName).ToArray());
                        int sizeLengthOfTargetInstanceName = 1;
                        index = tcp_start + sizeLengthOfChannelName + sizeChannelName;
                        int sizeTargetInstanceName = goodData[index];
                        //int sizeTargetInstanceName = goodData[tcp_start + sizeLengthOfChannelName + sizeChannelName];
                        int sizeLengthOfData = 4;
                        //int sizeData = Byte4Int(goodData.Skip(tcp_start + sizeLengthOfChannelName + sizeChannelName + sizeLengthOfTargetInstanceName + sizeTargetInstanceName).Take(sizeLengthOfData).ToArray());

                        index = index + sizeLengthOfTargetInstanceName + sizeTargetInstanceName + sizeLengthOfData;
                        int depapi_start = index;
                        //int depapi_start = tcp_start + sizeLengthOfChannelName + sizeChannelName + sizeLengthOfTargetInstanceName + sizeTargetInstanceName + sizeLengthOfData;
                        int messageType = goodData[depapi_start];
                        int requestType = goodData[depapi_start + 23];

                        if (packetType == 2 && messageType == 7 && tcpEnd == 3 && (requestType == 0 || requestType == 1))
                        {
                            //byte[] length_osin_byte = goodData.Skip(depapi_start + 23 + 5).Take(2).ToArray();
                            byte[] length_osin_byte = goodData.Skip(depapi_start + 28).Take(2).ToArray();
                            int length_osin = Byte2Int(length_osin_byte);

                            index = depapi_start + 30 + length_osin;
                            //int dsp_begin = depapi_start + 23 + 5 + 2 + length_osin + 5;
                            int dsp_begin = index + 5;
                            //int dsp_end = dsp_begin + 4;
                            byte[] length_dspm_byte = goodData.Skip(dsp_begin).Take(4).ToArray();
                            int length_dspm = Byte4Int(length_dspm_byte);

                            //int dspapi_begin = depapi_start + 23 + 5 + 2 + length_osin;
                            int dspapi_begin = index;
                            int dspapi_end = dspapi_begin + length_dspm;

                            byte[] body = goodData.Skip(dspapi_end).Take(goodData.Length - dspapi_end - 1).ToArray();
                            datas.Add(body);
                            requestTypes.Add(requestType);

                            var googleData = EmptyMsg.Parser.ParseFrom(body);
                            string id = ((Ctrade.Message.EmptyMsg)googleData).Header.MsgCd.ToString();

                            if (!string.IsNullOrEmpty(googleData.ToString()))
                            {
                                sourceDataAction.Invoke(string.Format("{0}|{1}|{2}|{3}", requestType, id, timestamp.ToOADate().ToString(), string.Empty, data.ToString())); ;
                            }
                        }
                    }
                }
            }

            return datas;
        }
        
        private List<byte[]>? GetGoogleData_TCP_IMIX_List(byte[] data, DateTime timestamp, PacketDotNet.Packet packet, out List<int> requestTypes, ref ConcurrentDictionary<uint, List<DataContainer>> badDatas, Func<string, Task> sourceDataAction, CancellationToken token = default)
        {
            List<byte[]> datas = new List<byte[]>();

            requestTypes = new List<int>();

            if (data == null || data.Length < 10)
            {
                return datas;
            }

            int imixType_0 = data[0];
            int imixType_1 = data[1];
            int imixType_2 = data[2];
            int imixType_3 = data[3];
            int imixType_4 = data[4];
            int imixType_5 = data[5];
            int imixEnd_8 = data[data.Length - 8];
            int imixEnd_7 = data[data.Length - 7];
            int imixEnd_6 = data[data.Length - 6];
            int imixEnd_5 = data[data.Length - 5];
            int imixEnd_1 = data[data.Length - 1];

            if (imixType_0 == 56 && imixType_1 == 61 && imixType_2 == 73 && imixType_3 == 77 && imixType_4 == 73 && imixType_5 == 88
                 && imixEnd_8 == 1 && imixEnd_7 == 49 && imixEnd_6 == 48 && imixEnd_5 == 61 && imixEnd_1 == 1)
            {
                List<byte[]> goodDatas = this.GetGoodData_IMIX(data);

                foreach (byte[] goodData in goodDatas)
                {
                    imixType_0 = data[0];
                    imixType_1 = data[1];
                    imixType_2 = data[2];
                    imixType_3 = data[3];
                    imixType_4 = data[4];
                    imixType_5 = data[5];
                    imixEnd_8 = data[data.Length - 8];
                    imixEnd_7 = data[data.Length - 7];
                    imixEnd_6 = data[data.Length - 6];
                    imixEnd_5 = data[data.Length - 5];
                    imixEnd_1 = data[data.Length - 1];

                    if (imixType_0 == 56 && imixType_1 == 61 && imixType_2 == 73 && imixType_3 == 77 && imixType_4 == 73 && imixType_5 == 88
                        && imixEnd_8 == 1 && imixEnd_7 == 49 && imixEnd_6 == 48 && imixEnd_5 == 61 && imixEnd_1 == 1)
                    {
                        string strGoodData = System.Text.Encoding.Default.GetString(goodData);

                        string requestTypeStart = "\x01" + "35=";
                        string requestTypeEnd = "\x01";
                        string requestTypeString = MidStrEx_New(strGoodData, requestTypeStart, requestTypeEnd);
                        int requestType = requestTypeString == "8" ? 1 : 0;

                        string idStart = "\x01" + "12087=";
                        string idEnd = "\x01";
                        string id = MidStrEx_New(strGoodData, idStart, idEnd);

                        datas.Add(goodData);
                        requestTypes.Add(requestType);

                        if (!string.IsNullOrEmpty(strGoodData.ToString()))
                        {
                            sourceDataAction.Invoke(string.Format("{0}|{1}|{2}|{3}", requestType, id, timestamp.ToOADate().ToString(), string.Empty, goodData.ToString())); ;
                        }
                    }
                    else if ((imixType_0 == 56 && imixType_1 == 61 && imixType_2 == 73 && imixType_3 == 77 && imixType_4 == 73 && imixType_5 == 88
                        && imixEnd_8 != 1 && imixEnd_7 != 49 && imixEnd_6 != 48 && imixEnd_5 != 61 && imixEnd_1 != 1)
                        || (imixType_0 != 56 && imixType_1 != 61 && imixType_2 != 73 && imixType_3 != 77 && imixType_4 != 73 && imixType_5 != 88
                        && imixEnd_8 == 1 && imixEnd_7 == 49 && imixEnd_6 == 48 && imixEnd_5 == 61 && imixEnd_1 == 1))
                    {
                        // Bad Data
                        uint ack = ((PacketDotNet.TcpPacket)packet).AcknowledgmentNumber;
                        uint seq = ((PacketDotNet.TcpPacket)packet).SequenceNumber;

                        if (!badDatas.TryGetValue(ack, out List<DataContainer>? ackDatas))
                        {
                            lock (badDatas)
                            {
                                if (!badDatas.TryGetValue(ack, out ackDatas))
                                {
                                    ackDatas = new List<DataContainer>();
                                    badDatas[ack] = ackDatas;
                                }
                            }
                        }

                        lock (ackDatas)
                        {
                            //ackDatas.Add(new DataContainer() { Ack = ack, Seq = seq, Timestamp = timestamp, Data = goodData });
                            ackDatas.Insert(0, new DataContainer() { Ack = ack, Seq = seq, Timestamp = timestamp, Data = goodData });
                        }
                    }
                }
            }
            else if ((imixType_0 == 56 && imixType_1 == 61 && imixType_2 == 73 && imixType_3 == 77 && imixType_4 == 73 && imixType_5 == 88
                && imixEnd_8 != 1 && imixEnd_7 != 49 && imixEnd_6 != 48 && imixEnd_5 != 61 && imixEnd_1 != 1)
                || (imixType_0 != 56 && imixType_1 != 61 && imixType_2 != 73 && imixType_3 != 77 && imixType_4 != 73 && imixType_5 != 88
                && imixEnd_8 == 1 && imixEnd_7 == 49 && imixEnd_6 == 48 && imixEnd_5 == 61 && imixEnd_1 == 1))
            {
                // Bad Data
                uint ack = ((PacketDotNet.TcpPacket)packet).AcknowledgmentNumber;
                uint seq = ((PacketDotNet.TcpPacket)packet).SequenceNumber;

                if (!badDatas.TryGetValue(ack, out List<DataContainer>? ackDatas))
                {
                    lock (badDatas)
                    {
                        if (!badDatas.TryGetValue(ack, out ackDatas))
                        {
                            ackDatas = new List<DataContainer>();
                            badDatas[ack] = ackDatas;
                        }
                    }
                }

                lock (ackDatas)
                {
                    //ackDatas.Add(new DataContainer() { Ack = ack, Seq = seq, Timestamp = timestamp, Data = data });
                    ackDatas.Insert(0, new DataContainer() { Ack = ack, Seq = seq, Timestamp = timestamp, Data = data });
                }
            }

            return datas;
        }

        private List<byte[]>? GetGoogleData_TCP_IMIX_List(byte[] data, DateTime timestamp, out List<int> requestTypes, Func<string, Task> sourceDataAction, CancellationToken token = default)
        {
            List<byte[]> datas = new List<byte[]>();

            requestTypes = new List<int>();

            if (data == null || data.Length < 10)
            {
                return datas;
            }

            int imixType_0 = data[0];
            int imixType_1 = data[1];
            int imixType_2 = data[2];
            int imixType_3 = data[3];
            int imixType_4 = data[4];
            int imixType_5 = data[5];
            int imixEnd_8 = data[data.Length - 8];
            int imixEnd_7 = data[data.Length - 7];
            int imixEnd_6 = data[data.Length - 6];
            int imixEnd_5 = data[data.Length - 5];
            int imixEnd_1 = data[data.Length - 1];

            if (imixType_0 == 56 && imixType_1 == 61 && imixType_2 == 73 && imixType_3 == 77 && imixType_4 == 73 && imixType_5 == 88
                 && imixEnd_8 == 1 && imixEnd_7 == 49 && imixEnd_6 == 48 && imixEnd_5 == 61 && imixEnd_1 == 1)
            {
                List<byte[]> goodDatas = this.GetGoodData_IMIX(data);

                foreach (byte[] goodData in goodDatas)
                {
                    imixType_0 = data[0];
                    imixType_1 = data[1];
                    imixType_2 = data[2];
                    imixType_3 = data[3];
                    imixType_4 = data[4];
                    imixType_5 = data[5];
                    imixEnd_8 = data[data.Length - 8];
                    imixEnd_7 = data[data.Length - 7];
                    imixEnd_6 = data[data.Length - 6];
                    imixEnd_5 = data[data.Length - 5];
                    imixEnd_1 = data[data.Length - 1];

                    if (imixType_0 == 56 && imixType_1 == 61 && imixType_2 == 73 && imixType_3 == 77 && imixType_4 == 73 && imixType_5 == 88
                         && imixEnd_8 == 1 && imixEnd_7 == 49 && imixEnd_6 == 48 && imixEnd_5 == 61 && imixEnd_1 == 1)
                    {
                        string strGoodData = System.Text.Encoding.Default.GetString(goodData);

                        string requestTypeStart = "\x01" + "35=";
                        string requestTypeEnd = "\x01";
                        string requestTypeString = MidStrEx_New(strGoodData, requestTypeStart, requestTypeEnd);
                        int requestType = requestTypeString == "8" ? 1 : 0;

                        string idStart = "\x01" + "12087=";
                        string idEnd = "\x01";
                        string id = MidStrEx_New(strGoodData, idStart, idEnd);

                        datas.Add(goodData);
                        requestTypes.Add(requestType);

                        if (!string.IsNullOrEmpty(strGoodData.ToString()))
                        {
                            sourceDataAction.Invoke(string.Format("{0}|{1}|{2}|{3}", requestType, id, timestamp.ToOADate().ToString(), string.Empty, goodData.ToString())); ;
                        }
                    }
                }
            }

            return datas;
        }

        private List<byte[]> GetGoodData_DEP(byte[] data)
        {
            List<byte[]> datas = new List<byte[]>();

            byte b02 = 0x02;
            byte b55 = 0x55;
            byte b03 = 0x03;
            byte[] b0302 = new byte[] { 0x03, 0x02 };

            byte[] srcBytes = data;
            int index = ByteIndexOf(srcBytes, b0302);

            if (index == -1)
            {
                datas.Add(srcBytes);
            }
            else
            {
                while (index > -1)
                {
                    int i55 = srcBytes[index + 6];

                    if (i55 == 85)
                    {
                        datas.Add(srcBytes.Skip(0).Take(index + 1).ToArray());
                    }

                    srcBytes = srcBytes.Skip(index + 1).Take(srcBytes.Length - index - 1).ToArray();
                    index = ByteIndexOf(srcBytes, b0302);
                }

                if (index == -1)
                {
                    datas.Add(srcBytes);
                }
            }

            return datas;
        }
        
        private List<byte[]> GetGoodData_IMIX(byte[] data)
        {
            List<byte[]> datas = new List<byte[]>();

            byte[] bSplit = new byte[] { 0x01, 0x38, 0x3d, 0x49, 0x4d, 0x49, 0x58 };

            byte[] srcBytes = data;
            int index = ByteIndexOf(srcBytes, bSplit);

            if (index == -1)
            {
                datas.Add(srcBytes);
            }
            else
            {
                while (index > -1)
                {
                    datas.Add(srcBytes.Skip(0).Take(index + 1).ToArray());

                    srcBytes = srcBytes.Skip(index + 1).Take(srcBytes.Length - index - 1).ToArray();
                    index = ByteIndexOf(srcBytes, bSplit);
                }

                if (index == -1)
                {
                    datas.Add(srcBytes);
                }
            }

            return datas;
        }

        private static byte[] MergeData(List<DataContainer> ackDatas)
        {
            int count = 0;

            foreach (DataContainer ackData in ackDatas)
            {
                count = count + ackData.Data.Length;
            }

            byte[] returnData = new byte[count];
            int length2 = 0;

            foreach (DataContainer ackData in ackDatas)
            {
                Buffer.BlockCopy(ackData.Data, 0, returnData, length2, ackData.Data.Length);
                length2 = ackData.Data.Length;
            }

            return returnData;
        }

        // <summary>  
        /// 定位指定的 System.Byte[] 在此实例中的第一个匹配项的索引。  
        /// </summary>  
        /// <param name="srcBytes">源数组</param>  
        /// <param name="searchBytes">查找的数组</param>  
        /// <returns>返回的索引位置；否则返回值为 -1。</returns>  
        private static int ByteIndexOf(byte[] srcBytes, byte[] searchBytes)
        {
            if (srcBytes == null) { return -1; }
            if (searchBytes == null) { return -1; }
            if (srcBytes.Length == 0) { return -1; }
            if (searchBytes.Length == 0) { return -1; }
            if (srcBytes.Length < searchBytes.Length) { return -1; }

            for (int i = 0; i < srcBytes.Length - searchBytes.Length; i++)
            {
                if (srcBytes[i] == searchBytes[0])
                {
                    if (searchBytes.Length == 1) { return i; }

                    bool flag = true;

                    for (int j = 1; j < searchBytes.Length; j++)
                    {
                        if (srcBytes[i + j] != searchBytes[j])
                        {
                            flag = false;
                            break;
                        }
                    }

                    if (flag) { return i; }
                }
            }

            return -1;
        }

        //2位byte转为int
        private static int Byte2Int(byte[] b)
        {
            return ((b[0] & 0xff) << 8) | (b[1] & 0xff);
        }

        //4位byte转为int
        private static int Byte4Int(byte[] b)
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
        
            DateTime startTime =new DateTime(1970, 1, 1).ToLocalTime();

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

        public static string MidStrEx_New(string sourse, string startstr, string endstr)
        {
            Regex rg = new Regex("(?<=(" + startstr + "))[.\\s\\S]*?(?=(" + endstr + "))", RegexOptions.Multiline | RegexOptions.Singleline);

            return rg.Match(sourse).Value;
        }

        #region Old
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
            catch (Exception ex)
            {
                throw new Exception(string.Format("OnReadPacket Error, Exception: {0}", ex.Message));
            }
        }

        private byte[]? GetGoogleData(byte[] data, out int requestType)
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

        private byte[]? GetGoogleData_TCP(byte[] data, out int requestType)
        {
            requestType = 0;

            if (data == null || data.Length < 82)
            {
                return null;
            }

            int packetType = data[0];
            int tcpMessageType = data[5];
            int tcpEnd = data[data.Length - 1];

            if (packetType == 2 && tcpMessageType == 85 && tcpEnd == 3)
            {
                bool isGoodData = this.IsGoodData(data);

                if (isGoodData)
                {
                    int index = 0;
                    int tcp_start = 6;
                    int sizeLengthOfChannelName = 2;
                    int sizeChannelName = Byte2Int(data.Skip(tcp_start).Take(sizeLengthOfChannelName).ToArray());
                    int sizeLengthOfTargetInstanceName = 1;
                    index = tcp_start + sizeLengthOfChannelName + sizeChannelName;
                    int sizeTargetInstanceName = data[index];
                    //int sizeTargetInstanceName = data[tcp_start + sizeLengthOfChannelName + sizeChannelName];
                    int sizeLengthOfData = 4;
                    //int sizeData = Byte4Int(data.Skip(tcp_start + sizeLengthOfChannelName + sizeChannelName + sizeLengthOfTargetInstanceName + sizeTargetInstanceName).Take(sizeLengthOfData).ToArray());

                    index = index + sizeLengthOfTargetInstanceName + sizeTargetInstanceName + sizeLengthOfData;
                    int depapi_start = index;
                    //int depapi_start = tcp_start + sizeLengthOfChannelName + sizeChannelName + sizeLengthOfTargetInstanceName + sizeTargetInstanceName + sizeLengthOfData;
                    int messageType = data[depapi_start];
                    requestType = data[depapi_start + 23];

                    if (packetType == 2 && messageType == 7 && tcpEnd == 3 && (requestType == 0 || requestType == 1))
                    {
                        //byte[] length_osin_byte = data.Skip(depapi_start + 23 + 5).Take(2).ToArray();
                        byte[] length_osin_byte = data.Skip(depapi_start + 28).Take(2).ToArray();
                        int length_osin = Byte2Int(length_osin_byte);

                        index = depapi_start + 30 + length_osin;
                        //int dsp_begin = depapi_start + 23 + 5 + 2 + length_osin + 5;
                        int dsp_begin = index + 5;
                        //int dsp_end = dsp_begin + 4;
                        byte[] length_dspm_byte = data.Skip(dsp_begin).Take(4).ToArray();
                        int length_dspm = Byte4Int(length_dspm_byte);

                        //int dspapi_begin = depapi_start + 23 + 5 + 2 + length_osin;
                        int dspapi_begin = index;
                        int dspapi_end = dspapi_begin + length_dspm;

                        byte[] body = data.Skip(dspapi_end).Take(data.Length - dspapi_end - 1).ToArray();

                        return body;
                    }

                    return null;
                }

                return null;
            }

            return null;
        }

        private bool IsGoodData(byte[] data)
        {
            byte b02 = 0x02;
            byte b55 = 0x55;
            byte b03 = 0x03;
            byte[] b0302 = new byte[] { 0x03, 0x02 };

            byte[] srcBytes = data;
            int index = ByteIndexOf(srcBytes, b0302);

            while (index > -1)
            {
                int i55 = data[index + 6];

                if (i55 == 85)
                {
                    return false;
                }

                srcBytes = srcBytes.Skip(index + 6).Take(srcBytes.Length - index - 6).ToArray();
                index = ByteIndexOf(srcBytes, b0302);
            }

            return true;
        }
        #endregion
    }

    [Injection(InterfaceType = typeof(IResolveFileNamePrefixService), Scope = InjectionScope.Singleton)]
    public class ResolveFileNamePrefixService : IResolveFileNamePrefixService
    {
        async Task<TestCaseHistory?> IResolveFileNamePrefixService.Resolve(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return null;
            }

            fileName = Path.GetFileNameWithoutExtension(fileName);
            // 命名规则：01_{caseid}_{historyid}_{newguid}.cap
            // 01为使用CaseHistory，需要通过historyid查询history，获取它的NetGatewayDataFormat属性，返回historyid和该属性
            string[] fileName_Split = fileName.Split("_");

            if (fileName_Split.Length >= 4)
            {
                string type = fileName_Split[0];
                Guid caseID = new Guid(fileName_Split[1]);
                Guid historyID = new Guid(fileName_Split[2]);
                string newGuid = fileName_Split[3];

                switch (type)
                {
                    case "01":
                        var testCaseHistoryStore = DIContainerContainer.Get<ITestCaseHistoryStore>();
                        var testCaseHistory = await testCaseHistoryStore.QueryByCase(caseID, historyID);

                        return testCaseHistory;

                    default:
                        break;
                }
            }

            return null;
        }
    }

    [Injection(InterfaceType = typeof(IConvertNetDataFromSourceService), Scope = InjectionScope.Singleton)]
    public class ConvertNetDataFromSourceService : IConvertNetDataFromSourceService
    {
        public async Task<NetData?> Convert(string prefix, string sourceData, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(sourceData))
            {
                return await Task.FromResult<NetData?>(null);
            }

            string[] sourceData_split = sourceData.Split("|");

            if (sourceData_split.Length == 4)
            {
                NetData netData = new NetData();
                netData.Type = sourceData_split[0] == "0" ? NetDataType.Request : NetDataType.Response;
                netData.ID = sourceData_split[1];
                netData.CreateTime = DateTime.FromOADate(double.Parse(sourceData_split[2]));
                netData.RunDuration = null;

                return await Task.FromResult(netData);
            }

            return await Task.FromResult<NetData?>(null);
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

    [Injection(InterfaceType = typeof(ITotalCollectService), Scope = InjectionScope.Singleton)]
    public class TotalCollectService : ITotalCollectService
    {
        private readonly IInfluxDBEndpointRepository _influxDBEndpointRepository;

        public TotalCollectService(IInfluxDBEndpointRepository influxDBEndpointRepository)
        {
            _influxDBEndpointRepository = influxDBEndpointRepository;
        }

        public async Task Collect(string prefix, int requestCount, double minDuration, double maxDuration, double avgDuration, double avgQps, DateTime time, CancellationToken cancellationToken = default)
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
            influxDBRecord.MeasurementName = InfluxDBParameters.NetGatewayTotalMeasurementName;
            TimeSpan ts = time - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            influxDBRecord.Timestamp = Convert.ToInt64(((long)(ts.TotalMilliseconds)).ToString().PadRight(19, '0'));
            influxDBRecord.Tags.Add("HistoryCaseID", prefix);
            influxDBRecord.Fields.Add("MaxDuration", maxDuration.ToString());
            influxDBRecord.Fields.Add("MinDurartion", minDuration.ToString());
            influxDBRecord.Fields.Add("AvgDuration", avgDuration.ToString());
            influxDBRecord.Fields.Add("RequestCount", requestCount.ToString());
            influxDBRecord.Fields.Add("AvgQPS", avgQps.ToString());
            await influxDBEndpoint.AddData(InfluxDBParameters.DBName, influxDBRecord);
        }
    }
}
