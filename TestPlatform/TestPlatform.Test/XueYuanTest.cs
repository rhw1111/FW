﻿using NUnit.Framework;
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
                Type = "Json",
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
                ID = new Guid("cae64c27-8e87-4a38-b94a-32a47a7eea63"),
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
                TestCaseID = new Guid("cae64c27-8e87-4a38-b94a-32a47a7eea63"),
                SlaveName = "slave1",
                Count = 100,
                ExtensionInfo = ""
            };

            await testCaseSlaveHost.Add();

            Assert.Pass();
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

        [Test]
        public async Task TestTestCaseRun()
        {
            TestCase testCase = new TestCase()
            {
                ID = new Guid("cae64c27-8e87-4a38-b94a-32a47a7eea63"),
                MasterHostID = new Guid("822114cf-5277-4667-961f-e231f9e67e4d"),
                OwnerID = new Guid("46f8bcca-af6e-11ea-8e6a-0242ac110002"),
                EngineType = EngineTypes.Tcp,
                Name = "Case1",
                Configuration = "",
                Status = TestCaseStatus.NoRun
            };

            var testCaseStore = DIContainerContainer.Get<ITestCaseStore>();

            var testCaseRunner = testCaseStore.QueryByID(testCase.ID);

            if (testCaseRunner == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.NotFoundTestCaseByID,
                    DefaultFormatting = "找不到Id为{0}的测试案例",
                    ReplaceParameters = new List<object>() { testCase.ID.ToString() }
                };

                throw new UtilityException((int)TestPlatformErrorCodes.NotFoundTestCaseByID, fragment, 1, 0);
            }

            await testCase.Run();

            Assert.Pass();
        }
    }
}
