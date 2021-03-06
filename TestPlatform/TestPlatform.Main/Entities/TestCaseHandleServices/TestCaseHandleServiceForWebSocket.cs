﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime;
using Newtonsoft.Json.Linq;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.Thread;
using MSLibrary.Serializer;
using MSLibrary.LanguageTranslate;
using MSLibrary.CommandLine.SSH;
using FW.TestPlatform.Main.Template.LabelParameterHandlers;
using FW.TestPlatform.Main.Configuration;
using System.Text.RegularExpressions;
using MSLibrary.CommandLine;
using FW.TestPlatform.Main.Entities.DAL;
using System.Linq;
using MSLibrary.Collections;
using MSLibrary.Collections.DAL;

namespace FW.TestPlatform.Main.Entities.TestCaseHandleServices
{
    [Injection(InterfaceType = typeof(TestCaseHandleServiceForWebSocket), Scope = InjectionScope.Singleton)]
    public class TestCaseHandleServiceForWebSocket : ITestCaseHandleService
    {
        private const string _testFilePath = "/usr/testfile/";
        private const string _testFileName = "script{0}.py";
        private const string _testLogFileName = "log{0}";
        private const string _testOutFileName = "out{0}";
        private const int _maxLogSize = 1024 * 1024;

        private readonly ITestDataSourceRepository _testDataSourceRepository;
        private readonly IScriptTemplateRepository _scriptTemplateRepository;
        private readonly ISSHEndpointRepository _sshEndpointRepository;
        private readonly ISystemConfigurationService _systemConfigurationService;
        private readonly ITestCaseStore _testCaseStore;
        private readonly ITreeEntityStore _treeEntityStore;


        /// <summary>
        /// 要使用的附加函数名称集合
        /// 系统初始化时注入
        /// </summary>
        public static IList<string> AdditionFuncNames { get; set; } = new List<string>();

        public TestCaseHandleServiceForWebSocket(ITestDataSourceRepository testDataSourceRepository, IScriptTemplateRepository scriptTemplateRepository, ISSHEndpointRepository sshEndpointRepository, ISystemConfigurationService systemConfigurationService, ITestCaseStore testCaseStore, ITreeEntityStore treeEntityStore)
        {
            _testDataSourceRepository = testDataSourceRepository;
            _scriptTemplateRepository = scriptTemplateRepository;
            _sshEndpointRepository = sshEndpointRepository;
            _systemConfigurationService = systemConfigurationService;
            _testCaseStore = testCaseStore;
            _treeEntityStore = treeEntityStore;
        }

        public async Task<string> GetMasterLog(TestCase tCase, TestHost host, CancellationToken cancellationToken = default)
        {
            var configuration = JsonSerializerHelper.Deserialize<ConfigurationData>(tCase.Configuration);
            string path = this.GetTestFilePath(configuration);

            bool fileExisted = await host.SSHEndpoint.ExistsFile($"{path}{string.Format(_testLogFileName, string.Empty)}", 10, cancellationToken);
            if (!fileExisted)
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.NotFoundLogFileByPath,
                    DefaultFormatting = "找不到路径为{0}的日志文件",
                    ReplaceParameters = new List<object>() { $"{path}{string.Format(_testLogFileName, string.Empty)}" }
                };

                throw new UtilityException((int)TestPlatformErrorCodes.NotFoundLogFileByPath, fragment, 1, 0);
            }
            string result = string.Empty;
            await host.SSHEndpoint.DownloadFile(
                async(fileStream)=>
                {
                    fileStream.Position = 0;
                    byte[] bytes = new byte[_maxLogSize];

                    Memory<byte> memoryBytes=bytes.AsMemory();

                    var realSize=await fileStream.ReadAsync(memoryBytes);
                    result=UTF8Encoding.UTF8.GetString(memoryBytes.Slice(0, realSize).Span);
                },
                $"{path}{string.Format(_testLogFileName,string.Empty)}",10,
                cancellationToken
                );
            return result;
        }

        public async Task<string> GetSlaveLog(TestCase tCase, TestHost host, int idx, CancellationToken cancellationToken = default)
        {
            var configuration = JsonSerializerHelper.Deserialize<ConfigurationData>(tCase.Configuration);
            string path = this.GetTestFilePath(configuration);

            //await host.SSHEndpoint.ExecuteCommand($"cat {path}{string.Format(_testLogFileName, "_slave_*")} > {path}{string.Format(_testLogFileName, "_slave")}", 10, cancellationToken);
            bool fileExisted = await host.SSHEndpoint.ExistsFile($"{path}{string.Format(_testLogFileName, "_slave_" + idx)}",10,cancellationToken);
            if (!fileExisted)
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.NotFoundLogFileByPath,
                    DefaultFormatting = "找不到路径为{0}的日志文件",
                    ReplaceParameters = new List<object>() { $"{path}{string.Format(_testLogFileName, "_slave_" + idx)}" }
                };

                throw new UtilityException((int)TestPlatformErrorCodes.NotFoundLogFileByPath, fragment, 1, 0);
            }
            //下载日志文件
            string result = string.Empty;
            await host.SSHEndpoint.DownloadFile(
                async (fileStream) =>
                {
                    fileStream.Position = 0;
                    byte[] bytes = new byte[_maxLogSize];

                    Memory<byte> memoryBytes = bytes.AsMemory();

                    var realSize = await fileStream.ReadAsync(memoryBytes);
                    result = UTF8Encoding.UTF8.GetString(memoryBytes.Slice(0, realSize).Span);
                },
                $"{path}{string.Format(_testLogFileName,"_slave_" + idx)}",10,
                cancellationToken
                );
            return result;
        }

        public async Task<bool> IsEngineRun(TestCase tCase, CancellationToken cancellationToken = default)
        {
            var configuration = JsonSerializerHelper.Deserialize<ConfigurationData>(tCase.Configuration);
            int port = this.GetPort(configuration);

            //执行主机查进程命令
            var result =await tCase.MasterHost.SSHEndpoint.ExecuteCommand($"ps -ef | grep locust | grep master-bind-port | grep {port.ToString()} | grep -v grep | awk '{{print $2}}'",10, cancellationToken);
            if (string.IsNullOrEmpty(result))
            {
                return false;
            }

            return true;
        }

        public async Task Run(TestCase tCase, CancellationToken cancellationToken = default)
        {
            List<TestCase> conflictedCases = await StatusCheck(tCase, cancellationToken);
            if (conflictedCases.Count > 0)
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.TestHostHasRunning,
                    DefaultFormatting = "名称为{0}的测试主机端口已经被{1}运行的测试用例使用",
                    ReplaceParameters = new List<object>() { tCase.Name, string.Join(",", conflictedCases.Select(tc => tc.Name).ToArray()) }
                };
                throw new UtilityException((int)TestPlatformErrorCodes.TestHostPortIsUsed, fragment, 1, 0);
            }
            var configuration = JsonSerializerHelper.Deserialize<ConfigurationData>(tCase.Configuration);

            var scriptTemplate=await _scriptTemplateRepository.QueryByName(ScriptTemplateNames.LocustWebSocket, cancellationToken);
            
            if (scriptTemplate==null)
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.NotFoundScriptTemplateByName,
                    DefaultFormatting = "找不到名称为{0}的脚本模板",
                    ReplaceParameters = new List<object>() { ScriptTemplateNames.LocustWebSocket }
                };

                throw new UtilityException((int)TestPlatformErrorCodes.NotFoundScriptTemplateByName, fragment, 1, 0);
            }

            var caseServiceBaseAddress = await _systemConfigurationService.GetCaseServiceBaseAddressAsync(cancellationToken);

            var contextDict= new Dictionary<string, object>();

            //将CaseID加入到模板上下文中
            contextDict.Add(TemplateContextParameterNames.CaseID, tCase.ID);
            //将CaseService基地址加入到模板上下文中
            contextDict.Add(TemplateContextParameterNames.CaseServiceBaseAddress, caseServiceBaseAddress);
            //将引擎类型加入到模板上下文中
            contextDict.Add(TemplateContextParameterNames.EngineType, RuntimeEngineTypes.Locust);
            //将请求体模板加入到模板上下文中
            contextDict.Add(TemplateContextParameterNames.RequestBody, configuration.RequestBody);
            //将响应分隔符加入到模板上下文中
            contextDict.Add(TemplateContextParameterNames.ResponseSeparator, configuration.ResponseSeparator);
            //将预热时间加入到模板上下文中
            contextDict.Add(TemplateContextParameterNames.ReadyTime, configuration.ReadyTime);
            //将测试地址加入到模板上下文中
            contextDict.Add(TemplateContextParameterNames.Address, configuration.Address);
            //将测试端口加入到模板上下文中
            contextDict.Add(TemplateContextParameterNames.Port, configuration.Port);

            //将要用到的附加函数名称集合加入到模板上下文中
            contextDict.Add(TemplateContextParameterNames.AdditionFuncNames, AdditionFuncNames);
            //将数据源变量配置加入到模板上下文中
            contextDict.Add(TemplateContextParameterNames.DataSourceVars, configuration.DataSourceVars);
            //将Tcp连接初始化脚本配置加入到模板上下文中
            contextDict.Add(TemplateContextParameterNames.ConnectInit, configuration.ConnectInit);
            //将Tcp发送前初始化脚本配置加入到模板上下文中
            contextDict.Add(TemplateContextParameterNames.SendInit, configuration.SendInit);
            //将Tcp停止前初始化脚本配置加入到模板上下文中
            contextDict.Add(TemplateContextParameterNames.StopInit, configuration.StopInit);

            //为DataSourceVars补充Data属性
            if (configuration.DataSourceVars.Count > 0)
            {
                if (tCase.TreeID == null)
                {
                    var fragment = new TextFragment()
                    {
                        Code = TestPlatformTextCodes.NotFoundTreeEntity,
                        DefaultFormatting = "运行测试案例,名称为{0}的测试案例所对应的目录节点实体为null",
                        ReplaceParameters = new List<object>() { tCase.Name }
                    };

                    throw new UtilityException((int)TestPlatformErrorCodes.NotFoundTreeEntity, fragment, 1, 0);
                }
                TreeEntity? tCaseTreeEntity = await _treeEntityStore.QueryByID((Guid)tCase.TreeID);
                if (tCaseTreeEntity == null)
                {
                    var fragment = new TextFragment()
                    {
                        Code = TestPlatformTextCodes.NotFoundTreeEntityByID,
                        DefaultFormatting = "运行名称为{0}的测试用例，找不到此测试案例对应的ID为{1}的目录节点实体",
                        ReplaceParameters = new List<object>() { tCase.Name, tCase.TreeID }
                    };

                    throw new UtilityException((int)TestPlatformErrorCodes.NotFoundTreeEntityByID, fragment, 1, 0);
                }
                await ParallelHelper.ForEach(configuration.DataSourceVars, 10,
                    async (item) =>
                    {
                        var dataSource = await _testDataSourceRepository.QueryByTreeEntityNameAndParentID(tCaseTreeEntity.ParentID, item.DataSourceName, cancellationToken);
                        if (dataSource == null && tCaseTreeEntity.ParentID != null)
                        {
                            dataSource = await _testDataSourceRepository.QueryByTreeEntityNameAndParentID(null, item.DataSourceName, cancellationToken);
                        }
                        if (dataSource == null)
                        {
                            var fragment = new TextFragment()
                            {
                                Code = TestPlatformTextCodes.NotFoundTestDataSourceByName,
                                DefaultFormatting = "运行名称为{0}的测试用例,在当前测试用例的同级目录或者根目录下找不到名称为{1}的测试数据源",
                                ReplaceParameters = new List<object>() { tCase.Name, item.DataSourceName }
                            };

                            throw new UtilityException((int)TestPlatformErrorCodes.NotFoundTestDataSourceByName, fragment, 1, 0);
                        }
                        //var dataSource = await _testDataSourceRepository.QueryByName(item.DataSourceName, cancellationToken);

                        //if (dataSource == null)
                        //{
                        //    var fragment = new TextFragment()
                        //    {
                        //        Code = TestPlatformTextCodes.NotFoundTestDataSourceByName,
                        //        DefaultFormatting = "找不到名称为{0}的测试数据源",
                        //        ReplaceParameters = new List<object>() { item.DataSourceName }
                        //    };

                        //    throw new UtilityException((int)TestPlatformErrorCodes.NotFoundTestDataSourceByName, fragment, 1, 0);
                        //}

                        item.Type = dataSource.Type;
                        item.Data = dataSource.Data;
                    }
                );
            }

            //生成代码
            var strCode=await scriptTemplate.GenerateScript(contextDict, cancellationToken);

            // 替换生成代码中的固定标签
            strCode = strCode.Replace("{Address}", configuration.Address);
            strCode = strCode.Replace("{Port}", configuration.Port.ToString());
            strCode = strCode.Replace("{CaseID}", tCase.ID.ToString());
            strCode = strCode.Replace("{ResponseSeparator}", configuration.ResponseSeparator);
            strCode = strCode.Replace("{CaseServiceBaseAddress}", caseServiceBaseAddress);
            strCode = strCode.Replace("{IsPrintLog}", configuration.IsPrintLog ? "True" : "False");
            strCode = strCode.Replace("{SyncType}", configuration.SyncType ? "True" : "False");

            string path = this.GetTestFilePath(configuration);

            await tCase.MasterHost.SSHEndpoint.ExecuteCommand($"rm -r {path}");
            await tCase.MasterHost.SSHEndpoint.ExecuteCommand($"mkdir {path}", 30, cancellationToken);

            #region 检查主机端口是否被占用，并强制终止
            //await this.Stop(tCase, cancellationToken);
            #endregion

            //代码模板必须有一个格式为{SlaveName}的替换符，该替换符标识每个Slave

            //获取测试用例的主测试机，上传测试代码
            using (var textStream=new MemoryStream(UTF8Encoding.UTF8.GetBytes(strCode.Replace("{SlaveName}", "Master"))))
            {
                #region Test Code
#if DEBUG
                //string testFilePath = @"E:\Downloads\script.py";

                //if (File.Exists(testFilePath))
                //{
                //    File.Delete(testFilePath);
                //}

                //using (FileStream fileStream = new FileStream(testFilePath, FileMode.CreateNew, FileAccess.Write, FileShare.Write))
                //{
                //    BinaryWriter w = new BinaryWriter(fileStream);
                //    w.Write(textStream.ToArray());
                //}
#endif
                #endregion

                await tCase.MasterHost.SSHEndpoint.UploadFile(textStream, $"{path}{string.Format(_testFileName,string.Empty)}", 30, cancellationToken);
                textStream.Close();
            }

            //获取测试用例的所有从属测试机，上传测试代码
            var slaveHosts=tCase.GetAllSlaveHosts(cancellationToken);
            int slaveCount = 0;
            object lockObj = new object();
            List<TestCaseSlaveHost> slaveHostList = new List<TestCaseSlaveHost>();

            await ParallelHelper.ForEach(slaveHosts, 10,
                async (item) =>
                {
                    await item.Host.SSHEndpoint.ExecuteCommand($"mkdir {path}", 30, cancellationToken);

                    //先删除文件夹内现有的所有文件
                    await item.Host.SSHEndpoint.ExecuteCommand($"rm -rf {path}{string.Format(_testFileName, "_*")}", 30, cancellationToken);

                    //为该Slave测试机下的每个Slave上传文件

                    try
                    {                        
                        await item.Host.SSHEndpoint.UploadFile(
                            async (service) =>
                            {
                                var index = 0;
                                for (index = 0; index <= item.Count - 1; index++)
                                {
                                    using (var textStream = new MemoryStream(UTF8Encoding.UTF8.GetBytes(strCode.Replace("{SlaveName}", $"{item.SlaveName}-{index.ToString()}"))))
                                    {
                                        await service.Upload(textStream, $"{path}{string.Format(_testFileName, $"_{index.ToString()}")}");
                                        textStream.Close();
                                    }
                                }
                            }
                            , 30,
                            cancellationToken
                          );
                        
                    }
                    catch(UtilityException ex)
                    {
                        if (ex.Code==(int)CommandLineErrorCodes.SSHOperationTimeout)
                        {
                            var fragment = new TextFragment()
                            {
                                Code = TestPlatformTextCodes.SlaveHostUploadTestFileTimeout,
                                DefaultFormatting = "从测试机{0}上传测试文件超时",
                                ReplaceParameters = new List<object>() { item.SlaveName }
                            };
                            throw new UtilityException((int)TestPlatformErrorCodes.SlaveHostUploadTestFileTimeout, fragment, 1, 0);
                        }
                        else
                        {
                            throw;
                        }
                    }

                    lock (lockObj)
                    {
                        slaveCount += item.Count;
                        slaveHostList.Add(item);
                    }
                });

            //执行主机测试命令
            List<Func<string?, Task<string>>> commands = new List<Func<string?, Task<string>>>()
            {
               async (preResult)=>
               {
                   return await Task.FromResult($"rm -rf {path}{string.Format(_testLogFileName,string.Empty)}");
               },
               async (preResult)=>
               {
                   //return await Task.FromResult($"locust -f {path}{string.Format(_testFileName,string.Empty)} --master --expect-slaves={slaveCount.ToString()} --no-web --run-time={  configuration.Duration.ToString()} --logfile={path}{string.Format(_testLogFileName,string.Empty)} --clients={configuration.UserCount.ToString()} --hatch-rate={configuration.PerSecondUserCount.ToString()} &");
                   //return await Task.FromResult($"locust -f {path}{string.Format(_testFileName,string.Empty)} --logfile {path}{string.Format(_testLogFileName,string.Empty)} --master --headless --expect-workers {slaveCount.ToString()} -t {configuration.Duration.ToString()} -u {configuration.UserCount.ToString()} -r {configuration.PerSecondUserCount.ToString()} > {path}{string.Format(_testOutFileName,string.Empty)} 2>&1 &");
                   return await Task.FromResult($"locust -f {path}{string.Format(_testFileName,string.Empty)} --master --headless --expect-workers {slaveCount.ToString()} --master-bind-port {this.GetPort(configuration)} -t {configuration.Duration.ToString()} -u {configuration.UserCount.ToString()} -r {configuration.PerSecondUserCount.ToString()} > {path}{string.Format(_testLogFileName,string.Empty)} 2>&1 &");
               }
            };
            await tCase.MasterHost.SSHEndpoint.ExecuteCommandBatch(commands, 30, cancellationToken);

            //执行从属机测试命令
            foreach(var item in slaveHostList)
            {
                List<Func<string?, Task<string>>> slaveCommands = new List<Func<string?, Task<string>>>()
                    {
                        async (preResult)=>
                        {
                            return await Task.FromResult($"rm -rf {path}{string.Format(_testLogFileName, "_slave*")}");
                        }
                    };


                //await item.Host.SSHEndpoint.ExecuteCommand($"rm -rf {path}{string.Format(_testLogFileName, "_slave")}", cancellationToken);
                for (var index = 0;index <= item.Count - 1;index++)
                {
                    var innerIndex = index;
                    slaveCommands.Add(
                        async (preResult) =>
                        {
                            //return await Task.FromResult($"locust -f {path}{string.Format(_testFileName, $"_{index.ToString()}")} --slave --master-host={tCase.MasterHost.Address} --no-web --run-time={  configuration.Duration.ToString()} --logfile={path}{string.Format(_testLogFileName, "_slave")} --clients={configuration.UserCount.ToString()} --hatch-rate={configuration.PerSecondUserCount.ToString()} &");
                            //return await Task.FromResult($"locust -f {path}{string.Format(_testFileName, $"_{innerIndex.ToString()}")} --logfile {path}{string.Format(_testLogFileName, $"_slave")} --worker --headless --master-host {tCase.MasterHost.Address} --master-port 5557 > {path}{string.Format(_testOutFileName, $"_slave_")}{innerIndex.ToString()} 2>&1 &");
                            //return await Task.FromResult($"locust -f {path}{string.Format(_testFileName, $"_{innerIndex.ToString()}")} --worker --headless --master-host {tCase.MasterHost.Address} --master-port 5557 > {path}{string.Format(_testLogFileName, $"_slave")} 2>&1 &");
                            return await Task.FromResult($"locust -f {path}{string.Format(_testFileName, $"_{innerIndex.ToString()}")} --worker --headless --master-host {tCase.MasterHost.Address} --master-port {this.GetPort(configuration)} > {path}{string.Format(_testLogFileName, $"_slave_")}{innerIndex.ToString()} 2>&1 &");
                            //return await Task.FromResult($"locust -f {path}{string.Format(_testFileName, $"_{innerIndex.ToString()}")} --logfile {path}{string.Format(_testLogFileName, $"_slave")} --worker --headless --master-host 127.0.0.1 --master-port {configuration.LocustMasterBindPort.ToString()} > {path}{string.Format(_testOutFileName, $"_slave_")}{innerIndex.ToString()} 2>&1 &");
                            //return await Task.FromResult($"locust -f {path}{string.Format(_testFileName, $"_{innerIndex.ToString()}")} --worker --headless --master-host 127.0.0.1 --master-port {configuration.LocustMasterBindPort.ToString()} > {path}{string.Format(_testLogFileName, $"_slave")} 2>&1 &");
                        }
                   );                  
                }

                await item.Host.SSHEndpoint.ExecuteCommandBatch(slaveCommands, 30, cancellationToken);
            }

        }

        public async Task Stop(TestCase tCase, CancellationToken cancellationToken = default)
        {
            //List<TestCase> conflictedCases = await StatusCheck(tCase, cancellationToken);
            //if (conflictedCases.Count == 0)
            //{
            var configuration = JsonSerializerHelper.Deserialize<ConfigurationData>(tCase.Configuration);
            int port = this.GetPort(configuration);

            //执行主机杀进程命令
            await tCase.MasterHost.SSHEndpoint.ExecuteCommand($"ps -ef | grep locust | grep master-bind-port | grep {port.ToString()} | grep -v grep | awk '{{print $2}}' | xargs kill -9", 10, cancellationToken);
            //执行slave杀进程命令
            var slaveHosts = tCase.GetAllSlaveHosts(cancellationToken);
            await foreach (var item in slaveHosts)
            {
                await item.Host.SSHEndpoint.ExecuteCommand($"ps -ef | grep locust | grep master-port | grep {port.ToString()} | grep -v grep | awk '{{print $2}}' | xargs kill -9", 10, cancellationToken);
            }
            //}
        }

        #region Private
        /// <summary>
        /// 获取Path
        /// </summary>
        /// <param name="configurationData"></param>
        /// <returns></returns>
        private string GetTestFilePath(ConfigurationData configurationData)
        {
            int port = this.GetPort(configurationData);
            string path = $"{_testFilePath}{port.ToString()}/";

            return path;
        }

        /// <summary>
        /// 获取Port
        /// </summary>
        /// <param name="configurationData"></param>
        /// <returns></returns>
        private int GetPort(ConfigurationData configurationData)
        {
            int locustMasterBindPort = configurationData.LocustMasterBindPort;

            if (locustMasterBindPort <= 5557)
            {
                locustMasterBindPort = 5557;
            }

            return locustMasterBindPort;
        }

        public async Task<List<TestCase>> StatusCheck(TestCase tCase, CancellationToken cancellationToken = default)
        {
            List<TestCase> conflictedCases = new List<TestCase>();
            var existsCases = await _testCaseStore.QueryCountNolockByStatus(TestCaseStatus.Running, new List<Guid>() { tCase.MasterHostID }, cancellationToken);
            if (existsCases.Count >= 1)
            {
                var configuration = JsonSerializerHelper.Deserialize<ConfigurationData>(tCase.Configuration);

                foreach (TestCase exiestedCase in existsCases)
                {
                    var existedConfiguration = JsonSerializerHelper.Deserialize<ConfigurationData>(exiestedCase.Configuration);
                    if (existedConfiguration.LocustMasterBindPort == configuration.LocustMasterBindPort)
                    {
                        conflictedCases.Add(exiestedCase);
                    }
                }
            }
            return conflictedCases;
        }
        #endregion
    }



    //[DataContract]
    //public class ConfigurationData
    //{
    //    /// <summary>
    //    /// 用户数量
    //    /// </summary>
    //    [DataMember]
    //    public int UserCount
    //    {
    //        get; set;
    //    }

    //    /// <summary>
    //    /// 每秒增加用户数量
    //    /// </summary>
    //    [DataMember]
    //    public int PerSecondUserCount
    //    {
    //        get; set;
    //    }

    //    /// <summary>
    //    /// 持续时间（秒）
    //    /// </summary>
    //    [DataMember]
    //    public int Duration
    //    {
    //        get; set;
    //    }

    //    /// <summary>
    //    /// 预热时间（秒）
    //    /// </summary>
    //    [DataMember]
    //    public int ReadyTime
    //    {
    //        get; set;
    //    }

    //    /// <summary>
    //    /// 测试接口地址
    //    /// </summary>
    //    [DataMember]
    //    public string Address
    //    {
    //        get; set;
    //    } = null!;

    //    /// <summary>
    //    /// 测试接口地址
    //    /// </summary>
    //    [DataMember]
    //    public int Port
    //    {
    //        get; set;
    //    }

    //    /// <summary>
    //    /// Tcp连接初始化脚本配置
    //    /// </summary>
    //    [DataMember]
    //    public ConfigurationDataForTcpConnectInit ConnectInit
    //    {
    //        get; set;
    //    } = null!;

    //    /// <summary>
    //    /// Tcp发送前初始化脚本配置
    //    /// </summary>
    //    [DataMember]
    //    public ConfigurationDataForTcpSendInit SendInit
    //    {
    //        get; set;
    //    } = null!;

    //    /// <summary>
    //    /// Tcp发送前初始化脚本配置
    //    /// </summary>
    //    [DataMember]
    //    public ConfigurationDataForTcpStopInit StopInit
    //    {
    //        get; set;
    //    } = null!;

    //    /// <summary>
    //    /// 请求体内容
    //    /// </summary>
    //    [DataMember]
    //    public string RequestBody
    //    {
    //        get; set;
    //    } = null!;

    //    /// <summary>
    //    /// 数据源变量配置
    //    /// </summary>
    //    [DataMember]
    //    public List<ConfigurationDataForDataSourceVar> DataSourceVars
    //    {
    //        get; set;
    //    } = new List<ConfigurationDataForDataSourceVar>();

    //    /// <summary>
    //    /// 响应内容分隔符
    //    /// </summary>
    //    [DataMember]
    //    public string ResponseSeparator
    //    {
    //        get; set;
    //    } = null!;

    //    /// <summary>
    //    /// 是否打印日志
    //    /// </summary>
    //    [DataMember]
    //    public bool IsPrintLog
    //    {
    //        get; set;
    //    } = false;

    //    /// <summary>
    //    /// 同步类型
    //    /// </summary>
    //    [DataMember]
    //    public bool SyncType
    //    {
    //        get; set;
    //    } = true;
    //}

    ///// <summary>
    ///// Tcp连接初始化脚本配置
    ///// </summary>
    //[DataContract]
    //public class ConfigurationDataForTcpConnectInit
    //{
    //    /// <summary>
    //    /// 变量赋值配置
    //    /// </summary>
    //    [DataMember]
    //    public List<ConfigurationDataForVar> VarSettings
    //    {
    //        get; set;
    //    } = new List<ConfigurationDataForVar>();
    //}

    ///// <summary>
    ///// Tcp发送前初始化脚本配置
    ///// </summary>
    //[DataContract]
    //public class ConfigurationDataForTcpSendInit
    //{
    //    /// <summary>
    //    /// 变量赋值配置
    //    /// </summary>
    //    [DataMember]
    //    public List<ConfigurationDataForVar> VarSettings
    //    {
    //        get; set;
    //    } = new List<ConfigurationDataForVar>();
    //}

    ///// <summary>
    ///// Tcp发送前初始化脚本配置
    ///// </summary>
    //[DataContract]
    //public class ConfigurationDataForTcpStopInit
    //{
    //    /// <summary>
    //    /// 变量赋值配置
    //    /// </summary>
    //    [DataMember]
    //    public List<ConfigurationDataForVar> VarSettings
    //    {
    //        get; set;
    //    } = new List<ConfigurationDataForVar>();
    //}

    ///// <summary>
    ///// 变量赋值配置
    ///// </summary>
    //[DataContract]
    //public class ConfigurationDataForVar
    //{
    //    [DataMember]
    //    public string Name { get; set; } = null!;

    //    [DataMember]
    //    public string Content { get; set; } = null!;
    //}

    ///// <summary>
    ///// 数据源变量
    ///// </summary>
    //[DataContract]
    //public class ConfigurationDataForDataSourceVar
    //{
    //    /// <summary>
    //    /// 变量名称
    //    /// </summary>
    //    [DataMember]
    //    public string Name { get; set; } = null!;

    //    /// <summary>
    //    /// 变量类型
    //    /// 来源自数据源
    //    /// 配置时不需要配置此属性
    //    /// </summary>
    //    public string Type { get; set; } = null!;

    //    /// <summary>
    //    /// 数据源名称
    //    /// </summary>
    //    [DataMember]
    //    public string DataSourceName { get; set; } = null!;

    //    /// <summary>
    //    /// 数据
    //    /// 配置时不需要配置此属性
    //    /// </summary>
    //    public string Data { get; set; } = string.Empty;
    //}

}
