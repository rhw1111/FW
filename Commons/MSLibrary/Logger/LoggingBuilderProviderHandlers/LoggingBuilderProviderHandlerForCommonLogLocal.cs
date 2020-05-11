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
    /// 针对CommonLogLocal提供方的日志构造器处理
    /// </summary>
    [Injection(InterfaceType = typeof(LoggingBuilderProviderHandlerForCommonLogLocal), Scope = InjectionScope.Singleton)]
    public class LoggingBuilderProviderHandlerForCommonLogLocal : ILoggingBuilderProviderHandler
    {
        private CommonLogLocalProvider _commonLogLocalProvider;

        public LoggingBuilderProviderHandlerForCommonLogLocal(CommonLogLocalProvider commonLogLocalProvider)
        {
            _commonLogLocalProvider = commonLogLocalProvider;
        }
        public async Task Execute(ILoggingBuilder builder, LoggerItemConfiguration configuration)
        {
            builder.AddProvider(_commonLogLocalProvider);

            //配置级别过滤
            foreach (var filterItem in configuration.LogLevels)
            {
                builder.AddFilter<CommonLogLocalProvider>(filterItem.Key, filterItem.Value);
            }

            //配置其他未指定目录的最低级别
            builder.AddFilter<CommonLogLocalProvider>((level) =>
            {
                if (level< configuration.DefaultMinLevel)
                {
                    return false;
                }

                return true;
            });
            await Task.FromResult(0);
        }
    }
}
