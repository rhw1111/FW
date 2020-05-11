using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.OData.Client;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.OData
{
    [Injection(InterfaceType = typeof(IODataClientFactory), Scope = InjectionScope.Singleton)]
    public class ODataClientFactory:IODataClientFactory
    {
        private static Dictionary<Type, IFactory<IODataClientGenerator>> _clientGeneratorFactories = new Dictionary<Type, IFactory<IODataClientGenerator>>();
        private static Dictionary<string, IFactory<IODataClientInitialization>> _clientInitializationFactories = new Dictionary<string, IFactory<IODataClientInitialization>>();

        /// <summary>
        /// OData客户端对象生成器工厂键值对
        /// </summary>
        public static Dictionary<Type, IFactory<IODataClientGenerator>> ClientGeneratorFactories
        {
            get
            {
                return _clientGeneratorFactories;
            }
        }
        /// <summary>
        /// ODataClient初始化工厂键值对
        /// </summary>
        public static Dictionary<string, IFactory<IODataClientInitialization>> ClientInitializationFactories
        {
            get
            {
                return _clientInitializationFactories;
            }
        }

        public async Task<T> Create<T>(ODataClientConnectConfiguration configuration) where T : DataServiceContext
        {
            if (!_clientGeneratorFactories.TryGetValue(typeof(T), out IFactory<IODataClientGenerator> generatorFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundODataClientGeneratorByType,
                    DefaultFormatting = "找不到类型为{0}的OData客户端生成器，发生位置：{1}",
                    ReplaceParameters = new List<object>() { typeof(T).FullName, $"{this.GetType().FullName}.ClientGeneratorFactories" }
                };
                throw new UtilityException((int)Errors.NotFoundODataClientGeneratorByType, fragment);
            }

            if (!_clientInitializationFactories.TryGetValue(configuration.InitType,out IFactory<IODataClientInitialization> initializationFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundODataClientInitializationByConfiguration,
                    DefaultFormatting = "找不到指定配置类型为{0}的OData客户端初始化器，发生位置：{1}",
                    ReplaceParameters = new List<object>() { configuration.InitConfigurationString, $"{this.GetType().FullName}.ClientInitializationFactories" }
                };
                throw new UtilityException((int)Errors.NotFoundODataClientInitializationByConfiguration, fragment);
            }

            var client=await generatorFactory.Create().Generate(configuration.GenerateConfigurationString);
            await initializationFactory.Create().Init(client, configuration.InitConfigurationString);

            return (T)client;
        }
    }

    /// <summary>
    /// OData客户端对象生成接口
    /// </summary>
    public interface IODataClientGenerator
    {
        Task<DataServiceContext> Generate(string configuration);
    }

    /// <summary>
    /// ODataClient初始化接口
    /// </summary>
    public interface IODataClientInitialization
    {
        Task Init(DataServiceContext client,string configuration);
    }
}
