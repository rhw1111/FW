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
using MSLibrary.StreamingDB.InfluxDB;
using MSLibrary.Collections;
using MSLibrary.Collections.DAL;

namespace FW.TestPlatform.Main.Entities.TestCaseHandleServices
{
    [Injection(InterfaceType = typeof(TestCaseHandleServiceForJmeter), Scope = InjectionScope.Singleton)]
    public class TestCaseHandleServiceForJmeter : ITestCaseHandleService
    {
        private const string _testFilePath = "/usr/testfile/";
        private const string _testFileName = "script{0}.jmx";
        private const string _testLogFileName = "log{0}.jtl";
        private const string _testResultFileName = "resultstatvisualizer{0}.csv";
        private const string _testOutFileName = "out{0}.log";
        private const int _maxLogSize = 1024 * 1024;

        private readonly ITestDataSourceRepository _testDataSourceRepository;
        private readonly IScriptTemplateRepository _scriptTemplateRepository;
        private readonly ISSHEndpointRepository _sshEndpointRepository;
        private readonly ISystemConfigurationService _systemConfigurationService;
        private readonly IInfluxDBEndpointRepository _influxDBEndpointRepository;
        private readonly ITestCaseStore _testCaseStore;
        private readonly ITreeEntityStore _treeEntityStore;

        /// <summary>
        /// 要使用的附加函数名称集合
        /// 系统初始化时注入
        /// </summary>
        public static IList<string> AdditionFuncNames { get; set; } = new List<string>();

        public TestCaseHandleServiceForJmeter(ITestDataSourceRepository testDataSourceRepository, IScriptTemplateRepository scriptTemplateRepository, ISSHEndpointRepository sshEndpointRepository, ISystemConfigurationService systemConfigurationService, IInfluxDBEndpointRepository influxDBEndpointRepository, ITestCaseStore testCaseStore, ITreeEntityStore treeEntityStore)
        {
            _testDataSourceRepository = testDataSourceRepository;
            _scriptTemplateRepository = scriptTemplateRepository;
            _sshEndpointRepository = sshEndpointRepository;
            _systemConfigurationService = systemConfigurationService;
            _influxDBEndpointRepository = influxDBEndpointRepository;
            _testCaseStore = testCaseStore;
            _treeEntityStore = treeEntityStore;
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

                        item.Type = dataSource.Type;
                        item.Data = dataSource.Data;
                    }
                );
            }

            //生成代码
            var strCode = await scriptTemplate.GenerateScript(contextDict, cancellationToken);
            string end = "    </hashTree>\n  </hashTree>\n</jmeterTestPlan>";
            strCode = configuration.FileContent.Replace(end, strCode + end);

            // 替换生成代码中的固定标签
            strCode = strCode.Replace("{CaseID}", tCase.ID.ToString());
            strCode = strCode.Replace("{CaseHistoryID}", tCase.TestCaseHistoryID.ToString());
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

            await tCase.MasterHost.SSHEndpoint.ExecuteCommand($"rm -r {path}");
            await tCase.MasterHost.SSHEndpoint.ExecuteCommand($"mkdir {path}");

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

                await tCase.MasterHost.SSHEndpoint.UploadFile(textStream, $"{path}{string.Format(_testFileName, string.Empty)}");
                textStream.Close();
            }

            var slaveHosts = tCase.GetAllSlaveHosts(cancellationToken);
            StringBuilder sbSlaveHosts = new StringBuilder();

            await foreach (var item in slaveHosts)
            {
                //await item.Host.SSHEndpoint.ExecuteCommand($"ps -ef | grep jmeter | grep {id} | grep -v grep | awk '{{print $2}}' | xargs kill -9", 10, cancellationToken);
                sbSlaveHosts.Append($",{item.Host.Address}:1099");
            }

            //执行主机测试命令
            List<Func<string?, Task<string>>> commands = new List<Func<string?, Task<string>>>()
            {
               async (preResult)=>
               {
                   return await Task.FromResult($"rm -rf {path}{string.Format(_testLogFileName,string.Empty)}");
               },
               async (preResult)=>
               {
                   return await Task.FromResult($"jmeter -R {sbSlaveHosts.ToString().Substring(1)} -n -t {path}{string.Format(_testFileName,string.Empty)} -l {path}{string.Format(_testLogFileName,string.Empty)} -e -o {path}out/ -j {path}jmeter.log > {path}{string.Format(_testOutFileName,string.Empty)} 2>&1 &");
               }
            };
            await tCase.MasterHost.SSHEndpoint.ExecuteCommandBatch(commands);
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
