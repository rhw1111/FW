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
using MSLibrary.Oauth.ADFS;

namespace MSLibrary.Xrm.Token
{
    /// <summary>
    /// 针对ADFS模式的令牌生成
    /// 需要传入的参数：
    /// AdfsUrl:string,ADFS的地址
    /// CrmUrl:string,Crm的地址
    /// ClientId:string,在ADFS中注册的ClientId
    /// RedirectUri:string,在ADFS中注册的RedirectUri
    /// UserName:string,用户名
    /// Password:string,密码
    /// </summary>
    [Injection(InterfaceType = typeof(CrmServiceTokenGenerateServiceForADFS), Scope = InjectionScope.Singleton)]
    public class CrmServiceTokenGenerateServiceForADFS : ICrmServiceTokenGenerateService
    {
        private static Dictionary<string, AdfsAuthWrapperContainer> _adfsAuthContexts = new Dictionary<string, AdfsAuthWrapperContainer>();

        private static int _limit = 10000;

        private static int _poolLimit = 100;

        private static int _refreashTokenTimeout = 3 * 60 * 60;
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
        /// <summary>
        /// RefreashToken的超时时间（秒）
        /// </summary>
        public static int RefreashTokenTimeout
        {
            set
            {
                _refreashTokenTimeout = value;
            }
        }

        public async Task<string> Genereate(Dictionary<string, object> parameters)
        {
            TextFragment fragment;
            //检查参数
            if (!parameters.ContainsKey(CrmServiceTokenGenerateServiceParameterNames.AdfsUrl) || parameters[CrmServiceTokenGenerateServiceParameterNames.AdfsUrl] == null)
            {
                fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundParameterInCrmServiceTokenGenerateService,
                    DefaultFormatting = "在Crm服务令牌生成服务{0}中，找不到名称为{1}的参数",
                    ReplaceParameters = new List<object>() { this.GetType().FullName, CrmServiceTokenGenerateServiceParameterNames.AdfsUrl }
                };

                throw new UtilityException((int)Errors.NotFoundParameterInCrmServiceTokenGenerateService, fragment);
            }
            if (!(parameters[CrmServiceTokenGenerateServiceParameterNames.AdfsUrl] is string))
            {
                fragment = new TextFragment()
                {
                    Code = TextCodes.ParameterTypeNotMatchInCrmServiceTokenGenerateService,
                    DefaultFormatting = "在Crm服务令牌生成服务{0}中，名称为{1}的参数期望类型为{2}，而实际类型为{3}",
                    ReplaceParameters = new List<object>() { this.GetType().FullName, CrmServiceTokenGenerateServiceParameterNames.AdfsUrl, typeof(string).FullName, parameters[CrmServiceTokenGenerateServiceParameterNames.AdfsUrl].GetType().FullName }
                };

                throw new UtilityException((int)Errors.ParameterTypeNotMatchInCrmServiceTokenGenerateService, fragment);
            }
            string strAdfsUrl = parameters[CrmServiceTokenGenerateServiceParameterNames.AdfsUrl].ToString();

            if (!parameters.ContainsKey(CrmServiceTokenGenerateServiceParameterNames.ClientId) || parameters[CrmServiceTokenGenerateServiceParameterNames.ClientId] == null)
            {
                fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundParameterInCrmServiceTokenGenerateService,
                    DefaultFormatting = "在Crm服务令牌生成服务{0}中，找不到名称为{1}的参数",
                    ReplaceParameters = new List<object>() { this.GetType().FullName, CrmServiceTokenGenerateServiceParameterNames.ClientId }
                };

                throw new UtilityException((int)Errors.NotFoundParameterInCrmServiceTokenGenerateService, fragment);
            }
            if (!(parameters[CrmServiceTokenGenerateServiceParameterNames.ClientId] is string))
            {
                fragment = new TextFragment()
                {
                    Code = TextCodes.ParameterTypeNotMatchInCrmServiceTokenGenerateService,
                    DefaultFormatting = "在Crm服务令牌生成服务{0}中，名称为{1}的参数期望类型为{2}，而实际类型为{3}",
                    ReplaceParameters = new List<object>() { this.GetType().FullName, CrmServiceTokenGenerateServiceParameterNames.ClientId, typeof(string).FullName, parameters[CrmServiceTokenGenerateServiceParameterNames.ClientId].GetType().FullName }
                };

                throw new UtilityException((int)Errors.ParameterTypeNotMatchInCrmServiceTokenGenerateService, fragment);
            }
            string strClientId = parameters[CrmServiceTokenGenerateServiceParameterNames.ClientId].ToString();

            if (!parameters.ContainsKey(CrmServiceTokenGenerateServiceParameterNames.CrmUrl) || parameters[CrmServiceTokenGenerateServiceParameterNames.CrmUrl] == null)
            {
                fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundParameterInCrmServiceTokenGenerateService,
                    DefaultFormatting = "在Crm服务令牌生成服务{0}中，找不到名称为{1}的参数",
                    ReplaceParameters = new List<object>() { this.GetType().FullName, CrmServiceTokenGenerateServiceParameterNames.CrmUrl }
                };

                throw new UtilityException((int)Errors.NotFoundParameterInCrmServiceTokenGenerateService, fragment);
            }
            if (!(parameters[CrmServiceTokenGenerateServiceParameterNames.CrmUrl] is string))
            {
                fragment = new TextFragment()
                {
                    Code = TextCodes.ParameterTypeNotMatchInCrmServiceTokenGenerateService,
                    DefaultFormatting = "在Crm服务令牌生成服务{0}中，名称为{1}的参数期望类型为{2}，而实际类型为{3}",
                    ReplaceParameters = new List<object>() { this.GetType().FullName, CrmServiceTokenGenerateServiceParameterNames.CrmUrl, typeof(string).FullName, parameters[CrmServiceTokenGenerateServiceParameterNames.CrmUrl].GetType().FullName }
                };

                throw new UtilityException((int)Errors.ParameterTypeNotMatchInCrmServiceTokenGenerateService,fragment);
            }
            string strCrmUrl = parameters[CrmServiceTokenGenerateServiceParameterNames.CrmUrl].ToString();

            if (!parameters.ContainsKey(CrmServiceTokenGenerateServiceParameterNames.RedirectUri) || parameters[CrmServiceTokenGenerateServiceParameterNames.RedirectUri] == null)
            {
                fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundParameterInCrmServiceTokenGenerateService,
                    DefaultFormatting = "在Crm服务令牌生成服务{0}中，找不到名称为{1}的参数",
                    ReplaceParameters = new List<object>() { this.GetType().FullName, CrmServiceTokenGenerateServiceParameterNames.RedirectUri }
                };

                throw new UtilityException((int)Errors.NotFoundParameterInCrmServiceTokenGenerateService, fragment);
            }
            if (!(parameters[CrmServiceTokenGenerateServiceParameterNames.RedirectUri] is string))
            {
                fragment = new TextFragment()
                {
                    Code = TextCodes.ParameterTypeNotMatchInCrmServiceTokenGenerateService,
                    DefaultFormatting = "在Crm服务令牌生成服务{0}中，名称为{1}的参数期望类型为{2}，而实际类型为{3}",
                    ReplaceParameters = new List<object>() { this.GetType().FullName, CrmServiceTokenGenerateServiceParameterNames.RedirectUri, typeof(string).FullName, parameters[CrmServiceTokenGenerateServiceParameterNames.RedirectUri].GetType().FullName }
                };

                throw new UtilityException((int)Errors.ParameterTypeNotMatchInCrmServiceTokenGenerateService, fragment);
            }
            string strRedirectUri = parameters[CrmServiceTokenGenerateServiceParameterNames.RedirectUri].ToString();

            if (!parameters.ContainsKey(CrmServiceTokenGenerateServiceParameterNames.UserName) || parameters[CrmServiceTokenGenerateServiceParameterNames.UserName] == null)
            {
                fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundParameterInCrmServiceTokenGenerateService,
                    DefaultFormatting = "在Crm服务令牌生成服务{0}中，找不到名称为{1}的参数",
                    ReplaceParameters = new List<object>() { this.GetType().FullName, CrmServiceTokenGenerateServiceParameterNames.UserName }
                };

                throw new UtilityException((int)Errors.NotFoundParameterInCrmServiceTokenGenerateService, fragment);
            }
            if (!(parameters[CrmServiceTokenGenerateServiceParameterNames.UserName] is string))
            {
                fragment = new TextFragment()
                {
                    Code = TextCodes.ParameterTypeNotMatchInCrmServiceTokenGenerateService,
                    DefaultFormatting = "在Crm服务令牌生成服务{0}中，名称为{1}的参数期望类型为{2}，而实际类型为{3}",
                    ReplaceParameters = new List<object>() { this.GetType().FullName, CrmServiceTokenGenerateServiceParameterNames.UserName, typeof(string).FullName, parameters[CrmServiceTokenGenerateServiceParameterNames.UserName].GetType().FullName }
                };

                throw new UtilityException((int)Errors.ParameterTypeNotMatchInCrmServiceTokenGenerateService,fragment);
            }
            string strUserName = parameters[CrmServiceTokenGenerateServiceParameterNames.UserName].ToString();

            if (!parameters.ContainsKey(CrmServiceTokenGenerateServiceParameterNames.Password) || parameters[CrmServiceTokenGenerateServiceParameterNames.Password] == null)
            {
                fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundParameterInCrmServiceTokenGenerateService,
                    DefaultFormatting = "在Crm服务令牌生成服务{0}中，找不到名称为{1}的参数",
                    ReplaceParameters = new List<object>() { this.GetType().FullName, CrmServiceTokenGenerateServiceParameterNames.Password }
                };

                throw new UtilityException((int)Errors.NotFoundParameterInCrmServiceTokenGenerateService,fragment);
            }
            if (!(parameters[CrmServiceTokenGenerateServiceParameterNames.Password] is string))
            {
                fragment = new TextFragment()
                {
                    Code = TextCodes.ParameterTypeNotMatchInCrmServiceTokenGenerateService,
                    DefaultFormatting = "在Crm服务令牌生成服务{0}中，名称为{1}的参数期望类型为{2}，而实际类型为{3}",
                    ReplaceParameters = new List<object>() { this.GetType().FullName, CrmServiceTokenGenerateServiceParameterNames.Password, typeof(string).FullName, parameters[CrmServiceTokenGenerateServiceParameterNames.Password].GetType().FullName }
                };

                throw new UtilityException((int)Errors.ParameterTypeNotMatchInCrmServiceTokenGenerateService, fragment);
            }
            string strPassword = parameters[CrmServiceTokenGenerateServiceParameterNames.Password].ToString();

            string strToken = null;
            await GetAdfsAuth(strAdfsUrl, strClientId, strRedirectUri, strCrmUrl, strUserName, strPassword, async (adfsAuth) =>
                  {
                      strToken = adfsAuth.AccessToken;
                      await Task.FromResult(0);
                  });

            return $"Bearer {strToken}";
        }


        private string GenerateKeyString(string adfsUrl,string clientId,string redirectUri, string crmUrl, string userName, string password)
        {
            return JsonSerializerHelper.Serializer(new AdfsParameter() { AdfsUrl=adfsUrl, ClientId=clientId, CrmUrl=crmUrl,  RedirectUri=redirectUri, UserName=userName, Password=password });
        }

        private async Task GetAdfsAuth(string adfsUrl, string clientId, string redirectUri, string crmUrl, string userName, string password, Func<AdfsAuth, Task> action)
        {
            var strKey = GenerateKeyString(adfsUrl, clientId, redirectUri, crmUrl, userName, password);
            if (!_adfsAuthContexts.TryGetValue(strKey, out AdfsAuthWrapperContainer wrapperContainer))
            {
                SharePool<AdfsAuthWrapper> wrapperPool = new SharePool<AdfsAuthWrapper>("Adfs", () =>
                {
                    return null;
                },
                (wrapper) =>
                {
                    return true;
                }
                ,
                (wrapper) =>
                {
                }
                ,
                async () =>
                {
                    var auth = await AdfsHelper.GetAdfsAuth(adfsUrl, $"{crmUrl}/api/data", clientId, redirectUri, userName, password);
                    AdfsAuthWrapper wrapper = new AdfsAuthWrapper()
                    {
                        AdfsAuth = auth,
                        AdfsParameter = new AdfsParameter() { AdfsUrl = adfsUrl, ClientId = clientId, CrmUrl = crmUrl, RedirectUri = redirectUri, UserName = userName, Password = password },
                        CreateTime = DateTime.UtcNow,
                        TokenCreateTime = DateTime.UtcNow
                    };
                    return wrapper;
                }
                ,
                async (wrapper) =>
                {
                    if ((DateTime.UtcNow - wrapper.CreateTime).TotalSeconds > _refreashTokenTimeout - 100)
                    {
                        return await Task.FromResult(false);
                    }
                    return await Task.FromResult(true);
                }
                ,
                async (wrapper) =>
                {
                    await Task.FromResult(0);
                },
                _poolLimit
                );

                wrapperContainer = new AdfsAuthWrapperContainer() { ContextPool = wrapperPool, LastTime = DateTime.UtcNow };



                lock (_adfsAuthContexts)
                {
                    if (_adfsAuthContexts.Count > _limit)
                    {

                        var deleteItem = (from item in _adfsAuthContexts
                                          orderby item.Value.LastTime
                                          select item
                                          ).FirstOrDefault();
                        if (deleteItem.Key != null)
                        {
                            _adfsAuthContexts.Remove(deleteItem.Key);
                        }

                    }
                    _adfsAuthContexts[strKey] = wrapperContainer;
                }


            }

            AdfsAuthWrapper adfsAuthWrapper = null;

            AdfsAuth adfsAuth = null;
            adfsAuthWrapper = await wrapperContainer.ContextPool.GetAsync();
            if ((DateTime.UtcNow - adfsAuthWrapper.TokenCreateTime).TotalSeconds > adfsAuthWrapper.AdfsAuth.Expires - 20)
            {
                adfsAuth = await AdfsHelper.RefreshToken(adfsAuthWrapper.AdfsParameter.AdfsUrl, adfsAuthWrapper.AdfsAuth.RefreshToken);
                adfsAuthWrapper.AdfsAuth = adfsAuth;
                adfsAuthWrapper.TokenCreateTime = DateTime.UtcNow;

            }
            else
            {
                adfsAuth = adfsAuthWrapper.AdfsAuth;
            }

            await action(adfsAuth);


        }


        [DataContract]
        public class AdfsParameter
        {
            [DataMember]
            public string AdfsUrl { get; set; }
            [DataMember]
            public string CrmUrl { get; set; }
            [DataMember]
            public string ClientId { get; set; }
            [DataMember]
            public string RedirectUri { get; set; }
            [DataMember]
            public string UserName { get; set;}
            [DataMember]
            public string Password { get; set; }
        }



        private class AdfsAuthWrapper
        {
            public AdfsAuth AdfsAuth { get; set; }
            public AdfsParameter AdfsParameter { get; set; }
            public DateTime TokenCreateTime { get; set; }

            public DateTime CreateTime { get; set; }
        }


        private class AdfsAuthWrapperContainer
        {
            public SharePool<AdfsAuthWrapper> ContextPool { get; set; }
            public DateTime LastTime { get; set; }
        }
    }



}
