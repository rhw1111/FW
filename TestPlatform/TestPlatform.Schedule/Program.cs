using System;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MSLibrary.Configuration;
using Microsoft.Extensions.Logging;
using MSLibrary.DI;
using FW.TestPlatform.Main;
using FW.TestPlatform.Main.Configuration;

namespace FW.TestPlatform.Schedule
{
    class Program
    {
        private static string _baseUrl;
        public static async Task Main(string[] args)
        {
            //允许客户端使用非安全的http2
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
            //设置编码，解决中文问题
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            //设置应用程序工作基目录
            _baseUrl = Path.GetDirectoryName(typeof(Program).Assembly.Location);
            Environment.CurrentDirectory = _baseUrl ?? Environment.CurrentDirectory;

            //获取运行环境名称
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_APPLICATIONNAME");
            if (environmentName == null)
            {
                environmentName = string.Empty;
            }
            //初始化配置容器
            MainStartupHelper.InitConfigurationContainer(environmentName, _baseUrl);
            ScheduledStartupHelper.InitConfigurationContainer(environmentName, _baseUrl);

            //获取核心配置
            var coreConfiguration = ConfigurationContainer.Get<CoreConfiguration>(ConfigurationNames.Application);

            //设置线程池
            System.Threading.ThreadPool.GetMaxThreads(out int workerCounts, out int ioCounts);
            System.Threading.ThreadPool.SetMaxThreads(workerCounts * coreConfiguration.ThreadPoolSetting.MaxWorkThread, ioCounts * coreConfiguration.ThreadPoolSetting.MaxIOThread);

            System.Threading.ThreadPool.GetMinThreads(out workerCounts, out ioCounts);
            System.Threading.ThreadPool.SetMinThreads(workerCounts * coreConfiguration.ThreadPoolSetting.MinWorkThread, ioCounts * coreConfiguration.ThreadPoolSetting.MinIOThread);


            //初始化上下文容器
            MainStartupHelper.InitContext();
            ScheduledStartupHelper.InitContext();

            await CreateHostBuilder(args).Build().RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureHostConfiguration(configHost =>
            {

            })
            .ConfigureAppConfiguration((buildContext, builder) =>
            {

            })

            .ConfigureServices((hostContext, services) =>
            {
                //获取核心配置
                var applicationConfiguration = ConfigurationContainer.Get<ApplicationConfiguration>(ConfigurationNames.Application);
                //初始化DI容器
                MainStartupHelper.InitDI(services, applicationConfiguration.DISetting);

                MainStartupHelper.InitStaticInfo();
                ScheduledStartupHelper.InitStaticInfo();
                //services.AddApplicationInsightsTelemetry(options =>
                //{
                //    options.InstrumentationKey = "123";
                //    options.ApplicationVersion ="1231";
                //});
                services.AddHostedService<ScheduledService>();
            })
            .ConfigureLogging((bulider) =>
            {
                //配置日志工厂
                var loggerFactory = LoggerFactory.Create((bulider) =>
                {
                    MainStartupHelper.InitLogger(bulider);
                });
                DIContainerContainer.Inject<ILoggerFactory>(loggerFactory);
            });
    }
}
