using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using MSLibrary.Configuration;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using MSLibrary.Serializer;
using System.Runtime.CompilerServices;

namespace MSLibrary.Logger
{
    /// <summary>
    /// 日志构建器默认处理
    /// </summary>
    [Injection(InterfaceType = typeof(ILoggingBuilderHandler), Scope = InjectionScope.Singleton)]
    public class LoggingBuilderHandlerDefault : ILoggingBuilderHandler
    {
        private static Dictionary<string, IFactory<ILoggingBuilderProviderHandler>> _providerHandlerFactories = new Dictionary<string, IFactory<ILoggingBuilderProviderHandler>>();

        public static Dictionary<string, IFactory<ILoggingBuilderProviderHandler>> ProviderHandlerFactories
        {
            get
            {
                return _providerHandlerFactories;
            }
        }

        public async Task Execute(ILoggingBuilder builder, LoggerConfiguration configuration)
        {
            if (!configuration.RemainAllProvider)
            {
                builder.ClearProviders();
            }

            //处理全局日志级别
            foreach (var levelItem in configuration.GlobalLogLevels)
            {
                builder.AddFilter(levelItem.Key, levelItem.Value);
            }

            //设置全局日志级别（为指定目录）
            builder.AddFilter((level) =>
            {
                if (level< configuration.GlobalLogDefaultMinLevel)
                {
                    return false;
                }
                return true;
            });

            
            //处理每一个提供方
            foreach(var providerItem in configuration.Providers)
            {
                providerItem.Value.ConfigurationObj = JsonSerializerHelper.Deserialize<JObject>(JsonSerializerHelper.Serializer(providerItem.Value.Configuration));
                var providerHandler = getProviderHandler(providerItem.Key);
                await providerHandler.Execute(builder, providerItem.Value);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ILoggingBuilderProviderHandler getProviderHandler(string type)
        {
            if (!_providerHandlerFactories.TryGetValue(type,out IFactory<ILoggingBuilderProviderHandler> serviceFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundLoggingBuilderProviderHandlerByType,
                    DefaultFormatting = "找不到类型为{0}的针对日志提供方的日志构建器处理，发生位置：{1}",
                    ReplaceParameters = new List<object>() { type, $"{typeof(LoggingBuilderHandlerDefault).FullName}.ProviderHandlerFactories" }
                };

                throw new UtilityException((int)Errors.NotFoundLoggingBuilderProviderHandlerByType, fragment);
            }

            return  serviceFactory.Create();

        }
    }


    /// <summary>
    /// 日志构建器提供方处理
    /// </summary>
    public interface ILoggingBuilderProviderHandler
    {
        Task Execute(ILoggingBuilder builder,LoggerItemConfiguration configuration);
    }
}
