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
using MSLibrary.Serializer;
using MSLibrary.LanguageTranslate;
using MSLibrary.CommandLine.SSH;
using FW.TestPlatform.Main.Template.LabelParameterHandlers;
using MongoDB.Libmongocrypt;
using Castle.Core.Internal;

namespace FW.TestPlatform.Main.Entities.TestCaseHandleServices
{
    [Injection(InterfaceType = typeof(TestCaseHandleServiceForTcp), Scope = InjectionScope.Singleton)]
    public class TestCaseHandleServiceForTcp : ITestCaseHandleService
    {
        private const string _testFilePath = "/usr/testfile/";
        private const string _testFileName = "script.py";
        private const string _testLogFileName = "log";
        private const int _maxLogSize = 1024 * 1024;

        private readonly ITestDataSourceRepository _testDataSourceRepository;
        private readonly IScriptTemplateRepository _scriptTemplateRepository;
        private readonly ISSHEndpointRepository _sshEndpointRepository;

        public TestCaseHandleServiceForTcp(ITestDataSourceRepository testDataSourceRepository, IScriptTemplateRepository scriptTemplateRepository, ISSHEndpointRepository sshEndpointRepository)
        {
            _testDataSourceRepository = testDataSourceRepository;
            _scriptTemplateRepository = scriptTemplateRepository;
            _sshEndpointRepository = sshEndpointRepository;
        }

        public async Task<string> GetMasterLog(TestHost host, CancellationToken cancellationToken = default)
        {
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
                $"{_testFilePath}.{_testLogFileName}",
                cancellationToken
                );
            return result;
        }

        public async Task<string> GetSlaveLog(TestHost host, CancellationToken cancellationToken = default)
        {
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
                $"{_testFilePath}.{_testLogFileName}",
                cancellationToken
                );
            return result;
        }

        public async Task<bool> IsEngineRun(TestCase tCase, CancellationToken cancellationToken = default)
        {
            //执行主机查进程命令
            var result=await tCase.MasterHost.SSHEndpoint.ExecuteCommand($"ps -ef |grep locust|grep -v grep | awk '{{print $2}}'", cancellationToken);
            if (result.IsNullOrEmpty())
            {
                return false;
            }

            return true;
        }

        public async Task Run(TestCase tCase, CancellationToken cancellationToken = default)
        {
            var configuration = JsonSerializerHelper.Deserialize<ConfigurationData>(tCase.Configuration);
            if (configuration.DataSourceNames==null)
            {
                configuration.DataSourceNames = new List<string>();
            }

            var scriptTemplate=await _scriptTemplateRepository.QueryByName(ScriptTemplateNames.LocustTcp, cancellationToken);
            
            if (scriptTemplate==null)
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.NotFoundScriptTemplateByName,
                    DefaultFormatting = "找不到名称为{0}的脚本模板",
                    ReplaceParameters = new List<object>() { ScriptTemplateNames.LocustTcp }
                };

                throw new UtilityException((int)TestPlatformErrorCodes.NotFoundScriptTemplateByName, fragment, 1, 0);
            }

            var contextDict= new Dictionary<string, object>();

            //将引擎类型加入到模板上下文中
            contextDict.Add(TemplateContextParameterNames.EngineType, tCase.EngineType);
            //将请求体模板加入到模板上下文中
            contextDict.Add(TemplateContextParameterNames.RequestBody, configuration.RequestBody);
            //将响应分隔符加入到模板上下文中
            contextDict.Add(TemplateContextParameterNames.ResponseSeparator, configuration.ResponseSeparator);
            //将预热时间加入到模板上下文中
            contextDict.Add(TemplateContextParameterNames.ReadyTime, configuration.ReadyTime);
            //将测试地址加入到模板上下文中
            contextDict.Add(TemplateContextParameterNames.Address, configuration.Address);


            //生成数据源函数,加入到模板上下文中
            var dataSources =await _testDataSourceRepository.QueryByNames(configuration.DataSourceNames, cancellationToken);

            Dictionary<string,DataSourceFuncConfigurationItem> funcItems = new Dictionary<string, DataSourceFuncConfigurationItem>();
            foreach (var item in dataSources)
            {
                funcItems[item.Name] =
                    new DataSourceFuncConfigurationItem()
                    {
                        FuncName = item.Name,
                        FuncUniqueName = $"{item.Name}_{Guid.NewGuid().ToString("N")}",
                        FuncType = item.Type,
                        Data = item.Data
                    };                   
            }

            contextDict.Add(TemplateContextParameterNames.DataSourceFuncs, funcItems);

            //生成代码
            var strCode=await scriptTemplate.GenerateScript(contextDict, cancellationToken);

            //获取测试用例的主测试机，上传测试代码
            using (var textStream=new MemoryStream(UTF8Encoding.UTF8.GetBytes(strCode)))
            {
                await tCase.MasterHost.SSHEndpoint.UploadFile(textStream, $"{_testFilePath}{_testFileName}", cancellationToken);
                textStream.Close();
            }

            //获取测试用例的所有从属测试机，上传测试代码
            var slaveHosts=tCase.GetAllSlaveHosts(cancellationToken);
            int slaveCount = 0;
            List<TestCaseSlaveHost> slaveHostList = new List<TestCaseSlaveHost>();
            await foreach(var item in slaveHosts)
            {
                slaveCount += item.Count;
                using (var textStream = new MemoryStream(UTF8Encoding.UTF8.GetBytes(strCode)))
                {
                    await item.Host.SSHEndpoint.UploadFile(textStream, $"{_testFilePath}{_testFileName}", cancellationToken);
                    textStream.Close();
                }

                slaveHostList.Add(item);
            }

            //执行主机测试命令
            List<Func<string?, Task<string>>> commands = new List<Func<string?, Task<string>>>()
            {
               async (preResult)=>
               {
                   return await Task.FromResult($"rm -rf {_testFilePath}{_testLogFileName}");
               },
               async (preResult)=>
               {
                   return await Task.FromResult($"locust -f {_testFilePath}{_testFileName} --master --expect-slaves={slaveCount.ToString()} --no-web --run-time={  configuration.Duration.ToString()} --logfile={_testFilePath}{_testLogFileName} --clients={configuration.UserCount.ToString()} --hatch-rate={configuration.PerSecondUserCount.ToString()} &");
               }
            };
            await tCase.MasterHost.SSHEndpoint.ExecuteCommandBatch(commands, cancellationToken);

            //执行从属机测试命令
            foreach(var item in slaveHostList)
            {
                for(var index=0;index<= item.Count-1;index++)
                {
                    List<Func<string?, Task<string>>> slaveCommands = new List<Func<string?, Task<string>>>()
                    {
                        async (preResult)=>
                        {
                            return await Task.FromResult($"rm -rf {_testFilePath}{_testLogFileName}");
                        },
                        async (preResult)=>
                        {
                            return await Task.FromResult($"locust -f {_testFilePath}{_testFileName} --slave --master-host={tCase.MasterHost.Address} --no-web --run-time={  configuration.Duration.ToString()} --logfile={_testFilePath}{_testLogFileName} --clients={configuration.UserCount.ToString()} --hatch-rate={configuration.PerSecondUserCount.ToString()} &");
                        }
                    };


                    await item.Host.SSHEndpoint.ExecuteCommandBatch(slaveCommands, cancellationToken);
                }
            }

        }

        public async Task Stop(TestCase tCase, CancellationToken cancellationToken = default)
        {
            //执行主机杀进程命令
            await tCase.MasterHost.SSHEndpoint.ExecuteCommand($"ps -ef |grep locust|grep -v grep | awk '{{print $2}}' | xargs kill -9", cancellationToken);
        }


        [DataContract]
        private class ConfigurationData
        {
            /// <summary>
            /// 用户数量
            /// </summary>
            [DataMember]
            public int UserCount
            {
                get; set;
            }
            /// <summary>
            /// 每秒增加用户数量
            /// </summary>
            [DataMember]
            public int PerSecondUserCount
            {
                get; set;
            }

            /// <summary>
            /// 持续时间（秒）
            /// </summary>
            [DataMember]
            public int Duration
            {
                get; set;
            }
            /// <summary>
            /// 预热时间（秒）
            /// </summary>
            [DataMember]
            public int ReadyTime
            {
                get; set;
            }

            /// <summary>
            /// 测试接口地址
            /// </summary>
            [DataMember]
            public string Address
            {
                get; set;
            } = null!;
            
            /// <summary>
            /// 请求体内容
            /// </summary>
            [DataMember]
            public string RequestBody
            {
                get; set;
            } = null!;

            /// <summary>
            /// 要用到的数据源名称列表
            /// </summary>
            [DataMember]
            public List<string> DataSourceNames
            {
                get; set;
            } = null!;

            /// <summary>
            /// 响应内容分隔符
            /// </summary>
            [DataMember]
            public string ResponseSeparator
            {
                get; set;
            } = null!;
        }
    }
}
