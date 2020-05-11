using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Microsoft.OData.Client;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using MSLibrary.DI;
using MSLibrary.Serializer;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.OData.ClientInitializations
{
    /// <summary>
    /// 针对AAD服务端之间通信的初始化
    /// configuration格式要求为
    /// {
    ///     "Tenant":"租户",
    ///     "ClientAppId":"应用程序Id",
    ///     "ClientAppSecret":"应用程序密钥",
    ///     "Resource":"请求资源",
    ///     "TokenRefreashDuration":"令牌刷新周期，单位秒，0表示不刷新",
    ///     "TokenHeaderName":OData客户端存储令牌的头名称
    /// }
    /// </summary>
    [Injection(InterfaceType = typeof(ODataClientInitializationForAADSTS), Scope = InjectionScope.Singleton)]
    public class ODataClientInitializationForAADSTS:IODataClientInitialization
    {
        private static ConcurrentDictionary<string, string> _tokens = new ConcurrentDictionary<string, string>();
        private static ConcurrentDictionary<string, Task> _tokenActions = new ConcurrentDictionary<string, Task>();
        private static ConcurrentDictionary<string, string> _tokenControllers = new ConcurrentDictionary<string, string>();

        public async Task Init(DataServiceContext client, string configuration)
        {
            var aadSTSConfiguration=JsonSerializerHelper.Deserialize<AADSTSConfiguration>(configuration);

            if (aadSTSConfiguration==null || aadSTSConfiguration.Tenant==null || aadSTSConfiguration.ClientAppId==null || aadSTSConfiguration.ClientAppSecret==null)
            {
                string strCorrectFormat = @"{
                 ""Tenant"":""租户"",
                 ""ClientAppId"":""应用程序Id"",
                 ""ClientAppSecret"":""应用程序密钥"",
                 ""Resource"":""请求资源"",
                 ""TokenRefreashDuration"":""令牌刷新周期，单位秒，0表示不刷新"",
                 ""TokenHeaderName"":""OData客户端存储令牌的头名称""
            }";

                var fragment = new TextFragment()
                {
                    Code = TextCodes.ODataClientInitConfigurationNotCorrect,
                    DefaultFormatting = "OData客户端初始化配置不正确，期待的格式为{0}，现在的内容为{1}，发生的位置：{2}",
                    ReplaceParameters = new List<object>() { strCorrectFormat, configuration, $"{this.GetType().FullName}.Init" }
                };
                throw new UtilityException((int)Errors.ODataClientInitConfigurationNotCorrect, fragment);
            }


            if (!_tokens.TryGetValue(configuration,out string token))
            {
                token = await getToken(configuration, aadSTSConfiguration);
                _tokens[configuration] = token;
            }

            //client.BaseUri = new Uri(aadSTSConfiguration.ODataServiceUri);

            client.SendingRequest2 += new EventHandler<SendingRequest2EventArgs>(delegate (object sender, SendingRequest2EventArgs e)
            {
                var authenticationHeader = token;
                e.RequestMessage.SetHeader(aadSTSConfiguration.TokenHeaderName, authenticationHeader);
            });
        }

        private async Task<string> getToken(string configuration,AADSTSConfiguration aadSTSConfiguration)
        {
            AuthenticationContext authenticationContext = new AuthenticationContext(aadSTSConfiguration.Tenant, false);
            AuthenticationResult authenticationResult;

            var creadential = new ClientCredential(aadSTSConfiguration.ClientAppId, aadSTSConfiguration.ClientAppSecret);
            authenticationResult = await authenticationContext.AcquireTokenAsync(aadSTSConfiguration.Resource, creadential);

            var token = authenticationResult.CreateAuthorizationHeader();


            if (!_tokenActions.ContainsKey(configuration))
            {
                lock(_tokenActions)
                {
                    if (!_tokenActions.ContainsKey(configuration))
                    {
                        _tokenControllers[configuration] = configuration;
                        var task=Task.Run(async () =>
                        {
                            if (aadSTSConfiguration.TokenRefreashDuration >= 1)
                            {
                                while (true)
                                {
                                    if (!_tokenControllers.ContainsKey(configuration))
                                    {
                                        break;
                                    }
                                    await Task.Delay(aadSTSConfiguration.TokenRefreashDuration * 1000);
                                    authenticationResult = await authenticationContext.AcquireTokenAsync(aadSTSConfiguration.Resource, creadential);
                                    var newToken = authenticationResult.CreateAuthorizationHeader();
                                    _tokens[configuration] = newToken;
                                }
                            }
                        });
                   
                        _tokenActions[configuration] = task;
                    }
                }
            }
            return token;
        }


        public static void Clear(string configuration)
        {
            _tokenControllers.TryRemove(configuration, out string config);
            _tokenActions.TryRemove(configuration, out Task task);
            _tokens.TryRemove(configuration, out string token);
        }

        [DataContract]
        private class AADSTSConfiguration
        {
            [DataMember]
            public string Tenant { get; set; }
            [DataMember]
            public string ClientAppId { get; set; }
            [DataMember]
            public string ClientAppSecret { get; set; }
            [DataMember]
            public string Resource { get; set; }
            [DataMember]
            public int TokenRefreashDuration { get; set; }
            [DataMember]
            public string TokenHeaderName { get; set; }
        }
    }
}
