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

namespace MSLibrary.Logger.LoggingBuilderProviderHandlers
{
    /// <summary>
    /// 针对CommonLog提供方的日志构造器处理
    /// </summary>
    [Injection(InterfaceType = typeof(LoggingBuilderProviderHandlerForCommonLog), Scope = InjectionScope.Singleton)]
    public class LoggingBuilderProviderHandlerForCommonLog : ILoggingBuilderProviderHandler
    {
        private CommonLogProvider _commonLogProvider;

        public LoggingBuilderProviderHandlerForCommonLog(CommonLogProvider commonLogProvider)
        {
            _commonLogProvider = commonLogProvider;
        }

        public async Task Execute(ILoggingBuilder builder, LoggerItemConfiguration configuration)
        {
            builder.AddProvider(_commonLogProvider);
            //配置级别过滤
            foreach (var filterItem in configuration.LogLevels)
            {
                builder.AddFilter<CommonLogProvider>(filterItem.Key, filterItem.Value);
            }

            //配置其他未指定目录的最低级别
            builder.AddFilter<CommonLogProvider>((level) =>
            {
                if (level < configuration.DefaultMinLevel)
                {
                    return false;
                }

                return true;
            });
            await Task.FromResult(0);
        }



        //[DataContract]
        //private class CommonLogConfiguration
        //{
        //    [DataMember]
        //    public string Key { get; set; }
        //    [DataMember]
        //    public bool IncludeScopes { get; set; }
        //    [DataMember]
        //    public bool TrackExceptionsAsExceptionTelemetry { get; set; }
        //}
    }

}
