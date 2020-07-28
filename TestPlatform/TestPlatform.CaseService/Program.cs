using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MSLibrary;
using MSLibrary.Configuration;
using MSLibrary.DI;
using FW.TestPlatform.Main;
using FW.TestPlatform.Main.Configuration;

namespace FW.TestPlatform.CaseService
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
            //设置编码，解决中文问题
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            //设置应用程序工作基目录
            var baseUrl = Path.GetDirectoryName(typeof(Program).Assembly.Location);
            Environment.CurrentDirectory = baseUrl ?? Environment.CurrentDirectory;

            //获取运行环境名称
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_APPLICATIONNAME");
            if (environmentName == null)
            {
                environmentName = string.Empty;
            }
            //初始化配置容器
            MainStartupHelper.InitConfigurationContainer(environmentName, baseUrl);
            WebApiStartupHelper.InitConfigurationContainer(environmentName, baseUrl);

            //获取核心配置
            var coreConfiguration = ConfigurationContainer.Get<ApplicationConfiguration>(ConfigurationNames.Application);

            //设置线程池
            System.Threading.ThreadPool.GetMaxThreads(out int workerCounts, out int ioCounts);
            System.Threading.ThreadPool.SetMaxThreads(workerCounts * coreConfiguration.ThreadPoolSetting.MaxWorkThread, ioCounts * coreConfiguration.ThreadPoolSetting.MaxIOThread);

            System.Threading.ThreadPool.GetMinThreads(out workerCounts, out ioCounts);
            System.Threading.ThreadPool.SetMinThreads(workerCounts * coreConfiguration.ThreadPoolSetting.MinWorkThread, ioCounts * coreConfiguration.ThreadPoolSetting.MinIOThread);


            //初始化上下文容器
            MainStartupHelper.InitContext();
            WebApiStartupHelper.InitContext();

            await CreateHostBuilder(args).Build().RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureServices((context, services) =>
                    {

                        //获取核心配置
                        var coreConfiguration = ConfigurationContainer.Get<CoreConfiguration>(ConfigurationNames.Application);

                        //初始化DI容器
                        MainStartupHelper.InitDI(services, coreConfiguration.DISetting);
                        WebApiStartupHelper.InitDI(services, coreConfiguration.DISetting);

                        //初始化静态设置
                        MainStartupHelper.InitStaticInfo();
                        WebApiStartupHelper.InitStaticInfo();

                        //配置日志工厂
                        var loggerFactory = LoggerFactory.Create((builder) =>
                        {
                            MainStartupHelper.InitLogger(builder);
                        });

                        DIContainerContainer.Inject<ILoggerFactory>(loggerFactory);
                    })
                    .UseDefaultServiceProvider((options) =>
                    {
                        options.ValidateScopes = false;
                        options.ValidateOnBuild = false;
                    })
                    .ConfigureLogging((builder) =>
                    {

                    })
                    .ConfigureKestrel((options) =>
                    {

                    })
                    .UseConfiguration(ConfigurationContainer.GetConfiguration(ConfigurationNames.Host))
                    .UseStartup<Startup>();
                });
    }
}
