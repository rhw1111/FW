using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Http;
using MSLibrary.DI;
using MSLibrary.SystemToken.DAL;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.SystemToken
{
    /// <summary>
    /// 验证终结点
    /// 负责与最终验证方交互，获取系统令牌
    /// </summary>
    public class AuthorizationEndpoint : EntityBase<IAuthorizationEndpointIMP>
    {
        private static IFactory<IAuthorizationEndpointIMP> _authorizationEndpointIMPFactory;

        public static IFactory<IAuthorizationEndpointIMP> AuthorizationEndpointIMPFactory
        {
            set
            {
                _authorizationEndpointIMPFactory = value;
            }
        }

        public override IFactory<IAuthorizationEndpointIMP> GetIMPFactory()
        {
            return _authorizationEndpointIMPFactory;
        }
        /// <summary>
        /// ID
        /// </summary>
        public Guid ID
        {
            get
            {
                return GetAttribute<Guid>("ID");
            }
            set
            {
                SetAttribute<Guid>("ID", value);
            }
        }


        /// <summary>
        /// 验证系统名称
        /// </summary>
        public string Name
        {
            get
            {
                return GetAttribute<string>("Name");
            }
            set
            {
                SetAttribute<string>("Name", value);
            }
        }

        /// <summary>
        /// 第三方系统类型
        /// </summary>
        public string ThirdPartyType
        {
            get
            {
                return GetAttribute<string>("ThirdPartyType");
            }
            set
            {
                SetAttribute<string>("ThirdPartyType", value);
            }
        }
        /// <summary>
        /// 第三方系统后续处理类型
        /// </summary>
        public string ThirdPartyPostExecuteType
        {
            get
            {
                return GetAttribute<string>("ThirdPartyPostExecuteType");
            }
            set
            {
                SetAttribute<string>("ThirdPartyPostExecuteType", value);
            }
        }


        /// <summary>
        /// 针对第三方系统的配置信息的配置信息
        /// </summary>
        public string ThirdPartyConfiguration
        {
            get
            {
                return GetAttribute<string>("ThirdPartyConfiguration");
            }
            set
            {
                SetAttribute<string>("ThirdPartyConfiguration", value);
            }
        }

        /// <summary>
        /// 针对第三方系统后续处理的配置信息的配置信息
        /// </summary>
        public string ThirdPartyPostConfiguration
        {
            get
            {
                return GetAttribute<string>("ThirdPartyPostConfiguration");
            }
            set
            {
                SetAttribute<string>("ThirdPartyPostConfiguration", value);
            }
        }

        /// <summary>
        /// 是否保持第三方令牌
        /// </summary>
        public bool KeepThirdPartyToken
        {
            get
            {
                return GetAttribute<bool>("KeepThirdPartyToken");
            }
            set
            {
                SetAttribute<bool>("KeepThirdPartyToken", value);
            }
        }

        /// <summary>
        /// 验证模式
        /// 目前1：Url跳转模式，2：用户名密码模式
        /// 多个以,隔开
        /// </summary>
        public int[] AuthModes
        {
            get
            {
                return GetAttribute<int[]>("AuthModes");
            }
            set
            {
                SetAttribute<int[]>("AuthModes", value);
            }
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime
        {
            get
            {
                return GetAttribute<DateTime>("CreateTime");
            }
            set
            {
                SetAttribute<DateTime>("CreateTime", value);
            }
        }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifyTime
        {
            get
            {
                return GetAttribute<DateTime>("ModifyTime");
            }
            set
            {
                SetAttribute<DateTime>("ModifyTime", value);
            }
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <returns></returns>
        public async Task Add()
        {
            await _imp.Add(this);
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        public async Task Update()
        {
            await _imp.Update(this);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public async Task Delete(AuthorizationEndpoint endpoint)
        {
            await _imp.Delete(this);
        }


        /// <summary>
        /// 根据配置信息和客户端重定向地址获取对应验证终结点登录地址
        /// </summary>
        /// <param name="systemLoginRedirectUrl">验证中心的重定向地址</param>
        /// <param name="clientRedirectUrl">客户端重定向地址</param>
        /// <returns>登陆地址</returns>
        public async Task<string> GetLoginUrl(string systemLoginRedirectUrl, string clientRedirectUrl)
        {
            return await _imp.GetLoginUrl(this, systemLoginRedirectUrl,clientRedirectUrl);
        }
        /// <summary>
        /// 获取第三方令牌键值对
        /// </summary>
        /// <param name="systemLoginRedirectUrl">验证中心的重定向地址</param>
        /// <param name="clientRedirectUrl">客户端重定向地址</param>
        /// <returns>获取系统令牌动作的结果</returns>
        public async Task<GetSystemTokenResult> GetSystemToken(SystemLoginEndpoint loginEndpoint,string systemLoginRedirectUrl, string clientRedirectUrl)
        {
            return await _imp.GetSystemToken(this,loginEndpoint, systemLoginRedirectUrl, clientRedirectUrl);
        }

        /// <summary>
        /// 验证第三方令牌是否合法
        /// </summary>
        /// <param name="systemToken">要验证的系统令牌</param>
        /// <returns>是否合法</returns>
        public async Task<bool> VerifyToken(string systemToken)
        {
            return await _imp.VerifyToken(this, systemToken);
        }

        /// <summary>
        /// 获取第三方系统的注销地址
        /// </summary>
        /// <param name="systemToken">第三方令牌</param>
        /// <returns>注销地址</returns>
        public async Task<string> GetLogoutUrl(string systemToken)
        {
            return await _imp.GetLogoutUrl(this, systemToken);
        }
        /// <summary>
        /// 获取针对第三方登陆系统回调的处理后获得的令牌结果
        /// </summary>
        /// <param name="request">回调请求</param>
        /// <returns></returns>
        public async Task<ThirdPartySystemTokenResult> GetSystemAttributes(SystemLoginEndpoint loginEndpoint,HttpRequest request)
        {
            return await _imp.GetSystemAttributes(this,loginEndpoint, request);
        }

        /// <summary>
        /// 根据第三方令牌获得通信令牌
        /// </summary>
        /// <param name="systemToken">第三方令牌</param>
        /// <returns></returns>
        public async Task<string> GetCommunicationToken(string systemToken)
        {
            return await _imp.GetCommunicationToken(this, systemToken);
        }

        /// <summary>
        /// 获取第三方保持登陆状态的URL
        /// </summary>
        /// <param name="systemToken">第三方令牌</param>
        /// <returns></returns>
        public async Task<string> GetKeepLoginUrl(string systemToken)
        {
            return await _imp.GetKeepLoginUrl(this, systemToken);
        }
        /// <summary>
        /// 刷新第三方令牌
        /// </summary>
        /// <param name="systemToken"></param>
        /// <returns></returns>
        public async Task<string> RefreshToken(SystemLoginEndpoint loginEndpoint, string userKey, string systemToken)
        {
            return await _imp.RefreshToken(this,loginEndpoint,userKey,systemToken);
        }
        /// <summary>
        /// 使用系统令牌对第三方验证系统执行登出操作
        /// </summary>
        /// <param name="systemToken"></param>
        /// <returns></returns>
        public async Task Logout(string systemToken)
        {
            await _imp.Logout(this, systemToken);
        }

        /// <summary>
        /// 从回调请求中获取实际重定向地址
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<string> GetRealRedirectUrl(HttpRequest request)
        {
            return await _imp.GetRealRedirectUrl(this, request);
        }

        /// <summary>
        /// 根据用户名密码获取第三方令牌键值对
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<ThirdPartySystemTokenResult> GetSystemTokenByPassword(SystemLoginEndpoint loginEndpoint,string userName, string password)
        {
            return await _imp.GetSystemTokenByPassword(this,loginEndpoint, userName, password);
        }
    }


    public interface IAuthorizationEndpointIMP
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        Task Add(AuthorizationEndpoint endpoint);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        Task Update(AuthorizationEndpoint endpoint);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        Task Delete(AuthorizationEndpoint endpoint);
        /// <summary>
        /// 根据配置信息和客户端重定向地址获取对应验证终结点登录地址
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="systemLoginRedirectUrl">验证中心的重定向地址</param>
        /// <param name="clientRedirectUrl">客户端重定向地址</param>
        /// <returns>登陆地址</returns>
        Task<string> GetLoginUrl(AuthorizationEndpoint endpoint,string systemLoginRedirectUrl,string clientRedirectUrl);
        /// <summary>
        /// 获取第三方系统令牌结果
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="systemLoginRedirectUrl">验证中心的重定向地址</param>
        /// <param name="clientRedirectUrl">客户端重定向地址</param>
        /// <returns>系统令牌结果</returns>
        Task<GetSystemTokenResult> GetSystemToken(AuthorizationEndpoint endpoint, SystemLoginEndpoint loginEndpoint, string systemLoginRedirectUrl, string clientRedirectUrl);

        /// <summary>
        /// 获取针对第三方登陆系统回调处理后获取的键值对
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="request">回调请求</param>
        /// <returns></returns>
        Task<ThirdPartySystemTokenResult> GetSystemAttributes(AuthorizationEndpoint endpoint, SystemLoginEndpoint loginEndpoint, HttpRequest request);

        /// <summary>
        /// 验证第三方令牌是否合法
        /// 这里的系统令牌指的是第三方验证方给的令牌
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="systemToken">要验证的系统令牌</param>
        /// <returns>是否合法</returns>
        Task<bool> VerifyToken(AuthorizationEndpoint endpoint, string systemToken);
        /// <summary>
        /// 获取第三方系统的注销地址
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="systemToken">第三方令牌</param>
        /// <returns>注销地址</returns>
        Task<string> GetLogoutUrl(AuthorizationEndpoint endpoint, string systemToken);

        /// <summary>
        /// 获取第三方系统保持登录的地址
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="systemToken">第三方令牌</param>
        /// <returns></returns>
        Task<string> GetKeepLoginUrl(AuthorizationEndpoint endpoint,string systemToken);
        /// <summary>
        /// 从系统令牌中获取实际通信需要的令牌
        /// 有些应用场景例如Oauth2.0中系统令牌可能需要保存AccessToken和RefreashToken，
        /// 而实际用来与第三方验证系统通信的是AccessToken，所以需要从系统令牌中分解出通信令牌
        /// 如果系统令牌本身只有一个，那直接返回即可
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="systemToken">系统令牌</param>
        /// <returns></returns>
        Task<string> GetCommunicationToken(AuthorizationEndpoint endpoint,  string systemToken);

        /// <summary>
        /// 刷新系统令牌
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="token">要刷新的系统令牌</param>
        /// <returns></returns>
        Task<string> RefreshToken(AuthorizationEndpoint endpoint, SystemLoginEndpoint loginEndpoint, string userKey, string systemToken);
        /// <summary>
        /// 使用系统令牌对第三方验证系统执行登出操作
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="systemToken"></param>
        /// <returns></returns>
        Task Logout(AuthorizationEndpoint endpoint, string systemToken);

        /// <summary>
        /// 从回调请求中获取实际重定向地址
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<string> GetRealRedirectUrl(AuthorizationEndpoint endpoint,HttpRequest request);
        /// <summary>
        /// 根据用户名密码获取令牌键值对
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<ThirdPartySystemTokenResult> GetSystemTokenByPassword(AuthorizationEndpoint endpoint, SystemLoginEndpoint loginEndpoint, string userName,string password);
    }

    [Injection(InterfaceType = typeof(IAuthorizationEndpointIMP), Scope = InjectionScope.Transient)]
    public class AuthorizationEndpointIMP : IAuthorizationEndpointIMP
    {
        private static Dictionary<string, IThirdPartySystemService> _thirdPartySystemServices = new Dictionary<string, IThirdPartySystemService>();

        public static Dictionary<string, IThirdPartySystemService> ThirdPartySystemServices
        {
            get
            {
                return _thirdPartySystemServices;
            }
        }

        private static Dictionary<string, IThirdPartySystemPostExecuteService> _thirdPartySystemPostExecuteServices = new Dictionary<string, IThirdPartySystemPostExecuteService>();

        public static Dictionary<string, IThirdPartySystemPostExecuteService> ThirdPartySystemPostExecuteServices
        {
            get
            {
                return _thirdPartySystemPostExecuteServices;
            }
        }



        private IAuthorizationEndpointStore _authorizationEndpointStore;
        private IThirdPartySystemTokenRecordStore _thirdPartySystemTokenRecordStore;

        public AuthorizationEndpointIMP(IAuthorizationEndpointStore authorizationEndpointStore, IThirdPartySystemTokenRecordStore thirdPartySystemTokenRecordStore)
        {
            _authorizationEndpointStore = authorizationEndpointStore;
            _thirdPartySystemTokenRecordStore = thirdPartySystemTokenRecordStore;
        }

        public async Task Add(AuthorizationEndpoint endpoint)
        {
            await _authorizationEndpointStore.Add(endpoint);
        }

        public async Task Delete(AuthorizationEndpoint endpoint)
        {
            await _authorizationEndpointStore.Delete(endpoint.ID);
        }

        public async Task<string> GetCommunicationToken(AuthorizationEndpoint endpoint,string systemToken)
        {
            var service= await GetService(endpoint);
            return await service.GetCommunicationToken(endpoint.ThirdPartyConfiguration, systemToken);
        }


        public async Task<string> GetLoginUrl(AuthorizationEndpoint endpoint, string systemLoginRedirectUrl, string clientRedirectUrl)
        {
            var service = await GetService(endpoint);
            return await service.GetLoginUrl(endpoint.ThirdPartyConfiguration, systemLoginRedirectUrl,clientRedirectUrl);
        }

        public async Task<string> GetLogoutUrl(AuthorizationEndpoint endpoint, string systemToken)
        {
            var service = await GetService(endpoint);
            return await service.GetLogoutUrl(endpoint.ThirdPartyConfiguration, systemToken);
        }

        public async Task<string> GetKeepLoginUrl(AuthorizationEndpoint endpoint, string systemToken)
        {
            var service = await GetService(endpoint);
            return await service.GetKeepLoginUrl(endpoint.ThirdPartyConfiguration, systemToken);
        }

        public async  Task<GetSystemTokenResult> GetSystemToken(AuthorizationEndpoint endpoint, SystemLoginEndpoint loginEndpoint, string systemLoginRedirectUrl, string clientRedirectUrl)
        {
            GetSystemTokenResult result = new GetSystemTokenResult();
            var service = await GetService(endpoint);      
            var postExecuteService = await GetPostExecuteService(endpoint);
                       
            //获取第三方系统的令牌结果
            var getSystemTokenResult=await service.GetSystemToken(endpoint.ThirdPartyConfiguration, systemLoginRedirectUrl, clientRedirectUrl);

            result.Direct = getSystemTokenResult.Direct;
            
            
            //如果不是结果是直接，则需要进行后续处理
            if (getSystemTokenResult.Direct)
            {
                result.TokenResult = new ThirdPartySystemTokenResult()
                {
                    Attributes = getSystemTokenResult.Token.Attributes
                };

                if (postExecuteService != null)
                {
                    var postExecuteResult = await postExecuteService.Execute(getSystemTokenResult.Token.Attributes, endpoint.ThirdPartyPostConfiguration);

                    result.TokenResult.Attributes.Merge(postExecuteResult.UserInfoAttributes);
                    result.TokenResult.AdditionalRedirectUrlQueryAttributes = postExecuteResult.AdditionalRedirectUrlQueryAttributes;
                }


                await SaveTokenRecord(getSystemTokenResult.Token.Attributes, getSystemTokenResult.Token.Token, endpoint, loginEndpoint);
            }
            else
            {
                result.RedirectUrl = getSystemTokenResult.RedirectUrl;
            }


            return result;
        }

        public async Task<ThirdPartySystemTokenResult> GetSystemAttributes(AuthorizationEndpoint endpoint,SystemLoginEndpoint loginEndpoint, HttpRequest request)
        {
            ThirdPartySystemTokenResult result = new ThirdPartySystemTokenResult();
          
            var service = await GetService(endpoint);
            var postExecuteService = await GetPostExecuteService(endpoint);


            var thirdPartyResult = await service.GetSystemToken(endpoint.ThirdPartyConfiguration,request);
            result.Attributes = thirdPartyResult.Attributes;

            if (postExecuteService!=null)
            {
                var postExecuteResult = await postExecuteService.Execute(thirdPartyResult.Attributes, endpoint.ThirdPartyPostConfiguration);
                result.Attributes.Merge(postExecuteResult.UserInfoAttributes);
                result.AdditionalRedirectUrlQueryAttributes = postExecuteResult.AdditionalRedirectUrlQueryAttributes;
            }

            await SaveTokenRecord(thirdPartyResult.Attributes, thirdPartyResult.Token, endpoint, loginEndpoint);

            return result;
        }

        public async Task Logout(AuthorizationEndpoint endpoint, string systemToken)
        {
            var service = await GetService(endpoint);
            
            await service.Logout(endpoint.ThirdPartyConfiguration, systemToken);
        }

        public async Task<string> RefreshToken(AuthorizationEndpoint endpoint,SystemLoginEndpoint loginEndpoint,string userKey, string systemToken)
        {
            var service = await GetService(endpoint);

            var newToken= await service.RefreshToken(endpoint.ThirdPartyConfiguration, systemToken);

            if (endpoint.KeepThirdPartyToken)
            {
                var record = await _thirdPartySystemTokenRecordStore.QueryByUserKey(userKey, loginEndpoint.ID, endpoint.ID);
                if (record != null)
                {
                    await _thirdPartySystemTokenRecordStore.UpdateToken(userKey, record.ID, newToken);
                }
            }

            return newToken;
        }

        public async Task Update(AuthorizationEndpoint endpoint)
        {
            await _authorizationEndpointStore.Update(endpoint);
        }

        public async Task<bool> VerifyToken(AuthorizationEndpoint endpoint, string systemToken)
        {
            var service = await GetService(endpoint);
            return await service.VerifyToken(endpoint.ThirdPartyConfiguration, systemToken);
        }


        private async Task<IThirdPartySystemService> GetService(AuthorizationEndpoint endpoint)
        {
            if (!_thirdPartySystemServices.TryGetValue(endpoint.ThirdPartyType, out IThirdPartySystemService service))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundThirdPartySystemServiceByType,
                    DefaultFormatting = "找不到类型为{0}的第三方系统服务，发生位置：{1}",
                    ReplaceParameters = new List<object>() { endpoint.ThirdPartyType, $"{typeof(AuthorizationEndpointIMP).FullName}.ThirdPartySystemServices" }
                };

                throw new UtilityException((int)Errors.NotFoundThirdPartySystemServiceByType, fragment);
            }

            return await Task.FromResult(service);
        }



        private async Task<IThirdPartySystemPostExecuteService> GetPostExecuteService(AuthorizationEndpoint endpoint)
        {
            if (string.IsNullOrEmpty(endpoint.ThirdPartyPostExecuteType))
            {
                return null;
            }
            if (!_thirdPartySystemPostExecuteServices.TryGetValue(endpoint.ThirdPartyPostExecuteType, out IThirdPartySystemPostExecuteService service))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundThirdPartySystemPostExecuteServiceByType,
                    DefaultFormatting = "找不到类型为{0}的第三方系统后续处理服务，发生位置：{1}",
                    ReplaceParameters = new List<object>() { endpoint.ThirdPartyPostExecuteType, $"{typeof(AuthorizationEndpointIMP).FullName}.ThirdPartySystemPostExecuteServices" }
                };

                throw new UtilityException((int)Errors.NotFoundThirdPartySystemPostExecuteServiceByType, fragment);
            }

            return await Task.FromResult(service);
        }


        public async Task<string> GetRealRedirectUrl(AuthorizationEndpoint endpoint, HttpRequest request)
        {
            var service = await GetService(endpoint);
            return await service.GetRealRedirectUrl(endpoint.ThirdPartyConfiguration, request);
        }

        public async Task<ThirdPartySystemTokenResult> GetSystemTokenByPassword(AuthorizationEndpoint endpoint, SystemLoginEndpoint loginEndpoint, string userName, string password)
        {
            ThirdPartySystemTokenResult result = new ThirdPartySystemTokenResult();
            var service = await GetService(endpoint);
            var postExecuteService = await GetPostExecuteService(endpoint);

            var thirdPartyResult = await service.GetSystemTokenByPassword(endpoint.ThirdPartyConfiguration, userName, password);

            result.Attributes = thirdPartyResult.Attributes;

            if (postExecuteService!=null)
            {
                var postExecuteResult = await postExecuteService.Execute(thirdPartyResult.Attributes, endpoint.ThirdPartyPostConfiguration);
                result.Attributes.Merge(postExecuteResult.UserInfoAttributes);
                result.AdditionalRedirectUrlQueryAttributes = postExecuteResult.AdditionalRedirectUrlQueryAttributes;
            }

            await SaveTokenRecord(thirdPartyResult.Attributes, thirdPartyResult.Token, endpoint, loginEndpoint);

            return result;
        }

        private async Task SaveTokenRecord(Dictionary<string,string> attributes, string token,AuthorizationEndpoint endpoint, SystemLoginEndpoint loginEndpoint)
        {
            if (!endpoint.KeepThirdPartyToken)
            {
                return;
            }
            var userKey=await GetUserKey(attributes,endpoint, loginEndpoint);

            var service = await GetService(endpoint);
            var record=await _thirdPartySystemTokenRecordStore.QueryByUserKey(userKey, loginEndpoint.ID, endpoint.ID);

            bool needUpdate = false;
            if (record == null)
            {
                record = new ThirdPartySystemTokenRecord()
                {
                    ID = Guid.NewGuid(),
                    AuthorizationEndpointID = endpoint.ID,
                    SystemLoginEndpointID = loginEndpoint.ID,
                    LastRefeshTime = DateTime.UtcNow,
                    Timeout = await service.GetTimeout(endpoint.ThirdPartyConfiguration),
                    Token = token,
                    UserKey = userKey
                };

                try
                {
                    await _thirdPartySystemTokenRecordStore.Add(record);
                }
                catch(UtilityException ex)
                {
                    if (ex.Code== (int)Errors.ExistSameThirdPartySystemTokenRecord)
                    {
                        record = await _thirdPartySystemTokenRecordStore.QueryByUserKey(userKey, loginEndpoint.ID, endpoint.ID);
                        needUpdate = true;
                    }
                    else
                    {
                        throw ex;
                    }
                }
            }
            else
            {
                needUpdate = true;
            }

            if (needUpdate)
            {
                record.Timeout = await service.GetTimeout(endpoint.ThirdPartyConfiguration);
                record.LastRefeshTime = DateTime.UtcNow;
                record.Token = token;

                await _thirdPartySystemTokenRecordStore.Update(record);
            }

            

        }

        private async Task<string> GetUserKey(Dictionary<string, string> attributes, AuthorizationEndpoint endpoint, SystemLoginEndpoint loginEndpoint)
        {
            if (!attributes.TryGetValue(loginEndpoint.UserInfoKey, out string userKey))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundUserInfoKeyInThirdPartyTokenAttributes,
                    DefaultFormatting = "在登录终结点{0}、验证终结点{1}的第三方及后续处理中获取的令牌键值对中，找不到键为{2}的值",
                    ReplaceParameters = new List<object>() { loginEndpoint.Name, endpoint.Name, loginEndpoint.UserInfoKey }
                };

                throw new UtilityException((int)Errors.NotFoundUserInfoKeyInThirdPartyTokenAttributes, fragment);
            }

            return await Task.FromResult(userKey);
        }
    }


    /// <summary>
    /// 第三方系统令牌结果
    /// </summary>
    [DataContract]
    public class ThirdPartySystemTokenResult
    {
        /// <summary>
        /// 令牌转换后的数据键值对
        /// </summary>
        [DataMember]
        public Dictionary<string, string> Attributes { get; set; }

    
        /// <summary>
        /// 附加到重定向地址的查询字符串键值对
        /// </summary>
        [DataMember]
        public Dictionary<string, string> AdditionalRedirectUrlQueryAttributes { get; set; }
    }

    /// <summary>
    /// 获取系统令牌动作的结果
    /// </summary>
    [DataContract]
    public class GetSystemTokenResult
    {
        /// <summary>
        /// 是否是直接返回系统令牌
        /// </summary>
        [DataMember]
        public bool Direct { get; set; }
        /// <summary>
        /// 如果Direct=true，则该值为第三方系统令牌的结果
        /// 否则该值为null
        /// </summary>
        [DataMember]
        public ThirdPartySystemTokenResult TokenResult { get; set; }

        /// <summary>
        /// 如果Direct=false，则RedirectUrl为第三方验证系统的登陆地址
        /// </summary>
        [DataMember]
        public string RedirectUrl { get; set; }
    }



}
