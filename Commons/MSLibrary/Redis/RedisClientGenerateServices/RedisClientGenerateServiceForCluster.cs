using CSRedis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using MSLibrary.Serializer;
using MSLibrary.LanguageTranslate;
using MSLibrary.DI;
using CSRedis;

namespace MSLibrary.Redis.RedisClientGenerateServices
{
    /// <summary>
    /// 针对官方集群的Redis客户端生成服务
    /// 配置格式为
    /// {
    ///     "Urls":["集群地址1","集群地址2"],
    ///     "ConnectionString":"连接字符串"
    /// }
    /// </summary>
    [Injection(InterfaceType = typeof(RedisClientGenerateServiceForCluster), Scope = InjectionScope.Singleton)]
    public class RedisClientGenerateServiceForCluster : IRedisClientGenerateService
    {
        private static Dictionary<string, CSRedisClient> _clients = new Dictionary<string, CSRedisClient>();

        public async Task<CSRedisClient> GenerateClient(string configuration)
        {

            return await Task.FromResult(GenerateClientSync(configuration));
        }

        public CSRedisClient GenerateClientSync(string configuration)
        {
            var configurationObj = JsonSerializerHelper.Deserialize<Configuration>(configuration);
            string strError = string.Empty;
            if (!_clients.TryGetValue(configuration, out CSRedisClient client))
            {
                lock (_clients)
                {
                    if (!_clients.TryGetValue(configuration, out client))
                    {
                        foreach (var item in configurationObj.Urls)
                        {
                            try
                            {
                                client = new CSRedisClient($"{item},{configurationObj.ConnectionString}");
    
                                break;
                            }
                            catch (Exception ex)
                            {
                                strError = ex.ToStackTraceString();
                            }
                        }

                        if (client == null)
                        {
                            var fragment = new TextFragment()
                            {
                                Code = TextCodes.GenerateRedisClientError,
                                DefaultFormatting = "生成Redis客户端错误,配置信息为{0}，错误内容为{1}",
                                ReplaceParameters = new List<object>() { configuration, strError }
                            };

                            throw new UtilityException((int)Errors.GenerateRedisClientError, fragment);
                        }

                        _clients[configuration] = client;
                    }
                }
            }

            return client;
        }

        [DataContract]
        private class Configuration
        {
            [DataMember]
            public string[] Urls { get; set; }
            [DataMember]
            public string ConnectionString { get; set; }
        }
    }
}
