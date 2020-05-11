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
    /// 针对ExceptionLess提供方的日志构造器处理
    /// </summary>
    [Injection(InterfaceType = typeof(LoggingBuilderProviderHandlerForExceptionLess), Scope = InjectionScope.Singleton)]
    public class LoggingBuilderProviderHandlerForExceptionLess : ILoggingBuilderProviderHandler
    {
        private ExceptionLessProvider _exceptionLessProvider;

        public LoggingBuilderProviderHandlerForExceptionLess(ExceptionLessProvider exceptionLessProvider)
        {
            _exceptionLessProvider = exceptionLessProvider;
        }

        public async Task Execute(ILoggingBuilder builder, LoggerItemConfiguration configuration)
        {
            //反序列化特定配置
            var innerConfiguration = JsonSerializerHelper.Deserialize<ExceptionLessConfiguration>(JsonSerializerHelper.Serializer(configuration.ConfigurationObj));
            _exceptionLessProvider.Key = innerConfiguration.Key;
            if (!string.IsNullOrEmpty(innerConfiguration.ServiceUri))
            {
                _exceptionLessProvider.ServiceUri = innerConfiguration.ServiceUri;
            }

            builder.AddProvider(_exceptionLessProvider);
            //配置级别过滤
            foreach (var filterItem in configuration.LogLevels)
            {
                builder.AddFilter<ExceptionLessProvider>(filterItem.Key, filterItem.Value);
            }

            //配置其他未指定目录的最低级别
            builder.AddFilter<ExceptionLessProvider>((level) =>
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
        private class ExceptionLessConfiguration
        {
            [DataMember]
            public string Key { get; set; }
            [DataMember]
            public string ServiceUri { get; set; }
        }
    }
}
