using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.ApplicationInsights;
using Newtonsoft.Json.Linq;
using MSLibrary.DI;
using MSLibrary.Serializer;
using Microsoft.ApplicationInsights.Channel;
using System.Reflection;
using System.Linq;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.DependencyInjection;

namespace MSLibrary.Logger.LoggingBuilderProviderHandlers
{
    /// <summary>
    /// 针对ApplicationInsights提供方的日志构造器处理
    /// </summary>
    [Injection(InterfaceType = typeof(LoggingBuilderProviderHandlerForApplicationInsights), Scope = InjectionScope.Singleton)]
    public class LoggingBuilderProviderHandlerForApplicationInsights : ILoggingBuilderProviderHandler
    {
        public async Task Execute(ILoggingBuilder builder, LoggerItemConfiguration configuration)
        {
            //反序列化特定配置
            var innerConfiguration = JsonSerializerHelper.Deserialize<ApplicationInsightsConfiguration>(JsonSerializerHelper.Serializer(configuration.ConfigurationObj));

            builder.Services.Configure<TelemetryConfiguration>((config) =>
            {
                config.TelemetryInitializers.Add(DIContainerContainer.Get<CustomTelemetryInitializer>());
            });

            //绑定提供方
            builder.AddApplicationInsights(innerConfiguration.Key, (opt) =>
             {
                 opt.IncludeScopes = innerConfiguration.IncludeScopes;
                 opt.TrackExceptionsAsExceptionTelemetry = innerConfiguration.TrackExceptionsAsExceptionTelemetry;
             });
            //配置级别过滤
            foreach (var filterItem in configuration.LogLevels)
            {
                builder.AddFilter<ApplicationInsightsLoggerProvider>(filterItem.Key, filterItem.Value);
            }

            //配置其他未指定目录的最低级别
            builder.AddFilter<ApplicationInsightsLoggerProvider>((level) =>
            {
                if (level < configuration.DefaultMinLevel)
                {
                   
                    return false;
                }

                return true;
            });

            await Task.FromResult(0);

        }


        [DataContract]
        private class ApplicationInsightsConfiguration
        {
            [DataMember]
            public string Key { get; set; }
            [DataMember]
            public bool IncludeScopes { get; set; }
            [DataMember]
            public bool TrackExceptionsAsExceptionTelemetry { get; set; }
        }
    }

    /// <summary>
    /// 客户化遥测推送序列化器
    /// </summary>
    [Injection(InterfaceType = typeof(CustomTelemetryInitializer), Scope = InjectionScope.Singleton)]
    public class CustomTelemetryInitializer : ITelemetryInitializer
    {
        private readonly ITelemetryInitService _telemetryInitService;

        public CustomTelemetryInitializer(ITelemetryInitService telemetryInitService)
        {
            _telemetryInitService = telemetryInitService;
        }

        public void Initialize(ITelemetry telemetry)
        {
            _telemetryInitService.Init(telemetry);
        }
    }

    /// <summary>
    /// ApplicationInsights的遥测推送对象初始化服务  
    /// </summary>
    public interface ITelemetryInitService
    {
        void Init(ITelemetry telemetry);
    }

    [Injection(InterfaceType = typeof(ITelemetryInitService), Scope = InjectionScope.Singleton)]
    public class TelemetryInitServiceDefault : ITelemetryInitService
    {
        public void Init(ITelemetry telemetry)
        {
            telemetry.Context.Component.Version = Assembly.GetEntryAssembly().GetCustomAttributes<AssemblyInformationalVersionAttribute>()
                .Select(att => att.InformationalVersion).FirstOrDefault();
        }
    }
}
