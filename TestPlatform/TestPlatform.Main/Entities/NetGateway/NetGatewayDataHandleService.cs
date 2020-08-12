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

namespace FW.TestPlatform.Main.NetGateway
{
    [Injection(InterfaceType = typeof(INetGatewayDataHandleService), Scope = InjectionScope.Singleton)]
    public class NetGatewayDataHandleService : INetGatewayDataHandleService
    {
        private static int _maxFileCount = 10;
        private readonly INetGatewayDataHandleConfigurationService _netGatewayDataHandleConfigurationService;
        private readonly IResolveFileNamePrefixService _resolveFileNamePrefixService;
        private readonly IGetSourceDataFromStreamService _getSourceDataFromStreamService;
        private readonly IConvertNetDataFromSourceService _convertNetDataFromSourceService;
        private readonly IQPSCollectService _qpsCollectService;
        private readonly INetDurationCollectService _netDurationCollectService;

        public static string LoggerCategoryName { get; set; } = "NetGatewayDataHandle";

        public NetGatewayDataHandleService(INetGatewayDataHandleConfigurationService netGatewayDataHandleConfigurationService, IResolveFileNamePrefixService resolveFileNamePrefixService, IGetSourceDataFromStreamService getSourceDataFromStreamService, IConvertNetDataFromSourceService convertNetDataFromSourceService, IQPSCollectService qpsCollectService, INetDurationCollectService netDurationCollectService)
        {
            _netGatewayDataHandleConfigurationService = netGatewayDataHandleConfigurationService;
            _resolveFileNamePrefixService = resolveFileNamePrefixService;
            _getSourceDataFromStreamService = getSourceDataFromStreamService;
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

            var t1=listenFileCompleted(netGatewayDataHandleResult,folderPath, completedFileNames, (info) =>
             {
                 lock (completedFiles)
                 {
                     completedFiles[info.FileName]=info;
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

                                  await using (var stream = File.OpenRead(fileName))
                                  {
                                      await _getSourceDataFromStreamService.Get(stream,
                                          async (sourceData) =>
                                          {
                                              var data = await _convertNetDataFromSourceService.Convert(prefix,sourceData, cancellationToken);

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

                                      stream.Close();
                                  }

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
                                      await _netDurationCollectService.Collect(minResponse, maxResponse, avgResponse, maxCreateTime!.Value, cancellationToken);
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



        private Task listenFileCompleted(NetGatewayDataHandleResultDefault result,string folderName, Dictionary<string, string> completedFileNames, Action<FileDataInfo> completedAction)
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
}
