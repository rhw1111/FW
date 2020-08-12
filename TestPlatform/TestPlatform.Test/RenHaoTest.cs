using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
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
using MSLibrary.Survey.SurveyMonkey;
using MSLibrary.Survey;
using MSLibrary.Survey.SurveyMonkey.Message;
using MSLibrary.Survey.SurveyMonkey.HttpAuthHandleServices;
using MSLibrary.Survey.SurveyMonkey.RequestHandleServices;
using MSLibrary.NetCap;
using MSLibrary.Schedule.DAL;
using MSLibrary.Template;
using MSLibrary.MySqlStore.Schedule.DAL;
using Ctrade.Message;

namespace TestPlatform.Test
{
    public class RenHaoTest
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

            SurveyMonkeyEndpointIMP.SurveyMonkeyHttpAuthHandleServiceFactories[SurveyMonkeyTypes.OAuth] = DIContainerContainer.Get<SurveyMonkeyHttpAuthHandleServiceForOAuthFactory>();
            
            SurveyMonkeyEndpointIMP.SurveyMonkeyRequestHandleServiceFactories[SurveyMonkeyRequestTypes.SurveyResponseQuery]= DIContainerContainer.Get<RequestHandleServiceForSurveyResponseQueryFactory>();
            SurveyMonkeyEndpointIMP.SurveyMonkeyRequestHandleServiceFactories[SurveyMonkeyRequestTypes.SurveyResponseQuerySingle] = DIContainerContainer.Get<RequestHandleServiceForSurveyResponseQuerySingleFactory>();
            SurveyMonkeyEndpointIMP.SurveyMonkeyRequestHandleServiceFactories[SurveyMonkeyRequestTypes.WebhookCallback] = DIContainerContainer.Get<RequestHandleServiceForWebhookCallbackFactory>();
            SurveyMonkeyEndpointIMP.SurveyMonkeyRequestHandleServiceFactories[SurveyMonkeyRequestTypes.WebhookDelete] = DIContainerContainer.Get<RequestHandleServiceForWebhookDeleteFactory>();
            SurveyMonkeyEndpointIMP.SurveyMonkeyRequestHandleServiceFactories[SurveyMonkeyRequestTypes.WebhookQuery] = DIContainerContainer.Get<RequestHandleServiceForWebhookQueryFactory>();
            SurveyMonkeyEndpointIMP.SurveyMonkeyRequestHandleServiceFactories[SurveyMonkeyRequestTypes.WebhookQuerySingle] = DIContainerContainer.Get<RequestHandleServiceForWebhookQuerySingleFactory>();
            SurveyMonkeyEndpointIMP.SurveyMonkeyRequestHandleServiceFactories[SurveyMonkeyRequestTypes.WebhookRegister] = DIContainerContainer.Get<RequestHandleServiceForWebhookRegisterFactory>();


            //配置日志工厂
            var loggerFactory = LoggerFactory.Create((builder) =>
            {
                MainStartupHelper.InitLogger(builder);
            });

            DIContainerContainer.Inject<ILoggerFactory>(loggerFactory);
        }

        [Test]
        public async Task Test1()
        {

            var testDataSourceStore = DIContainerContainer.Get<ITestDataSourceStore>();

            TestDataSource dataSource = new TestDataSource()
            {
                 ID=Guid.NewGuid(),
                  Name="1",
                   Type="Json",
                    Data="{}"                    
            };

            await testDataSourceStore.QueryByNameNoLock("1");

            //IAsyncEnumerable<object> asyncEnumer;


            //var bytes=Convert.FromBase64String("XExBYS06nWWTysT9lzOTnw==");

            //var str= UTF8Encoding.UTF32.GetString(bytes);
            //var str = Convert.ToBase64String(bytes);
            //bytes = Convert.FromBase64String(str);
            //str = UTF8Encoding.UTF8.GetString(bytes);

            await using (var scope = new DBTransactionScope(System.Transactions.TransactionScopeOption.Required, new System.Transactions.TransactionOptions() { }))
            {

                scope.Complete();
            }

            var aa=DBTransactionScope.InScope();

                Assert.Pass();
        }

        [Test]
        public async Task TestForSurveyMonkey()
        {

            DateTime dateTimeUTC = DateTime.UtcNow;
            DateTime dateTime = DateTime.Now;

            string strDateTime = dateTime.ToString("yyyy-MM-ddTHH:mm:sszzz");
            strDateTime = dateTimeUTC.ToString("yyyy-MM-ddTHH:mm:sszzz");

            Regex regex = new Regex("/dd/ss", RegexOptions.IgnoreCase);
            var regResult=regex.IsMatch("http://ssdd/dd/ss/ff?dssd");

            SurveyMonkeyEndpoint endpoint = new SurveyMonkeyEndpoint()
            {
                ID = Guid.NewGuid(),
                Name = "0uozfpinTjyfYWhb0eR7EA",
                Address = "https://api.surveymonkey.com",
                Type = SurveyMonkeyTypes.OAuth,
                Vesion = "v3",
                Configuration = @"{
                                        ""ClientID"":""0uozfpinTjyfYWhb0eR7EA"",
                                        ""ClientSecret"":""279529081356860839199098816144866363827"",
                                        ""AccessToken"":""9PuO701uc8gOEoQmmC8kHKYtzSd6cxw5LliPuTg46D0DNUUOkDLYXcvyerwYw2tLb2KtAm0xocHoeF7r6W2l-3TG1TfyyD-DkBl5QR6RFFJDORisHD7V7MLDPef69p2j""
                                  }",
                CreateTime = DateTime.UtcNow,
                ModifyTime = DateTime.UtcNow
            };

            /*WebhookQueryRequest request = new WebhookQueryRequest()
            {
                Page = 1,
                PageSize = 10
            };*/

            /*WebhookRegisterRequest request = new WebhookRegisterRequest()
            {
                EventType = "response_completed",
                Name = "ResponseCompleted",
                ObjectType = "survey",
                ObjectIds = new List<string>() { "287291102" },
                SubscriptionUrl = "http://52.188.14.158:8081/weatherforecast"
               
            };*/

            WebhookCallbackRequest request = new WebhookCallbackRequest()
            {
                SmApikey = "0uozfpinTjyfYWhb0eR7EA",
                SmSignature = "+/tt6Tjgvn7WPf3J68A82srprJA=",
                Body = @"{""name"":""ResponseCompleted"",""filter_type"":""survey"",""filter_id"":""287291102"",""event_type"":""response_completed"",""event_id"":""10030925191"",""event_datetime"":""2020-07-05T02:32:49.504424+00:00"",""object_type"":""response"",""object_id"":""11759142137"",""resources"":{""respondent_id"":""11759142137"",""recipient_id"":""0"",""survey_id"":""287291102"",""user_id"":""154606068"",""collector_id"":""263263092""}}"
            };

            var response=await endpoint.Execute(request);
        }

        private async Task Do1()
        {
            _connections.Value.Add("1", "1");
            await Task.FromResult(0);
        }

        private async Task Do2()
        {
           var dict= _connections.Value;
            await Task.FromResult(0);
        }

        [Test]
        public async Task TestDict()
        {
            Dictionary<string, Item> dict = new Dictionary<string, Item>();

            for(var index=0;index<=10000000; index++)
            {
                dict.Add(index.ToString(), new Item { Value = index });
            }
            
            //var sum = dict.((item)=>(long)item.Value.Value);
        }

        [Test]
        public async Task TestCap()
        {

            Task tt = new Task(async()=>
            {
                var aa = 1;
            });
            tt.Start();
            await tt;
            await using (var stream=File.OpenRead(@"D:\1.cap"))
            {
                PacketCaptureReader reader = new PacketCaptureReader(stream);
                while (true)
                {
                    var capture = reader.Read();

                    try
                    {

                        var data = APIOrderCancelReplyMsg.Parser.ParseFrom(capture.Packet.Array);
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
        }

        [Test]
        public async Task TestSchedule()
        {
            var n=typeof(FW.TestPlatform.Main.Schedule.Actions.ScheduleActionServiceForNetGatewayFactory).AssemblyQualifiedName;
            var id=Guid.NewGuid();
            var testDataSourceStore = DIContainerContainer.Get<IScheduleActionGroupStore>();
            var result=await testDataSourceStore.QueryByPage("", 1, 1);
        }


        private class Item
        {
            public int Value { get; set; }
        }
    }
}