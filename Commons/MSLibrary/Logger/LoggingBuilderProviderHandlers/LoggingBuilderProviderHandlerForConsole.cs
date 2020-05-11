using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Newtonsoft.Json.Linq;
using MSLibrary.DI;
using MSLibrary.Serializer;

namespace MSLibrary.Logger.LoggingBuilderProviderHandlers
{
    /// <summary>
    /// 针对控制台提供方的日志构造器处理
    /// </summary>
    [Injection(InterfaceType = typeof(LoggingBuilderProviderHandlerForConsole), Scope = InjectionScope.Singleton)]
    public class LoggingBuilderProviderHandlerForConsole : ILoggingBuilderProviderHandler
    {
        public async Task Execute(ILoggingBuilder builder, LoggerItemConfiguration configuration)
        {
            //反序列化特定配置
            var innerConfiguration=JsonSerializerHelper.Deserialize<ConsoleConfiguration>(JsonSerializerHelper.Serializer(configuration.ConfigurationObj));
            //绑定提供方
            builder.AddConsole((opt)=>
                {
                    if (innerConfiguration!=null)
                    {
                        opt.DisableColors = innerConfiguration.DisableColors;
                        opt.IncludeScopes = innerConfiguration.IncludeScopes;
                    }
                    
            });


            //配置级别过滤
            foreach(var filterItem in configuration.LogLevels)
            {
                builder.AddFilter<ConsoleLoggerProvider>(filterItem.Key, filterItem.Value);
            }

            //配置其他未指定目录的最低级别
            builder.AddFilter<ConsoleLoggerProvider>((level) =>
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
        private class ConsoleConfiguration
        {
            [DataMember]
            public bool IncludeScopes { get; set; }
            [DataMember]
            public bool DisableColors { get; set; }
        }
    }
}
