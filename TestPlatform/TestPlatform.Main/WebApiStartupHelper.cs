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
using FW.TestPlatform.Main.Code.GenerateDataVarDeclareServices;
using FW.TestPlatform.Main.Code.GenerateAdditionFuncServices;
using MSLibrary.CommandLine.SSH;
using MSLibrary.CommandLine.SSH.SSHEndpointServices;


namespace FW.TestPlatform.Main
{
    public static class WebApiStartupHelper
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
            var hostConfigurationUri = $"{fileBaseUrl}{Path.DirectorySeparatorChar}Configurations{Path.DirectorySeparatorChar}host-{environmentName}.json";

            if (!File.Exists(hostConfigurationUri))
            {
                hostConfigurationUri = $"{fileBaseUrl}{Path.DirectorySeparatorChar}Configurations{Path.DirectorySeparatorChar}host.json";
            }

            var hostConfiguration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(hostConfigurationUri, optional: true, reloadOnChange: true)
                .Build();


            //向配置容器增加主机配置信息
            ConfigurationContainer.Add(ConfigurationNames.Host, hostConfiguration);



        }



        /// <summary>
        /// 初始化上下文
        /// </summary>
        public static void InitContext()
        {

        }



        /// <summary>
        /// 初始化DI容器
        /// 自动装载被标识的对象
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="dISetting"></param>
        public static void InitDI(IServiceCollection serviceCollection, DISetting dISetting)
        {
        }

        /// <summary>
        /// 初始化静态化信息
        /// 所有通过静态属性来配置的信息，都在该方法中初始化
        /// </summary>
        public static void InitStaticInfo()
        {

        }





        /// <summary>
        /// 初始化日志
        /// </summary>
        /// <param name="builder"></param>
        public static void InitLogger(ILoggingBuilder builder)
        {

        }
    }
}
