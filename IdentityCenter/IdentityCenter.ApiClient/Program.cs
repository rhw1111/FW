using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Globalization;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using MSLibrary;
using MSLibrary.Configuration;
using MSLibrary.DI;
using MSLibrary.Grpc;
using IdentityCenter.Main;
using IdentityCenter.Main.Configuration;
using IdentityCenter.Api;
using IdentityServer4.Models;

namespace IdentityCenter.ApiClient
{
    class Program
    {
        static async Task Main(string[] args)
        {

            //AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2Support", true);
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            //AppContext.SetSwitch("System.Net.Http.UseSocketsHttpHandler", false);
            //AppContext.SetSwitch("System.Net.Http.UseSocketsHttpHandler", false);
            HttpClient client1 = new HttpClient();
            //client1.DefaultRequestVersion = new Version(2, 0);
            var req = new HttpRequestMessage(HttpMethod.Get, "https://http2.pro/api/v1")
            {
                Version = new Version(2, 0)
            };
            var x = await client1.SendAsync(req);

            //client1.DefaultRequestVersion = new Version(2, 0);
            //var response=await client1.GetAsync("https://www.google.com");

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
            StartupHelper.InitConfigurationContainer(environmentName, baseUrl);

            //获取核心配置
            var coreConfiguration = ConfigurationContainer.Get<ApplicationConfiguration>(ConfigurationNames.Application);

            //初始化上下文容器
            StartupHelper.InitContext();

            ServiceCollection services = new ServiceCollection();
            //初始化DI容器
            StartupHelper.InitDI(services, coreConfiguration.DISetting);


            //初始化静态设置
            StartupHelper.InitStaticInfo();


            //配置日志工厂
            var loggerFactory = LoggerFactory.Create((builder) =>
            {
                StartupHelper.InitLogger(builder);
            });

            DIContainerContainer.Inject<ILoggerFactory>(loggerFactory);


            ChannelPoolRepository.ChannelPools.Add("A", new ChannelPool()
            {
                ID = Guid.Parse("23B779BA-D4D9-4132-A951-B471527A3B71"),
                Name = "A",
                Address = "http://localhost:5035",
                Configuration = "",
                InterceptNames = new string[] { GrpcClinetInterceptorDescriptionNames.ClinetExceptionWrapper, GrpcClinetInterceptorDescriptionNames.ClientRequestTrace },
                CreateTime = DateTime.UtcNow,
                ModifyTime = DateTime.UtcNow
            });


            var channelPoolRepository = DIContainerContainer.Get<IChannelPoolRepository>();
            var channelPool = await channelPoolRepository.QueryByName("A");
            var invoker = await channelPool.GetInvoker();
            Greeter.GreeterClient client = new Greeter.GreeterClient(invoker);
            await client.SayHelloAsync(new HelloRequest() { Name = "AA" });
            Console.WriteLine("Hello World!");
        }
    }
}
