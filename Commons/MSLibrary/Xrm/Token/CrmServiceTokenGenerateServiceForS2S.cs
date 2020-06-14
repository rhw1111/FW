using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Runtime.Serialization;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using MSLibrary.Serializer;
using MSLibrary.Collections;

namespace MSLibrary.Xrm.Token
{
    /// <summary>
    /// 针对S2S模式的令牌生成
    /// 需要传入的参数：
    /// BaseUri:string,Azure云login的基地址
    /// ApplicationId:string,应用的Id
    /// ApplicationKey:string,应用的密钥
    /// CrmUrl:string,Crm的地址
    /// AADId:string,Application所在的AAD的id,可以为null，这种情况下将多请求一次，用来获取实际带aadid的验证地址
    /// </summary>
    [Injection(InterfaceType = typeof(CrmServiceTokenGenerateServiceForS2S), Scope = InjectionScope.Singleton)]
    public class CrmServiceTokenGenerateServiceForS2S : ICrmServiceTokenGenerateService
    {
        private static Dictionary<string, AuthenticationContextContainer> _authenticationContexts = new Dictionary<string, AuthenticationContextContainer>();

        private static int _limit = 10000;

        private static int _poolLimit = 100;
        /// <summary>
        /// 以连接字符串为键的键值对最大长度
        /// </summary>
        public static int Limit
        {
            set
            {
                _limit = value;
            }
        }
        /// <summary>
        /// 每个键对应的池的最大长度
        /// </summary>
        public static int PoolLimit
        {
            set
            {
                _poolLimit = value;
            }
        }

        public async Task<string> Genereate(Dictionary<string, object> parameters)
        {
            //检查参数
            if (!parameters.ContainsKey(CrmServiceTokenGenerateServiceParameterNames.BaseUri) || parameters[CrmServiceTokenGenerateServiceParameterNames.BaseUri] == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundParameterInCrmServiceTokenGenerateService,
                    DefaultFormatting = "在Crm服务令牌生成服务{0}中，找不到名称为{1}的参数",
                    ReplaceParameters = new List<object>() { this.GetType().FullName, CrmServiceTokenGenerateServiceParameterNames.BaseUri }
                };

                throw new UtilityException((int)Errors.NotFoundParameterInCrmServiceTokenGenerateService, fragment);
            }
            if (!(parameters[CrmServiceTokenGenerateServiceParameterNames.BaseUri] is string))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.ParameterTypeNotMatchInCrmServiceTokenGenerateService,
                    DefaultFormatting = "在Crm服务令牌生成服务{0}中，名称为{1}的参数期望类型为{2}，而实际类型为{3}",
                    ReplaceParameters = new List<object>() { this.GetType().FullName, CrmServiceTokenGenerateServiceParameterNames.BaseUri, typeof(string).FullName, parameters[CrmServiceTokenGenerateServiceParameterNames.BaseUri].GetType().FullName }
                };

                throw new UtilityException((int)Errors.ParameterTypeNotMatchInCrmServiceTokenGenerateService, fragment);
            }
            string strBaseUri = parameters[CrmServiceTokenGenerateServiceParameterNames.BaseUri].ToString();



            if (!parameters.ContainsKey(CrmServiceTokenGenerateServiceParameterNames.ApplicationId) || parameters[CrmServiceTokenGenerateServiceParameterNames.ApplicationId] == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundParameterInCrmServiceTokenGenerateService,
                    DefaultFormatting = "在Crm服务令牌生成服务{0}中，找不到名称为{1}的参数",
                    ReplaceParameters = new List<object>() { this.GetType().FullName, CrmServiceTokenGenerateServiceParameterNames.ApplicationId }
                };

                throw new UtilityException((int)Errors.NotFoundParameterInCrmServiceTokenGenerateService, fragment);
            }
            if (!(parameters[CrmServiceTokenGenerateServiceParameterNames.ApplicationId] is string))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.ParameterTypeNotMatchInCrmServiceTokenGenerateService,
                    DefaultFormatting = "在Crm服务令牌生成服务{0}中，名称为{1}的参数期望类型为{2}，而实际类型为{3}",
                    ReplaceParameters = new List<object>() { this.GetType().FullName, CrmServiceTokenGenerateServiceParameterNames.ApplicationId, typeof(string).FullName, parameters[CrmServiceTokenGenerateServiceParameterNames.ApplicationId].GetType().FullName }
                };

                throw new UtilityException((int)Errors.ParameterTypeNotMatchInCrmServiceTokenGenerateService, fragment);
            }
            string strApplicationId = parameters[CrmServiceTokenGenerateServiceParameterNames.ApplicationId].ToString();

            if (!parameters.ContainsKey(CrmServiceTokenGenerateServiceParameterNames.ApplicationKey) || parameters[CrmServiceTokenGenerateServiceParameterNames.ApplicationKey] == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundParameterInCrmServiceTokenGenerateService,
                    DefaultFormatting = "在Crm服务令牌生成服务{0}中，找不到名称为{1}的参数",
                    ReplaceParameters = new List<object>() { this.GetType().FullName, CrmServiceTokenGenerateServiceParameterNames.ApplicationKey }
                };

                throw new UtilityException((int)Errors.NotFoundParameterInCrmServiceTokenGenerateService, fragment);
            }
            if (!(parameters[CrmServiceTokenGenerateServiceParameterNames.ApplicationKey] is string))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.ParameterTypeNotMatchInCrmServiceTokenGenerateService,
                    DefaultFormatting = "在Crm服务令牌生成服务{0}中，名称为{1}的参数期望类型为{2}，而实际类型为{3}",
                    ReplaceParameters = new List<object>() { this.GetType().FullName, CrmServiceTokenGenerateServiceParameterNames.ApplicationKey, typeof(string).FullName, parameters[CrmServiceTokenGenerateServiceParameterNames.ApplicationKey].GetType().FullName }
                };

                throw new UtilityException((int)Errors.ParameterTypeNotMatchInCrmServiceTokenGenerateService, fragment);
            }
            string strApplcationKey = parameters[CrmServiceTokenGenerateServiceParameterNames.ApplicationKey].ToString();

            if (!parameters.ContainsKey(CrmServiceTokenGenerateServiceParameterNames.CrmUrl) || parameters[CrmServiceTokenGenerateServiceParameterNames.CrmUrl] == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundParameterInCrmServiceTokenGenerateService,
                    DefaultFormatting = "在Crm服务令牌生成服务{0}中，找不到名称为{1}的参数",
                    ReplaceParameters = new List<object>() { this.GetType().FullName, CrmServiceTokenGenerateServiceParameterNames.CrmUrl }
                };

                throw new UtilityException((int)Errors.NotFoundParameterInCrmServiceTokenGenerateService, fragment);
            }
            if (!(parameters[CrmServiceTokenGenerateServiceParameterNames.CrmUrl] is string))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.ParameterTypeNotMatchInCrmServiceTokenGenerateService,
                    DefaultFormatting = "在Crm服务令牌生成服务{0}中，名称为{1}的参数期望类型为{2}，而实际类型为{3}",
                    ReplaceParameters = new List<object>() { this.GetType().FullName, CrmServiceTokenGenerateServiceParameterNames.CrmUrl, typeof(string).FullName, parameters[CrmServiceTokenGenerateServiceParameterNames.CrmUrl].GetType().FullName }
                };

                throw new UtilityException((int)Errors.ParameterTypeNotMatchInCrmServiceTokenGenerateService, fragment);
            }
            string strCrmUrl = parameters[CrmServiceTokenGenerateServiceParameterNames.CrmUrl].ToString();

            if (!parameters.ContainsKey(CrmServiceTokenGenerateServiceParameterNames.AADId))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundParameterInCrmServiceTokenGenerateService,
                    DefaultFormatting = "在Crm服务令牌生成服务{0}中，找不到名称为{1}的参数",
                    ReplaceParameters = new List<object>() { this.GetType().FullName, CrmServiceTokenGenerateServiceParameterNames.AADId }
                };

                throw new UtilityException((int)Errors.NotFoundParameterInCrmServiceTokenGenerateService, fragment);
            }
            if (parameters[CrmServiceTokenGenerateServiceParameterNames.AADId]!=null && !(parameters[CrmServiceTokenGenerateServiceParameterNames.AADId] is string))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.ParameterTypeNotMatchInCrmServiceTokenGenerateService,
                    DefaultFormatting = "在Crm服务令牌生成服务{0}中，名称为{1}的参数期望类型为{2}，而实际类型为{3}",
                    ReplaceParameters = new List<object>() { this.GetType().FullName, CrmServiceTokenGenerateServiceParameterNames.AADId, typeof(string).FullName, parameters[CrmServiceTokenGenerateServiceParameterNames.AADId].GetType().FullName }
                };

                throw new UtilityException((int)Errors.ParameterTypeNotMatchInCrmServiceTokenGenerateService, fragment);
            }
            string strAADId = null;

            if (parameters[CrmServiceTokenGenerateServiceParameterNames.AADId] != null)
            {
                strAADId = parameters[CrmServiceTokenGenerateServiceParameterNames.AADId].ToString();
            }

            string strToken = null;
            await GetAuthenticationContext(strBaseUri,strCrmUrl,strApplicationId, strApplcationKey, strAADId,async(context)=>
            {
                ClientCredential clientcred = new ClientCredential(strApplicationId,strApplcationKey);
                AuthenticationResult authenticationResult = await context.AcquireTokenAsync(strCrmUrl, clientcred);
                strToken = authenticationResult.AccessToken;            
            });


            return $"Bearer {strToken}";
        }


        private string GenerateKeyString(string baseUri,string crmUrl, string applicationId, string applicationkey, string aadId)
        {
            return JsonSerializerHelper.Serializer(new InnerKey(baseUri,crmUrl, applicationId, applicationkey, aadId));
        }

        private async Task<AuthenticationContext> CreateAuthenticationContext(string baseUri,string crmUrl,string aadId)
        {
            AuthenticationContext authenticationContext;
            //如果aadId==null，则需要首先获取认证地址
            if (aadId == null)
            {
                AuthenticationParameters authenticationParameters = await AuthenticationParameters.CreateFromUrlAsync(new Uri(new Uri(crmUrl), "api/data/"));
                authenticationContext = new AuthenticationContext(authenticationParameters.Authority.Replace("/oauth2/authorize", string.Empty));
            }
            else
            {
                authenticationContext =new AuthenticationContext($"https://{baseUri}/{aadId}");
            }

            return authenticationContext;
        }

        private async Task GetAuthenticationContext(string baseUri,string crmUrl, string applicationId, string applicationkey, string aadId, Func<AuthenticationContext, Task> action)
        {
            var strKey = GenerateKeyString(baseUri, crmUrl, applicationId, applicationkey, aadId);
            if (!_authenticationContexts.TryGetValue(strKey, out AuthenticationContextContainer contextContainer))
            {

                SharePool<AuthenticationContext> contextPool = new SharePool<AuthenticationContext>("CrmS2S",
                   null,
                (authcContext) =>
                {
                    return true;
                }
                ,
                (authcContext) =>
                {

                },
                async () =>
                {
                    AuthenticationContext newContext = await CreateAuthenticationContext(baseUri,crmUrl, aadId);
                    return await Task.FromResult(newContext);
                }
                ,
                async (authcContext) =>
                {
                    return await Task.FromResult(true);
                }
                ,
                async (authcContext) =>
                {
                    await Task.FromResult(0);
                }
                ,
                _poolLimit
                );

                contextContainer = new AuthenticationContextContainer() { ContextPool = contextPool, LastTime = DateTime.UtcNow };



                lock (_authenticationContexts)
                {
                    if (_authenticationContexts.Count > _limit)
                    {
                        var deleteItem = (from item in _authenticationContexts
                                          orderby item.Value.LastTime
                                          select item
                                          ).FirstOrDefault();
                        if (deleteItem.Key != null)
                        {
                            _authenticationContexts.Remove(deleteItem.Key);
                        }
                    }

                    _authenticationContexts[strKey] = contextContainer;
                }


            }

            AuthenticationContext context = null;

            context = await contextContainer.ContextPool.GetAsync();
            await action(context);
        }

        [DataContract]
        private class InnerKey
        {
            public InnerKey(string baseUri, string crmUrl,string applicationId,string applicationKey,string aadId)
            {
                BaseUrl = baseUri;
                CrmUrl = crmUrl;
                ApplicationId = applicationId;
                ApplicationKey = applicationKey;
                AADId = aadId;
            }
            [DataMember]
            public string BaseUrl { get; private set; }

            [DataMember]
            public string CrmUrl { get; private set; }

            [DataMember]
            public string ApplicationId { get; private set; }
            [DataMember]
            public string ApplicationKey { get; private set; }
            [DataMember]
            public string AADId { get; private set; }
        }

        private class AuthenticationContextContainer
        {
            public SharePool<AuthenticationContext> ContextPool { get; set; }
            public DateTime LastTime { get; set; }
        }

    }
}
