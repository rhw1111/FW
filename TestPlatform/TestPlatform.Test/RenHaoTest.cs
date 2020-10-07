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
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
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
using MSLibrary.Thread;
using MSLibrary.Serializer;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Net;

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
        public async Task TestDI()
        {
            ContextContainer.SetValue<int>(ContextTypes.CurrentUserLcid, 22);
            var lcid = ContextContainer.GetValue<int>(ContextTypes.CurrentUserLcid);

            var t=Task.Run(() =>
            {
                lcid = ContextContainer.GetValue<int>(ContextTypes.CurrentUserLcid);
            }).ConfigureAwait(false);

            await t;
            //DIContainerContainer.Inject<ITA,TA>(InjectionScope.Singleton);
            //var obj=DIContainerContainer.Get<TA>(new object[] { "a" },new Type[] {typeof(string) });

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
            string ss = "sddd-12332";
            if (long.TryParse(ss,out long longSS))
            {
                var dd = longSS;
            }
            List<Task> waitTasks = new List<Task>();
            Task resultTask = new Task(async () =>
            {
                foreach (var item in waitTasks)
                {
                    await item;
                }
            });

            var t = Task.Run(async () =>
              {
                  while(true)
                  {
                      var cc = 1;
                      await Task.Delay(1000);
                  }
              });

            await t;
                 
           var a=JsonSerializerHelper.Serializer<JObject>(null);
            Dictionary<string, Item> dict = new Dictionary<string, Item>();

            for(var index=0;index<=10000000; index++)
            {
                dict.Add(index.ToString(), new Item { Value = index });
            }
            
            //var sum = dict.((item)=>(long)item.Value.Value);
        }


        [Test]
        public async Task TestRunMultiple()
        {
            List<string> caseIds = new List<string>() {
            "f0a6cd8e-44a0-47a4-ad53-2f3c83f9d728","b37c153d-413a-4d32-81a8-9179a302dc3a","98a44749-31c0-4389-97a9-0c82fb2936bd",
                "2ba37547-a5f4-44a6-af8d-352728f81887","4a473d64-223b-4760-aed4-fdf0ae92cebc","f5fdb4a3-5c68-4752-8939-5f425250e77e",
                "55e8853b-ce47-43d4-83db-d20b78987049","4cfd66bd-e397-4a26-835e-2b4f6992c413","ba9ff428-6c1c-42bb-95c7-5869982edeec",
                "f0ee1d7e-a0fe-456d-9647-f1da17a89150","69281de2-037c-46eb-a441-fdd008383768","2e76178b-41fe-4431-8d98-0ea28676d3e9",
                "a64c773b-3a45-42ce-99b4-d3f2e6bd4dd6"};
            List<HttpResponseMessage> responses = new List<HttpResponseMessage>();

  

            await ParallelHelper.ForEach(caseIds, 10,
                async (caseId) =>
                {
                    var httpclientHandler = new HttpClientHandler();
                    httpclientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, error) => true;

                    using (HttpClient httpClient = new HttpClient(httpclientHandler))
                    {
                        //httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        //StringContent strcontent = new StringContent(JsonConvert.SerializeObject("aa"), Encoding.UTF8, "application/json");
                        var message = new HttpRequestMessage(HttpMethod.Post, "https://52.188.14.158:8081/api/testcase/run/?caseId=" + caseId);
                        //设置contetn
                        //message.Content = strcontent;
                        //发送请求
                        var httpResponseHeaders =await httpClient.SendAsync(message);
                        responses.Add(httpResponseHeaders);
                        if (httpResponseHeaders.IsSuccessStatusCode)
                        {
                            var content = httpResponseHeaders.Content;
                        }
                    }
                }
               );
        }



        [Test]
        public async Task TestCap()
        {
            using (var reader = Haukcode.PcapngUtils.IReaderFactory.GetReader(@"D:\1.cap"))
            {
                reader.OnReadPacketEvent += (c, p) =>
                {
                    var pp = p;
                };
                reader.OnExceptionEvent += (o, e) =>
                  {
                      var ex = e;
                  };
            }


            List<string> names = new List<string>();
            names.Add("");
            await ParallelHelper.ForEach(names, 10,

                async(name)=>
                {
                    using (var reader = Haukcode.PcapngUtils.IReaderFactory.GetReader(@"D:\1.cap"))
                    {
                        reader.OnReadPacketEvent += (c, p) =>
                          {
                              var pp = p;
                          };
                    }

                    var dda = 1;
                }

                );


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

    public interface ITA
    {
        string GetValue();
    }

    public class TA : ITA
    {
        private string _v;
        public TA(string v)
        {
            _v = v;
        }

        public string GetValue()
        {
            return _v;
        }
    }
}