using System;
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
using MSLibrary.StreamingDB.InfluxDB;

namespace FW.TestPlatform.Main.Entities.TestCaseHandleServices
{
    [Injection(InterfaceType = typeof(TestCaseHandleServiceForJmeter), Scope = InjectionScope.Singleton)]
    public class TestCaseHandleServiceForJmeter : ITestCaseHandleService
    {
        private const string _testFilePath = "/usr/testfile/";
        private const string _testFileName = "script{0}.jmx";
        private const string _testLogFileName = "log{0}.jtl";
        private const string _testResultFileName = "resultstatvisualizer{0}.csv";
        private const string _testOutFileName = "out{0}";
        private const int _maxLogSize = 1024 * 1024;

        private readonly ITestDataSourceRepository _testDataSourceRepository;
        private readonly IScriptTemplateRepository _scriptTemplateRepository;
        private readonly ISSHEndpointRepository _sshEndpointRepository;
        private readonly ISystemConfigurationService _systemConfigurationService;
        private readonly IInfluxDBEndpointRepository _influxDBEndpointRepository;
        private readonly ITestCaseStore _testCaseStore;

        /// <summary>
        /// 要使用的附加函数名称集合
        /// 系统初始化时注入
        /// </summary>
        public static IList<string> AdditionFuncNames { get; set; } = new List<string>();

        public TestCaseHandleServiceForJmeter(ITestDataSourceRepository testDataSourceRepository, IScriptTemplateRepository scriptTemplateRepository, ISSHEndpointRepository sshEndpointRepository, ISystemConfigurationService systemConfigurationService, IInfluxDBEndpointRepository influxDBEndpointRepository, ITestCaseStore testCaseStore)
        {
            _testDataSourceRepository = testDataSourceRepository;
            _scriptTemplateRepository = scriptTemplateRepository;
            _sshEndpointRepository = sshEndpointRepository;
            _systemConfigurationService = systemConfigurationService;
            _influxDBEndpointRepository = influxDBEndpointRepository;
            _testCaseStore = testCaseStore;
        }

        public async Task<string> GetMasterLog(TestCase tCase, TestHost host, CancellationToken cancellationToken = default)
        {
            string path = this.GetTestFilePath(tCase.ID.ToString());

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
            string path = this.GetTestFilePath(tCase.ID.ToString());

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
            string id = tCase.ID.ToString();

            //执行主机查进程命令
            var result =await tCase.MasterHost.SSHEndpoint.ExecuteCommand($"ps -ef | grep jmeter | grep {id} | grep -v grep | awk '{{print $2}}'",10, cancellationToken);
            
            if (string.IsNullOrEmpty(result))
            {
                return false;
            }

            return true;
        }

        public async Task Run(TestCase tCase, CancellationToken cancellationToken = default)
        {
            var configuration = JsonSerializerHelper.Deserialize<ConfigurationDataJmeter>(tCase.Configuration);

            var scriptTemplate = await _scriptTemplateRepository.QueryByName(ScriptTemplateNames.Jmeter, cancellationToken);

            if (scriptTemplate == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.NotFoundScriptTemplateByName,
                    DefaultFormatting = "找不到名称为{0}的脚本模板",
                    ReplaceParameters = new List<object>() { ScriptTemplateNames.Jmeter }
                };

                throw new UtilityException((int)TestPlatformErrorCodes.NotFoundScriptTemplateByName, fragment, 1, 0);
            }

            var caseServiceBaseAddress = await _systemConfigurationService.GetCaseServiceBaseAddressAsync(cancellationToken);

            var influxDBEndpoint = await _influxDBEndpointRepository.QueryByName(InfluxDBParameters.EndpointName, cancellationToken);

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

            var contextDict = new Dictionary<string, object>();

            //将CaseID加入到模板上下文中
            contextDict.Add(TemplateContextParameterNames.CaseID, tCase.ID);
            //将CaseService基地址加入到模板上下文中
            contextDict.Add(TemplateContextParameterNames.CaseServiceBaseAddress, caseServiceBaseAddress);
            //将引擎类型加入到模板上下文中
            contextDict.Add(TemplateContextParameterNames.EngineType, RuntimeEngineTypes.Jmeter);

            //将数据源变量配置加入到模板上下文中
            contextDict.Add(TemplateContextParameterNames.DataSourceVars, configuration.DataSourceVars);

            //为DataSourceVars补充Data属性

            await ParallelHelper.ForEach(configuration.DataSourceVars, 10,
                async (item) =>
                {
                    var dataSource = await _testDataSourceRepository.QueryByName(item.DataSourceName, cancellationToken);

                    if (dataSource == null)
                    {
                        var fragment = new TextFragment()
                        {
                            Code = TestPlatformTextCodes.NotFoundTestDataSourceByName,
                            DefaultFormatting = "找不到名称为{0}的测试数据源",
                            ReplaceParameters = new List<object>() { item.DataSourceName }
                        };

                        throw new UtilityException((int)TestPlatformErrorCodes.NotFoundTestDataSourceByName, fragment, 1, 0);
                    }

                    item.Type = dataSource.Type;
                    item.Data = dataSource.Data;
                }
            );

            //生成代码
            var strCode = await scriptTemplate.GenerateScript(contextDict, cancellationToken);
            string end = "    </hashTree>\n  </hashTree>\n</jmeterTestPlan>";
            strCode = configuration.FileContent.Replace(end, strCode + end);

            // 替换生成代码中的固定标签
            strCode = strCode.Replace("{CaseID}", tCase.ID.ToString());
            List<Match> matchs = this.GetIPPort(caseServiceBaseAddress);

            if (matchs != null && matchs.Count >= 2)
            {
                string ipport = matchs[1].ToString();
                strCode = strCode.Replace("{CaseServiceBaseIP}", ipport.Substring(0, ipport.IndexOf(":")));
                strCode = strCode.Replace("{CaseServiceBasePort}", ipport.Substring(ipport.IndexOf(":") + 1));
            }

            strCode = strCode.Replace("{InfluxDBAddress}", influxDBEndpoint == null ? "" : influxDBEndpoint.Address);

            string path = this.GetTestFilePath(tCase.ID.ToString());
            strCode = strCode.Replace("{ResultStatVisualizer}", $"{path}{string.Format(_testResultFileName, string.Empty)}");

            // 替换CSV
            foreach (var item in configuration.DataSourceVars)
            {
                string fileName = Path.GetFileName(item.Path);

                using (var textStream = new MemoryStream(UTF8Encoding.UTF8.GetBytes(item.Data)))
                {
                    #region Test Code
#if DEBUG
                    //string testFilePath = $"E:\\Downloads\\{fileName}";

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

                    await tCase.MasterHost.SSHEndpoint.UploadFile(textStream, $"{path}{fileName}");
                    textStream.Close();
                }

                strCode = strCode.Replace(item.Path, $"{path}{fileName}");
            }

            #region 检查主机端口是否被占用，并强制终止
            //await this.Stop(tCase, cancellationToken);
            #endregion

            //代码模板必须有一个格式为{SlaveName}的替换符，该替换符标识每个Slave

            //获取测试用例的主测试机，上传测试代码
            using (var textStream = new MemoryStream(UTF8Encoding.UTF8.GetBytes(strCode)))
            {
                #region Test Code
#if DEBUG
                //string testFilePath = @"E:\Downloads\script.jmx";

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

                await tCase.MasterHost.SSHEndpoint.ExecuteCommand($"mkdir {path}");
                await tCase.MasterHost.SSHEndpoint.UploadFile(textStream, $"{path}{string.Format(_testFileName, string.Empty)}");
                textStream.Close();
            }

            ////获取测试用例的所有从属测试机，上传测试代码
            //var slaveHosts = tCase.GetAllSlaveHosts(cancellationToken);
            //int slaveCount = 0;
            //object lockObj = new object();
            //List<TestCaseSlaveHost> slaveHostList = new List<TestCaseSlaveHost>();
            //StringBuilder sbSlaveHosts = new StringBuilder();

            //await ParallelHelper.ForEach(slaveHosts, 10,
            //    async (item) =>
            //    {
            //        await item.Host.SSHEndpoint.ExecuteCommand($"mkdir {path}");

            //        //先删除文件夹内现有的所有文件
            //        await item.Host.SSHEndpoint.ExecuteCommand($"rm -rf {path}{string.Format(_testFileName, "_*")}");

            //        //为该Slave测试机下的每个Slave上传文件

            //        try
            //        {

            //            await item.Host.SSHEndpoint.UploadFile(
            //                async (service) =>
            //                {
            //                    var index = 0;
            //                    for (index = 0; index <= item.Count - 1; index++)
            //                    {
            //                        using (var textStream = new MemoryStream(UTF8Encoding.UTF8.GetBytes(strCode)))
            //                        {
            //                            await service.Upload(textStream, $"{path}{string.Format(_testFileName, $"_{index.ToString()}")}");
            //                            textStream.Close();
            //                        }
            //                    }
            //                }
            //              );

            //        }
            //        catch(UtilityException ex)
            //        {
            //            if (ex.Code==(int)CommandLineErrorCodes.SSHOperationTimeout)
            //            {
            //                var fragment = new TextFragment()
            //                {
            //                    Code = TestPlatformTextCodes.SlaveHostUploadTestFileTimeout,
            //                    DefaultFormatting = "从测试机{0}上传测试文件超时",
            //                    ReplaceParameters = new List<object>() { item.SlaveName }
            //                };
            //                throw new UtilityException((int)TestPlatformErrorCodes.SlaveHostUploadTestFileTimeout, fragment, 1, 0);
            //            }
            //            else
            //            {
            //                throw;
            //            }
            //        }

            //        lock (lockObj)
            //        {
            //            slaveCount += item.Count;
            //            slaveHostList.Add(item);
            //        }
            //    });

            var slaveHosts = tCase.GetAllSlaveHosts(cancellationToken);
            StringBuilder sbSlaveHosts = new StringBuilder();

            await foreach (var item in slaveHosts)
            {
                //await item.Host.SSHEndpoint.ExecuteCommand($"ps -ef | grep jmeter | grep {id} | grep -v grep | awk '{{print $2}}' | xargs kill -9", 10, cancellationToken);
                sbSlaveHosts.Append($",{item.Host.Address}:1099");
            }

            string s = $"jmeter -D remote_hosts={sbSlaveHosts.ToString().Substring(1)} -n -t {path}{string.Format(_testFileName, string.Empty)} -l {path}{string.Format(_testLogFileName, string.Empty)} -e -o {path} 2>&1 &";

            //执行主机测试命令
            List<Func<string?, Task<string>>> commands = new List<Func<string?, Task<string>>>()
            {
               async (preResult)=>
               {
                   return await Task.FromResult($"rm -rf {path}{string.Format(_testLogFileName,string.Empty)}");
               },
               async (preResult)=>
               {
                   return await Task.FromResult($"jmeter -D remote_hosts={sbSlaveHosts.ToString().Substring(1)} -n -t {path}{string.Format(_testFileName,string.Empty)} -l {path}{string.Format(_testLogFileName,string.Empty)} -e -o {path} 2>&1 &");
               }
            };
            await tCase.MasterHost.SSHEndpoint.ExecuteCommandBatch(commands);

            ////执行从属机测试命令
            //foreach(var item in slaveHostList)
            //{
            //    List<Func<string?, Task<string>>> slaveCommands = new List<Func<string?, Task<string>>>()
            //        {
            //            async (preResult)=>
            //            {
            //                return await Task.FromResult($"rm -rf {path}{string.Format(_testLogFileName, "_slave*")}");
            //            }
            //        };


            //    //await item.Host.SSHEndpoint.ExecuteCommand($"rm -rf {path}{string.Format(_testLogFileName, "_slave")}", cancellationToken);
            //    for (var index = 0;index <= item.Count - 1;index++)
            //    {
            //        var innerIndex = index;
            //        slaveCommands.Add(
            //            async (preResult) =>
            //            {
            //                //return await Task.FromResult($"locust -f {path}{string.Format(_testFileName, $"_{index.ToString()}")} --slave --master-host={tCase.MasterHost.Address} --no-web --run-time={  configuration.Duration.ToString()} --logfile={path}{string.Format(_testLogFileName, "_slave")} --clients={configuration.UserCount.ToString()} --hatch-rate={configuration.PerSecondUserCount.ToString()} &");
            //                //return await Task.FromResult($"locust -f {path}{string.Format(_testFileName, $"_{innerIndex.ToString()}")} --logfile {path}{string.Format(_testLogFileName, $"_slave")} --worker --headless --master-host {tCase.MasterHost.Address} --master-port 5557 > {path}{string.Format(_testOutFileName, $"_slave_")}{innerIndex.ToString()} 2>&1 &");
            //                //return await Task.FromResult($"locust -f {path}{string.Format(_testFileName, $"_{innerIndex.ToString()}")} --worker --headless --master-host {tCase.MasterHost.Address} --master-port 5557 > {path}{string.Format(_testLogFileName, $"_slave")} 2>&1 &");
            //                return await Task.FromResult($"locust -f {path}{string.Format(_testFileName, $"_{innerIndex.ToString()}")} --worker --headless --master-host {tCase.MasterHost.Address} --master-port {this.GetPort(configuration).ToString()} > {path}{string.Format(_testLogFileName, $"_slave_")}{innerIndex.ToString()} 2>&1 &");
            //                //return await Task.FromResult($"locust -f {path}{string.Format(_testFileName, $"_{innerIndex.ToString()}")} --logfile {path}{string.Format(_testLogFileName, $"_slave")} --worker --headless --master-host 127.0.0.1 --master-port {configuration.LocustMasterBindPort.ToString()} > {path}{string.Format(_testOutFileName, $"_slave_")}{innerIndex.ToString()} 2>&1 &");
            //                //return await Task.FromResult($"locust -f {path}{string.Format(_testFileName, $"_{innerIndex.ToString()}")} --worker --headless --master-host 127.0.0.1 --master-port {configuration.LocustMasterBindPort.ToString()} > {path}{string.Format(_testLogFileName, $"_slave")} 2>&1 &");
            //            }
            //       );                  
            //    }

            //    await item.Host.SSHEndpoint.ExecuteCommandBatch(slaveCommands);
            //}

        }

        public async Task Stop(TestCase tCase, CancellationToken cancellationToken = default)
        {
            string id = tCase.ID.ToString();

            //执行主机杀进程命令
            await tCase.MasterHost.SSHEndpoint.ExecuteCommand($"ps -ef | grep jmeter | grep {id} | grep -v grep | awk '{{print $2}}' | xargs kill -9", 10, cancellationToken);
            ////执行slave杀进程命令
            //var slaveHosts = tCase.GetAllSlaveHosts(cancellationToken);
            //await foreach (var item in slaveHosts)
            //{
            //    await item.Host.SSHEndpoint.ExecuteCommand($"ps -ef | grep jmeter | grep {id} | grep -v grep | awk '{{print $2}}' | xargs kill -9", 10, cancellationToken);
            //}
        }

        #region Private
        /// <summary>
        /// GetIPPort
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private List<Match> GetIPPort(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return null;
            }

            //string url = "http://172.0.0.1:8080/comm/logins/jwt";
            Regex re = new Regex(@"(((?:[-;:&=\+\$,\w]+@)?[A-Za-z0-9.-]+(:[0-9]+)?|(?:ww‌​w.|[-;:&=\+\$,\w]+@)[A-Za-z0-9.-]+)((?:\/[\+~%\/.\w-_]*)?\??(?:[-\+=&;%@.\w_]*)#?‌​(?:[\w]*))?)");

            MatchCollection mc = re.Matches(url);//获取的是一个数组

            return mc.ToList();
        }

        /// <summary>
        /// 获取Path
        /// </summary>
        /// <param name="configurationData"></param>
        /// <returns></returns>
        private string GetTestFilePath(string id)
        {
            string path = $"{_testFilePath}{id}/";

            return path;
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


    [DataContract]
    public class ConfigurationDataJmeter
    {
        /// <summary>
        /// 文本文件内容
        /// </summary>
        [DataMember]
        public string FileContent
        {
            get; set;
        } = null!;

        /// <summary>
        /// 数据源变量配置
        /// </summary>
        [DataMember]
        public List<ConfigurationDataJmeterForDataSourceVar> DataSourceVars
        {
            get; set;
        } = new List<ConfigurationDataJmeterForDataSourceVar>();
    }

    /// <summary>
    /// 数据源变量
    /// </summary>
    [DataContract]
    public class ConfigurationDataJmeterForDataSourceVar
    {
        /// <summary>
        /// 变量名称
        /// </summary>
        [DataMember]
        public string Name { get; set; } = null!;

        /// <summary>
        /// 数据
        /// 配置时不需要配置此属性
        /// </summary>
        [DataMember]
        public string Path { get; set; } = string.Empty;

        /// <summary>
        /// 变量类型
        /// 来源自数据源
        /// 配置时不需要配置此属性
        /// </summary>
        public string Type { get; set; } = null!;

        /// <summary>
        /// 数据源名称
        /// </summary>
        [DataMember]
        public string DataSourceName { get; set; } = null!;

        /// <summary>
        /// 数据
        /// 配置时不需要配置此属性
        /// </summary>
        public string Data { get; set; } = string.Empty;
    }

}
