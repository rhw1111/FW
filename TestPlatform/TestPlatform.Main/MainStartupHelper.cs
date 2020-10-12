using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using System.IO;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using MSLibrary;
using MSLibrary.AspNet.Filter;
using MSLibrary.AspNet.Middleware;
using MSLibrary.Oauth.ADFS;
using MSLibrary.Configuration;
using MSLibrary.DI;
using MSLibrary.Context;
using MSLibrary.Context.Filter;
using MSLibrary.Context.HttpClaimGeneratorServices;
using MSLibrary.Context.ClaimContextGeneratorServices;
using MSLibrary.Transaction;
using MSLibrary.Template;
using MSLibrary.Collections;
using MSLibrary.Collections.TreeEntityValueServices;
using MSLibrary.Xrm;
using MSLibrary.Xrm.Token;
using MSLibrary.Xrm.Message;
using MSLibrary.Xrm.Message.AssociateCollection;
using MSLibrary.Xrm.Message.AssociateCollectionMultiple;
using MSLibrary.Xrm.Message.AssociateLookup;
using MSLibrary.Xrm.Message.Batch;
using MSLibrary.Xrm.Message.BoundAction;
using MSLibrary.Xrm.Message.BoundFunction;
using MSLibrary.Xrm.Message.Create;
using MSLibrary.Xrm.Message.CreateRetrieve;
using MSLibrary.Xrm.Message.Delete;
using MSLibrary.Xrm.Message.DisAssociateCollection;
using MSLibrary.Xrm.Message.DisAssociateLookup;
using MSLibrary.Xrm.Message.FileAttributeDeleteData;
using MSLibrary.Xrm.Message.FileAttributeDownloadChunking;
using MSLibrary.Xrm.Message.FileAttributeUploadChunking;
using MSLibrary.Xrm.Message.GetFileAttributeUploadInfo;
using MSLibrary.Xrm.Message.Retrieve;
using MSLibrary.Xrm.Message.RetrieveAggregation;
using MSLibrary.Xrm.Message.RetrieveCollectionAttribute;
using MSLibrary.Xrm.Message.RetrieveCollectionAttributeAggregation;
using MSLibrary.Xrm.Message.RetrieveCollectionAttributeReference;
using MSLibrary.Xrm.Message.RetrieveCollectionAttributeSavedQuery;
using MSLibrary.Xrm.Message.RetrieveCollectionAttributeUserQuery;
using MSLibrary.Xrm.Message.RetrieveEntityAttributeMetadata;
using MSLibrary.Xrm.Message.RetrieveEntityAttributeMetadataMultiple;
using MSLibrary.Xrm.Message.RetrieveEntityMetadata;
using MSLibrary.Xrm.Message.RetrieveEntityMetadataMultiple;
using MSLibrary.Xrm.Message.RetrieveEntityN2NRelationMetadataMultiple;
using MSLibrary.Xrm.Message.RetrieveEntityO2NRelationMetadataMultiple;
using MSLibrary.Xrm.Message.RetrieveGlobalOptionSetMetadata;
using MSLibrary.Xrm.Message.RetrieveLookupAttribute;
using MSLibrary.Xrm.Message.RetrieveLookupAttributeReference;
using MSLibrary.Xrm.Message.RetrieveMultiple;
using MSLibrary.Xrm.Message.RetrieveMultipleFetch;
using MSLibrary.Xrm.Message.RetrieveMultiplePage;
using MSLibrary.Xrm.Message.RetrieveMultipleSavedQuery;
using MSLibrary.Xrm.Message.RetrieveMultipleUserQuery;
using MSLibrary.Xrm.Message.RetrieveN2NRelationMetadata;
using MSLibrary.Xrm.Message.RetrieveN2NRelationMetadataMultiple;
using MSLibrary.Xrm.Message.RetrieveO2NRelationMetadata;
using MSLibrary.Xrm.Message.RetrieveO2NRelationMetadataMultiple;
using MSLibrary.Xrm.Message.RetrieveRelationMetadata;
using MSLibrary.Xrm.Message.RetrieveRelationMetadataMultiple;
using MSLibrary.Xrm.Message.RetrieveSignleAttribute;
using MSLibrary.Xrm.Message.UnBoundAction;
using MSLibrary.Xrm.Message.UnBoundFunction;
using MSLibrary.Xrm.Message.Update;
using MSLibrary.Xrm.Message.UpdateRetrieve;
using MSLibrary.Xrm.Message.Upsert;
using MSLibrary.Xrm.Message.UpsertRetrieve;
using MSLibrary.Xrm.MessageHandle;
using MSLibrary.Xrm.MessageHandle.CrmAttributeMetadataHandle;
using MSLibrary.Xrm.Convert;
using MSLibrary.Xrm.Convert.CrmRetrieveJTokenHandle;
using MSLibrary.Xrm.CrmServiceFactoryServices;
using MSLibrary.Logger;
using MSLibrary.Logger.LoggingBuilderProviderHandlers;
using MSLibrary.Xrm.Convert.CrmFunctionParameterHandle;
using MSLibrary.Xrm.Convert.CrmExecuteEntityTypeHandle;
using MSLibrary.Xrm.Convert.CrmAlternateKeyTypeHandle;
using MSLibrary.Xrm.Convert.CrmActionParameterHandle;
using MSLibrary.Security.Jwt.JwtGenerateCreateSignKeyServices;
using MSLibrary.Security.Jwt.JwtGenerateValidateSignKeyServices;
using MSLibrary.Security.Jwt.JwtValidateParameterBuildServices;
using MSLibrary.CommonQueue;
using MSLibrary.CommonQueue.QueueRealExecuteServices;
using MSLibrary.CommonQueue.MessageConvertServices.AzureServiceBus.To;
using MSLibrary.CommonQueue.MessageConvertServices.AzureServiceBus.From;
using MSLibrary.SystemToken;
using MSLibrary.SystemToken.TokenControllerServices;
using MSLibrary.Cache;
using MSLibrary.Cache.RealKVCacheVisitServices;
using MSLibrary.Schedule;
using MSLibrary.Schedule.ScheduleActionInitGeneratorServices;
using MSLibrary.MessageQueue.DAL;
using MSLibrary.MessageQueue.DAL.SMessageStores;
using MSLibrary.MessageQueue;
using MSLibrary.ExceptionHandle;
using MSLibrary.AspNet;
using MSLibrary.MySqlStore.Transaction;
using FW.TestPlatform.Main.Context.HttpExtensionContextHandleServices;
using FW.TestPlatform.Main.Context.ClaimContextGeneratorServices;
using FW.TestPlatform.Main.Context.EnvironmentClaimGeneratorServices;
using FW.TestPlatform.Main.Context;
using FW.TestPlatform.Main.Configuration;
using FW.TestPlatform.Main.Entities;
using FW.TestPlatform.Main.Entities.TestCaseHandleServices;
using FW.TestPlatform.Main.Template.LabelParameterHandlers;
using FW.TestPlatform.Main.Code;
using FW.TestPlatform.Main.Code.GetSeparatorServices;
using FW.TestPlatform.Main.Code.GetSpaceServices;
using FW.TestPlatform.Main.Code.GenerateDataVarDeclareServices;
using FW.TestPlatform.Main.Code.GenerateAdditionFuncServices;
using MSLibrary.CommandLine.SSH;
using MSLibrary.CommandLine.SSH.SSHEndpointServices;
using FW.TestPlatform.Main.Code.GenerateVarSettingServices;
using FW.TestPlatform.Main.Code.GenerateFuncInvokeServices;
using FW.TestPlatform.Main.Code.GenerateVarInvokeServices;
using FW.TestPlatform.Main.Collections.TreeEntityValueServices;
using FW.TestPlatform.Main.Entities.EntityTreeCopyServices;

namespace FW.TestPlatform.Main
{
    public static class MainStartupHelper
    {
        /// <summary>
        /// 初始化配置容器
        /// 首先尝试从后缀名-{环境名}的文件加载配置
        /// 如果该文件不存在，则加载去除后缀名后的文件
        /// <paramref name="environmentName">当前环境名称</paramref>
        /// <paramref name="fileBaseUrl">配置文件基地址</paramref>
        /// </summary>
        public static void InitConfigurationContainer(string environmentName, string fileBaseUrl)
        {
            var appConfigurationUri = $"{fileBaseUrl}{Path.DirectorySeparatorChar}Configurations{Path.DirectorySeparatorChar}app-{environmentName}.json";

            if (!File.Exists(appConfigurationUri))
            {
                appConfigurationUri = $"{fileBaseUrl}{Path.DirectorySeparatorChar}Configurations{Path.DirectorySeparatorChar}app.json";
            }


            var loggerConfigurationUri = $"{fileBaseUrl}{Path.DirectorySeparatorChar}Configurations{Path.DirectorySeparatorChar}logger-{environmentName}.json";

            if (!File.Exists(loggerConfigurationUri))
            {
                loggerConfigurationUri = $"{fileBaseUrl}{Path.DirectorySeparatorChar}Configurations{Path.DirectorySeparatorChar}logger.json";
            }


            //获取应用配置，用环境变量中的值替换关键信息
            var appContent = File.ReadAllText(appConfigurationUri);
            appContent = appContent
                .Replace($"{{{EnvironmentVarNames.AppName}}}", Environment.GetEnvironmentVariable(EnvironmentVarNames.AppName))
                ;


            ConfigurationContainer.Container = new ConfigurationContainerDefault();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonContent(appContent)
                .Build();

            var loggerConfiguration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile(loggerConfigurationUri, optional: true, reloadOnChange: true)
                .Build();


            //向配置容器增加主配置
            ConfigurationContainer.Add(ConfigurationNames.Application, configuration);

            //向配置容器增加日志配置
            ConfigurationContainer.Add(ConfigurationNames.Logger, loggerConfiguration);

        }



        /// <summary>
        /// 初始化上下文
        /// </summary>
        public static void InitContext()
        {
            ContextContainer.Current.Register<IDIContainer>(ContextTypes.DI, new ContextDIContainer());
            ContextContainer.Current.Register<Guid>(ContextTypes.CurrentUserId, new ContextCurrentUserId());
            ContextContainer.Current.Register<int>(ContextTypes.CurrentUserLcid, new ContextCurrentUserLcid());
            ContextContainer.Current.Register<int>(ContextTypes.CurrentUserTimezoneOffset, new ContextCurrentUserTimezoneOffset());
            ContextContainer.Current.Register<ConcurrentDictionary<string, object>>(ContextTypes.Dictionary, new ContextDictionary());
            ContextContainer.Current.Register<IRequestTraceInofContext>(ContextTypes.Trace, new ContextCurrentTrace());

        }



        /// <summary>
        /// 初始化DI容器
        /// 自动装载被标识的对象
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="dISetting"></param>
        public static void InitDI(IServiceCollection serviceCollection, DISetting dISetting)
        {

            //serviceCollection.AddHttpClient();
            serviceCollection.AddHttpClient();
            serviceCollection.AddHttpClient("A")  
                
                    .ConfigurePrimaryHttpMessageHandler(() =>
                        new HttpClientHandler()
                        {
                             
                            AllowAutoRedirect = false
                        }
                    );

            DIContainerContainer.DIContainer = new DIContainerDefault(serviceCollection, serviceCollection.BuildServiceProvider());
            DIContainerInit.Init = new DIContainerInitDefault();
            DIContainerInit.Execute(dISetting.SearchAssemblyNames);



            //装载需要手动处理的DI数据

            //Microsoft.AspNetCore.DataProtection.Repositories.IXmlRepository
            //DIContainerContainer.Inject<IXmlRepository, DataProtectionXmlRepository>(@"Configurations\Dataprotection\key.xml");
        }

        /// <summary>
        /// 初始化静态化信息
        /// 所有通过静态属性来配置的信息，都在该方法中初始化
        /// </summary>
        public static void InitStaticInfo()
        {


            //为HttpClinetHelper的HttpClientFactory赋值
            HttpClinetHelper.HttpClientFactoryGenerator = () => DIContainerContainer.Get<IHttpClientFactory>();

            //为AdfsHelper.HttpClientFactory赋值
            AdfsHelper.HttpClientFactoryGenerator = () => DIContainerContainer.Get<IHttpClientFactory>();

            //为日志构建器处理的提供方处理工厂赋值
            LoggingBuilderHandlerDefault.ProviderHandlerFactories[LoggerProviderHandlerNames.Local] = DIContainerContainer.Get<LoggingBuilderProviderHandlerForCommonLogLocalFactory>();
            LoggingBuilderHandlerDefault.ProviderHandlerFactories[LoggerProviderHandlerNames.Console] = DIContainerContainer.Get<LoggingBuilderProviderHandlerForConsoleFactory>();
            //LoggingBuilderHandlerDefault.ProviderHandlerFactories[LoggerProviderHandlerNames.ExceptionLess] = DIContainerContainer.Get<LoggingBuilderProviderHandlerForExceptionLessFactory>();


            //ClaimContextGeneratorIMP.ClaimContextGeneratorServiceFactories[ClaimContextGeneratorTypes.Administrator] = DIContainerContainer.Get<ClaimContextGeneratorServiceForAdministratorFactory>();
            //ClaimContextGeneratorIMP.ClaimContextGeneratorServiceFactories[ClaimContextGeneratorTypes.Default] = DIContainerContainer.Get<ClaimContextGeneratorServiceForDefaultFactory>();

            //InternationalizationHandleServiceFactorySelector.InternationalizationHandleServiceFactories[InternationalizationHandleServiceNames.Default] = DIContainerContainer.Get<InternationalizationHandleServiceForDefaultFactory>();

            KVCacheVisitorIMP.RealKVCacheVisitServiceFactories[KVCacheVisitServiceTypes.Combination] = DIContainerContainer.Get<RealKVCacheVisitServiceForCombinationFactory>();
            KVCacheVisitorIMP.RealKVCacheVisitServiceFactories[KVCacheVisitServiceTypes.LocalTimeout] = DIContainerContainer.Get<RealKVCacheVisitServiceForLocalTimeoutFactory>();
            KVCacheVisitorIMP.RealKVCacheVisitServiceFactories[KVCacheVisitServiceTypes.LocalVersion] = DIContainerContainer.Get<RealKVCacheVisitServiceForLocalVersionFactory>();

            HttpExtensionContextHandleServiceFactorySelector.HttpExtensionContextHandleServiceFactories[HttpExtensionContextHandleServiceNames.Internationalization]= DIContainerContainer.Get<HttpExtensionContextHandleServiceForInternationalizationFactory>();

  
            ClaimContextGeneratorIMP.ClaimContextGeneratorServiceFactories[ClaimContextGeneratorTypes.Default] = DIContainerContainer.Get<ClaimContextGeneratorServiceForDefaultFactory>();

            EnvironmentClaimGeneratorIMP.EnvironmentClaimGeneratorServiceFactories[ClaimContextGeneratorTypes.Default] = DIContainerContainer.Get<EnvironmentClaimGeneratorServiceForDefaultFactory>();

            ScheduleActionGroupIMP.ScheduleActionInitGeneratorServiceFactories[ScheduleActionInitTypes.Default] = DIContainerContainer.Get<ScheduleActionInitGeneratorForDefaultFactory>();

            TreeEntityIMP.ValueServices[TreeEntityValueServiceTypes.Folder]= DIContainerContainer.Get<TreeEntityValueServiceForFolderFactory>();
            TreeEntityIMP.ValueServices[TreeEntityValueServiceTypes.TestCase] = DIContainerContainer.Get<TreeEntityValueServiceForTestCaseFactory>();
            TreeEntityIMP.ValueServices[TreeEntityValueServiceTypes.TestDataSource] = DIContainerContainer.Get<TreeEntityValueServiceForTestDataSourceFactory>();

            var applicationConfiguration = ConfigurationContainer.Get<ApplicationConfiguration>(ConfigurationNames.Application);


            UserAuthorizeFilter.ErrorCatalogName = applicationConfiguration.ApplicationName;


            DBTransactionHelper.DBConnGenerates[DBTypes.MySql] = new DBConnGenerateForMySql();

            SSHEndpointIMP.SSHEndpointServiceFactories[SSHEndpointTypes.Default] = DIContainerContainer.Get<SSHEndpointServiceForOneFactory>();

            TestCaseIMP.HandleServiceFactories[EngineTypes.Http] = DIContainerContainer.Get<TestCaseHandleServiceForHttpFactory>();
            TestCaseIMP.HandleServiceFactories[EngineTypes.Tcp] = DIContainerContainer.Get<TestCaseHandleServiceForTcpFactory>();
            TestCaseIMP.HandleServiceFactories[EngineTypes.WebSocket] = DIContainerContainer.Get<TestCaseHandleServiceForWebSocketFactory>();

            TestCaseHandleServiceForTcp.AdditionFuncNames = new List<string> { 
                AdditionFuncNames.NameOnceJsonData, 
                AdditionFuncNames.TcpRR, 
                AdditionFuncNames.TcpRRWithConnect,
                AdditionFuncNames.GetJsonRowData,
                AdditionFuncNames.GetNameSerialNo,
                AdditionFuncNames.NumberFill,
                AdditionFuncNames.IntRange,
                AdditionFuncNames.DecimalRange,
                AdditionFuncNames.RanJsonData,
                AdditionFuncNames.DesSecurity,
                AdditionFuncNames.FilterJsonData,
                AdditionFuncNames.CalcCheckSum,
                AdditionFuncNames.GetJsonData,
                AdditionFuncNames.SplitJsonData,
                AdditionFuncNames.Print,
                AdditionFuncNames.FireEventRequest,
                AdditionFuncNames.DateTimeFormate,
                AdditionFuncNames.DateTimeAdd,
                AdditionFuncNames.HttpGetWithConnect,
                AdditionFuncNames.HttpPostWithConnect,
                AdditionFuncNames.WebSocketWithConnect };

            TestCaseHandleServiceForHttp.AdditionFuncNames = new List<string> {
                AdditionFuncNames.NameOnceJsonData,
                AdditionFuncNames.TcpRR,
                AdditionFuncNames.TcpRRWithConnect,
                AdditionFuncNames.GetJsonRowData,
                AdditionFuncNames.GetNameSerialNo,
                AdditionFuncNames.NumberFill,
                AdditionFuncNames.IntRange,
                AdditionFuncNames.DecimalRange,
                AdditionFuncNames.RanJsonData,
                AdditionFuncNames.DesSecurity,
                AdditionFuncNames.FilterJsonData,
                AdditionFuncNames.CalcCheckSum,
                AdditionFuncNames.GetJsonData,
                AdditionFuncNames.SplitJsonData,
                AdditionFuncNames.Print,
                AdditionFuncNames.FireEventRequest,
                AdditionFuncNames.DateTimeFormate,
                AdditionFuncNames.DateTimeAdd,
                AdditionFuncNames.HttpGetWithConnect,
                AdditionFuncNames.HttpPostWithConnect,
                AdditionFuncNames.WebSocketWithConnect };

            TestCaseHandleServiceForWebSocket.AdditionFuncNames = new List<string> {
                AdditionFuncNames.NameOnceJsonData,
                AdditionFuncNames.TcpRR,
                AdditionFuncNames.TcpRRWithConnect,
                AdditionFuncNames.GetJsonRowData,
                AdditionFuncNames.GetNameSerialNo,
                AdditionFuncNames.NumberFill,
                AdditionFuncNames.IntRange,
                AdditionFuncNames.DecimalRange,
                AdditionFuncNames.RanJsonData,
                AdditionFuncNames.DesSecurity,
                AdditionFuncNames.FilterJsonData,
                AdditionFuncNames.CalcCheckSum,
                AdditionFuncNames.GetJsonData,
                AdditionFuncNames.SplitJsonData,
                AdditionFuncNames.Print,
                AdditionFuncNames.FireEventRequest,
                AdditionFuncNames.DateTimeFormate,
                AdditionFuncNames.DateTimeAdd,
                AdditionFuncNames.HttpGetWithConnect,
                AdditionFuncNames.HttpPostWithConnect,
                AdditionFuncNames.WebSocketWithConnect };

            GenerateAdditionFuncServiceSelector.ServiceFactories[$"{RuntimeEngineTypes.Locust}-{AdditionFuncNames.NameOnceJsonData}"] = DIContainerContainer.Get<GenerateAdditionFuncServiceForLocustNameOnceJsonDataFactory>();
            GenerateAdditionFuncServiceSelector.ServiceFactories[$"{RuntimeEngineTypes.Locust}-{AdditionFuncNames.TcpRR}"] = DIContainerContainer.Get<GenerateAdditionFuncServiceForLocustTcpRRFactory>();
            GenerateAdditionFuncServiceSelector.ServiceFactories[$"{RuntimeEngineTypes.Locust}-{AdditionFuncNames.TcpRRWithConnect}"] = DIContainerContainer.Get<GenerateAdditionFuncServiceForLocustTcpRRWithConnectFactory>();
            GenerateAdditionFuncServiceSelector.ServiceFactories[$"{RuntimeEngineTypes.Locust}-{AdditionFuncNames.GetJsonRowData}"] = DIContainerContainer.Get<GenerateAdditionFuncServiceForLocustGetJsonRowDataFactory>();
            GenerateAdditionFuncServiceSelector.ServiceFactories[$"{RuntimeEngineTypes.Locust}-{AdditionFuncNames.GetNameSerialNo}"] = DIContainerContainer.Get<GenerateAdditionFuncServiceForLocustGetNameSerialNoFactory>();
            GenerateAdditionFuncServiceSelector.ServiceFactories[$"{RuntimeEngineTypes.Locust}-{AdditionFuncNames.NumberFill}"] = DIContainerContainer.Get<GenerateAdditionFuncServiceForLocustNumberFillFactory>();
            GenerateAdditionFuncServiceSelector.ServiceFactories[$"{RuntimeEngineTypes.Locust}-{AdditionFuncNames.IntRange}"] = DIContainerContainer.Get<GenerateAdditionFuncServiceForLocustIntRangeFactory>();
            GenerateAdditionFuncServiceSelector.ServiceFactories[$"{RuntimeEngineTypes.Locust}-{AdditionFuncNames.DecimalRange}"] = DIContainerContainer.Get<GenerateAdditionFuncServiceForLocustDecimalRangeFactory>();
            GenerateAdditionFuncServiceSelector.ServiceFactories[$"{RuntimeEngineTypes.Locust}-{AdditionFuncNames.RanJsonData}"] = DIContainerContainer.Get<GenerateAdditionFuncServiceForLocustRanJsonDataFactory>();
            GenerateAdditionFuncServiceSelector.ServiceFactories[$"{RuntimeEngineTypes.Locust}-{AdditionFuncNames.DesSecurity}"] = DIContainerContainer.Get<GenerateAdditionFuncServiceForLocustDesSecurityFactory>();
            GenerateAdditionFuncServiceSelector.ServiceFactories[$"{RuntimeEngineTypes.Locust}-{AdditionFuncNames.FilterJsonData}"] = DIContainerContainer.Get<GenerateAdditionFuncServiceForLocustFilterJsonDataFactory>();
            GenerateAdditionFuncServiceSelector.ServiceFactories[$"{RuntimeEngineTypes.Locust}-{AdditionFuncNames.CalcCheckSum}"] = DIContainerContainer.Get<GenerateAdditionFuncServiceForLocustCalcCheckSumFactory>();
            GenerateAdditionFuncServiceSelector.ServiceFactories[$"{RuntimeEngineTypes.Locust}-{AdditionFuncNames.GetJsonData}"] = DIContainerContainer.Get<GenerateAdditionFuncServiceForLocustGetJsonDataFactory>();
            GenerateAdditionFuncServiceSelector.ServiceFactories[$"{RuntimeEngineTypes.Locust}-{AdditionFuncNames.SplitJsonData}"] = DIContainerContainer.Get<GenerateAdditionFuncServiceForLocustSplitJsonDataFactory>();
            GenerateAdditionFuncServiceSelector.ServiceFactories[$"{RuntimeEngineTypes.Locust}-{AdditionFuncNames.Print}"] = DIContainerContainer.Get<GenerateAdditionFuncServiceForLocustPrintFactory>();
            GenerateAdditionFuncServiceSelector.ServiceFactories[$"{RuntimeEngineTypes.Locust}-{AdditionFuncNames.FireEventRequest}"] = DIContainerContainer.Get<GenerateAdditionFuncServiceForLocustFireEventRequestFactory>();
            GenerateAdditionFuncServiceSelector.ServiceFactories[$"{RuntimeEngineTypes.Locust}-{AdditionFuncNames.DateTimeFormate}"] = DIContainerContainer.Get<GenerateAdditionFuncServiceForLocustDateTimeFormateFactory>();
            GenerateAdditionFuncServiceSelector.ServiceFactories[$"{RuntimeEngineTypes.Locust}-{AdditionFuncNames.DateTimeAdd}"] = DIContainerContainer.Get<GenerateAdditionFuncServiceForLocustDateTimeAddFactory>();
            GenerateAdditionFuncServiceSelector.ServiceFactories[$"{RuntimeEngineTypes.Locust}-{AdditionFuncNames.HttpGetWithConnect}"] = DIContainerContainer.Get<GenerateAdditionFuncServiceForLocustHttpGetWithConnectFactory>();
            GenerateAdditionFuncServiceSelector.ServiceFactories[$"{RuntimeEngineTypes.Locust}-{AdditionFuncNames.HttpPostWithConnect}"] = DIContainerContainer.Get<GenerateAdditionFuncServiceForLocustHttpPostWithConnectFactory>();
            GenerateAdditionFuncServiceSelector.ServiceFactories[$"{RuntimeEngineTypes.Locust}-{AdditionFuncNames.WebSocketWithConnect}"] = DIContainerContainer.Get<GenerateAdditionFuncServiceForLocustWebSocketWithConnectFactory>();

            LabelParameterIMP.HandlerFactories[LabelParameterTypes.AdditionFunc] = DIContainerContainer.Get<LabelParameterHandlerForAdditionFuncFactory>();
            LabelParameterIMP.HandlerFactories[LabelParameterTypes.DataVarDeclareInit] = DIContainerContainer.Get<LabelParameterHandlerForDataVarDeclareInitFactory>();
            LabelParameterIMP.HandlerFactories[LabelParameterTypes.ConnectInit] = DIContainerContainer.Get<LabelParameterHandlerForConnectInitFactory>();
            LabelParameterIMP.HandlerFactories[LabelParameterTypes.SendInit] = DIContainerContainer.Get<LabelParameterHandlerForSendInitFactory>();
            LabelParameterIMP.HandlerFactories[LabelParameterTypes.StopInit] = DIContainerContainer.Get<LabelParameterHandlerForStopInitFactory>();
            LabelParameterIMP.HandlerFactories[LabelParameterTypes.SendData] = DIContainerContainer.Get<LabelParameterHandlerForSendDataFactory>();
            LabelParameterIMP.HandlerFactories[LabelParameterTypes.RecvData] = DIContainerContainer.Get<LabelParameterHandlerForRecvDataFactory>();
            LabelParameterIMP.HandlerFactories[LabelParameterTypes.RequestBody] = DIContainerContainer.Get<LabelParameterHandlerForRequestBodyFactory>();
            LabelParameterIMP.HandlerFactories[LabelParameterTypes.HostName] = DIContainerContainer.Get<LabelParameterHandlerForHostNameFactory>();
            LabelParameterIMP.HandlerFactories[LabelParameterTypes.CurConnectID] = DIContainerContainer.Get<LabelParameterHandlerForCurConnectIDFactory>();
            LabelParameterIMP.HandlerFactories[LabelParameterTypes.CurConnect] = DIContainerContainer.Get<LabelParameterHandlerForCurConnectFactory>();
            LabelParameterIMP.HandlerFactories[LabelParameterTypes.DataSource] = DIContainerContainer.Get<LabelParameterHandlerForDataSourceFactory>();
            LabelParameterIMP.HandlerFactories[LabelParameterTypes.NameOnceJsonDataInvoke] = DIContainerContainer.Get<LabelParameterHandlerForNameOnceJsonDataInvokeFactory>();
            LabelParameterIMP.HandlerFactories[LabelParameterTypes.TcpRRInvoke] = DIContainerContainer.Get<LabelParameterHandlerForTcpRRInvokeFactory>();
            LabelParameterIMP.HandlerFactories[LabelParameterTypes.TcpRRWithConnectInvoke] = DIContainerContainer.Get<LabelParameterHandlerForTcpRRWithConnectInvokeFactory>();
            LabelParameterIMP.HandlerFactories[LabelParameterTypes.GetJsonRowDataInvoke] = DIContainerContainer.Get<LabelParameterHandlerForGetJsonRowDataInvokeFactory>();
            LabelParameterIMP.HandlerFactories[LabelParameterTypes.GetNameSerialNoInvoke] = DIContainerContainer.Get<LabelParameterHandlerForGetNameSerialNoInvokeFactory>();
            LabelParameterIMP.HandlerFactories[LabelParameterTypes.NumberFill] = DIContainerContainer.Get<LabelParameterHandlerForNumberFillFactory>();
            LabelParameterIMP.HandlerFactories[LabelParameterTypes.IntRange] = DIContainerContainer.Get<LabelParameterHandlerForIntRangeFactory>();
            LabelParameterIMP.HandlerFactories[LabelParameterTypes.DecimalRange] = DIContainerContainer.Get<LabelParameterHandlerForDecimalRangeFactory>();
            LabelParameterIMP.HandlerFactories[LabelParameterTypes.RanJsonData] = DIContainerContainer.Get<LabelParameterHandlerForRanJsonDataFactory>();
            LabelParameterIMP.HandlerFactories[LabelParameterTypes.DesSecurity] = DIContainerContainer.Get<LabelParameterHandlerForDesSecurityFactory>();
            LabelParameterIMP.HandlerFactories[LabelParameterTypes.VarKV] = DIContainerContainer.Get<LabelParameterHandlerForVarKVFactory>();
            LabelParameterIMP.HandlerFactories[LabelParameterTypes.CurrConnectKV] = DIContainerContainer.Get<LabelParameterHandlerForCurrConnectKVFactory>();
            LabelParameterIMP.HandlerFactories[LabelParameterTypes.CaseID] = DIContainerContainer.Get<LabelParameterHandlerForCaseIDFactory>();
            LabelParameterIMP.HandlerFactories[LabelParameterTypes.CaseServiceBaseAddress] = DIContainerContainer.Get<LabelParameterHandlerForCaseServiceBaseAddressFactory>();
            LabelParameterIMP.HandlerFactories[LabelParameterTypes.FilterJsonDataInvoke] = DIContainerContainer.Get<LabelParameterHandlerForFilterJsonDataInvokeFactory>();
            LabelParameterIMP.HandlerFactories[LabelParameterTypes.CalcCheckSumInvoke] = DIContainerContainer.Get<LabelParameterHandlerForCalcCheckSumInvokeFactory>();
            LabelParameterIMP.HandlerFactories[LabelParameterTypes.SlaveName] = DIContainerContainer.Get<LabelParameterHandlerForSlaveNameFactory>();
            LabelParameterIMP.HandlerFactories[LabelParameterTypes.GetJsonDataInvoke] = DIContainerContainer.Get<LabelParameterHandlerForGetJsonDataInvokeFactory>();
            LabelParameterIMP.HandlerFactories[LabelParameterTypes.SplitJsonDataInvoke] = DIContainerContainer.Get<LabelParameterHandlerForSplitJsonDataInvokeFactory>();
            LabelParameterIMP.HandlerFactories[LabelParameterTypes.Print] = DIContainerContainer.Get<LabelParameterHandlerForPrintFactory>();
            LabelParameterIMP.HandlerFactories[LabelParameterTypes.HttpGetWithConnectInvoke] = DIContainerContainer.Get<LabelParameterHandlerForHttpGetWithConnectInvokeFactory>();
            LabelParameterIMP.HandlerFactories[LabelParameterTypes.HttpPostWithConnectInvoke] = DIContainerContainer.Get<LabelParameterHandlerForHttpPostWithConnectInvokeFactory>();
            LabelParameterIMP.HandlerFactories[LabelParameterTypes.Now] = DIContainerContainer.Get<LabelParameterHandlerForNowFactory>();
            LabelParameterIMP.HandlerFactories[LabelParameterTypes.Time] = DIContainerContainer.Get<LabelParameterHandlerForTimeFactory>();
            LabelParameterIMP.HandlerFactories[LabelParameterTypes.Sleep] = DIContainerContainer.Get<LabelParameterHandlerForSleepFactory>();
            LabelParameterIMP.HandlerFactories[LabelParameterTypes.FireEventRequest] = DIContainerContainer.Get<LabelParameterHandlerForFireEventRequestFactory>();
            LabelParameterIMP.HandlerFactories[LabelParameterTypes.DateTimeFormate] = DIContainerContainer.Get<LabelParameterHandlerForFireEventRequestFactory>();
            LabelParameterIMP.HandlerFactories[LabelParameterTypes.DateTimeAdd] = DIContainerContainer.Get<LabelParameterHandlerForFireEventRequestFactory>();
            LabelParameterIMP.HandlerFactories[LabelParameterTypes.UserName] = DIContainerContainer.Get<LabelParameterHandlerForUserNameFactory>();
            LabelParameterIMP.HandlerFactories[LabelParameterTypes.WebSocketWithConnectInvoke] = DIContainerContainer.Get<LabelParameterHandlerForWebSocketWithConnectInvokeFactory>();

            GetSeparatorServiceSelector.GetSeparatorServiceFactories[RuntimeEngineTypes.Locust] = DIContainerContainer.Get<GetSeparatorServiceForLocustFactory>();
            GetSpaceServiceSelector.GetSpaceServiceFactories[RuntimeEngineTypes.Locust] = DIContainerContainer.Get<GetSpaceServiceForLocustFactory>();

            GenerateDataVarDeclareServiceSelector.ServiceFactories[$"{RuntimeEngineTypes.Locust}"] = DIContainerContainer.Get<GenerateDataVarDeclareServiceForLocustFactory>();
            GenerateDataVarDeclareServiceSelector.ServiceFactories[$"{RuntimeEngineTypes.Locust}-{DataSourceTypes.Label}"] = DIContainerContainer.Get<GenerateDataVarDeclareServiceForLocustLabelFactory>();
            GenerateDataVarDeclareServiceSelector.ServiceFactories[$"{RuntimeEngineTypes.Locust}-{DataSourceTypes.String}"] = DIContainerContainer.Get<GenerateDataVarDeclareServiceForLocustStringFactory>();
            GenerateDataVarDeclareServiceSelector.ServiceFactories[$"{RuntimeEngineTypes.Locust}-{DataSourceTypes.Int}"] = DIContainerContainer.Get<GenerateDataVarDeclareServiceForLocustIntFactory>();
            GenerateDataVarDeclareServiceSelector.ServiceFactories[$"{RuntimeEngineTypes.Locust}-{DataSourceTypes.Json}"] = DIContainerContainer.Get<GenerateDataVarDeclareServiceForLocustJsonFactory>();

            GenerateFuncInvokeServiceSelector.ServiceFactories[$"{RuntimeEngineTypes.Locust}"] = DIContainerContainer.Get<GenerateFuncInvokeServiceForLocustFactory>();

            GenerateVarInvokeServiceSelector.ServiceFactories[$"{RuntimeEngineTypes.Locust}"] = DIContainerContainer.Get<GenerateVarInvokeServiceForLocustFactory>();
            GenerateVarInvokeServiceSelector.ServiceFactories[$"{RuntimeEngineTypes.Locust}-{LabelParameterTypes.CurrConnectKV}"] = DIContainerContainer.Get<GenerateVarInvokeServiceForLocustCurrConnectKVFactory>();
            GenerateVarInvokeServiceSelector.ServiceFactories[$"{RuntimeEngineTypes.Locust}-{LabelParameterTypes.Now}"] = DIContainerContainer.Get<GenerateVarInvokeServiceForLocustNowFactory>();
            GenerateVarInvokeServiceSelector.ServiceFactories[$"{RuntimeEngineTypes.Locust}-{LabelParameterTypes.Time}"] = DIContainerContainer.Get<GenerateVarInvokeServiceForLocustTimeFactory>();
            GenerateVarInvokeServiceSelector.ServiceFactories[$"{RuntimeEngineTypes.Locust}-{LabelParameterTypes.Sleep}"] = DIContainerContainer.Get<GenerateVarInvokeServiceForLocustSleepFactory>();
            GenerateVarInvokeServiceSelector.ServiceFactories[$"{RuntimeEngineTypes.Locust}-{LabelParameterTypes.VarKV}"] = DIContainerContainer.Get<GenerateVarInvokeServiceForLocustVarKVFactory>();

            GenerateVarSettingServiceSelector.ServiceFactories[$"{RuntimeEngineTypes.Locust}"] = DIContainerContainer.Get<GenerateVarSettingServiceForLocustFactory>();
        
        
            EntityTreeCopyService.EntityTreeCopyServiceFactories[EntityTreeCopyServiceTypes.TestCase]= DIContainerContainer.Get<EntityTreeCopyServiceForTestCaseFactory>();
            EntityTreeCopyService.EntityTreeCopyServiceFactories[EntityTreeCopyServiceTypes.TestDataSource] = DIContainerContainer.Get<EntityTreeCopyServiceForTestDataSourceFactory>();
        }





        /// <summary>
        /// 初始化日志
        /// </summary>
        /// <param name="builder"></param>
        public static void InitLogger(ILoggingBuilder builder)
        {
            var mainHandler = DIContainerContainer.Get<ILoggingBuilderHandler>();
            var loggerConfiguration = ConfigurationContainer.Get<LoggerConfiguration>(ConfigurationNames.Logger);

            mainHandler.Execute(builder, loggerConfiguration).Wait();
        }
    }
}
