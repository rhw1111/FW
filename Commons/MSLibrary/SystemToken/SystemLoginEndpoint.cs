using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Transactions;
using System.Net;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using MSLibrary.Transaction;
using MSLibrary.DI;
using MSLibrary.Security;
using MSLibrary.Serializer;
using MSLibrary.LanguageTranslate;
using MSLibrary.SystemToken.DAL;

namespace MSLibrary.SystemToken
{
    /// <summary>
    /// 系统登陆终结点
    /// 负责维护各个接入方系统登录需要的配置信息、转换系统令牌为通用令牌等系统令牌后续处理的职责
    /// 系统登陆终结点与验证中节点之间的关系是多对多
    /// </summary>
    public class SystemLoginEndpoint : EntityBase<ISystemLoginEndpointIMP>
    {
        private static IFactory<ISystemLoginEndpointIMP> _systemLoginEndpointIMPFactory;

        public static IFactory<ISystemLoginEndpointIMP> SystemLoginEndpointIMPFactory
        {
            set
            {
                _systemLoginEndpointIMPFactory = value;
            }
        }

        public override IFactory<ISystemLoginEndpointIMP> GetIMPFactory()
        {
            return _systemLoginEndpointIMPFactory;
        }



        /// <summary>
        /// Id
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
        /// 系统名称
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
        /*/// <summary>
        /// 令牌类型
        /// 用来区分不同格式的令牌的生成
        /// </summary>
        public string TokenType
        {
            get
            {
                return GetAttribute<string>("TokenType");
            }
            set
            {
                SetAttribute<string>("TokenType", value);
            }
        }*/

        /// <summary>
        /// 签名密钥
        /// </summary>
        public string SecretKey
        {
            get
            {
                return GetAttribute<string>("SecretKey");
            }
            set
            {
                SetAttribute<string>("SecretKey", value);
            }
        }

        /// <summary>
        /// 通用令牌的过期时间（秒）
        /// </summary>
        public int ExpireSecond
        {
            get
            {
                return GetAttribute<int>("ExpireSecond");
            }
            set
            {
                SetAttribute<int>("ExpireSecond", value);
            }
        }


        /// <summary>
        /// 合法的客户端重定向基地址
        /// 这里的重定向基地址指的是接入方系统的重定向基地址
        /// </summary>
        public Uri[] ClientRedirectBaseUrls
        {
            get
            {
                return GetAttribute<Uri[]>("ClientRedirectBaseUrls");
            }
            set
            {
                SetAttribute<Uri[]>("ClientRedirectBaseUrls", value);
            }
        }


        /// <summary>
        /// 该系统终结点本身的基地址
        /// 第三方验证系统回调的地址以该地址为基地址
        /// </summary>
        public string BaseUrl
        {
            get
            {
                return GetAttribute<string>("BaseUrl");
            }
            set
            {
                SetAttribute<string>("BaseUrl", value);
            }
        }

        /// <summary>
        /// 用户信息键值对中作为唯一标示的键
        /// </summary>
        public string UserInfoKey
        {
            get
            {
                return GetAttribute<string>("UserInfoKey");
            }
            set
            {
                SetAttribute<string>("UserInfoKey", value);
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
        public async Task Delete()
        {
            await _imp.Delete(this);
        }
        /// <summary>
        /// 根据验证终结点名称匹配分页匹配查询关联的验证终结点
        /// </summary>
        /// <param name="authorizationName"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<QueryResult<AuthorizationEndpoint>> GetAuthorizationEndpoint(string authorizationName, int page, int pageSize)
        {
            return await _imp.GetAuthorizationEndpoint(this, authorizationName, page, pageSize);
        }


        /// <summary>
        /// 关联一个验证终结点
        /// </summary>
        /// <param name="authorizationEndpointId"></param>
        /// <returns></returns>
        public async Task AddAuthorizationEndpoint(Guid authorizationEndpointId)
        {
            await _imp.AddAuthorizationEndpoint(this,authorizationEndpointId);
        }
        /// <summary>
        /// 移除一个关联的而验证终结点
        /// </summary>
        /// <param name="authorizationEndpointId"></param>
        /// <returns></returns>
        public async Task RemoveAuthorizationEndpoint(Guid authorizationEndpointId)
        {
            await _imp.RemoveAuthorizationEndpoint(this,authorizationEndpointId);
        }
        /// <summary>
        /// 根据验证终结点ID获取关联的验证终结点
        /// </summary>
        /// <param name="authorizationEndpointId"></param>
        /// <returns></returns>
        public async Task<AuthorizationEndpoint> GetAuthorizationEndpoint(Guid authorizationEndpointId)
        {
            return await _imp.GetAuthorizationEndpoint(this, authorizationEndpointId);
        }

        /// <summary>
        /// 根据验证终结点名称获取关联的验证终结点
        /// </summary>
        /// <param name="authorizationEndpointName"></param>
        /// <returns></returns>
        public async Task<AuthorizationEndpoint> GetAuthorizationEndpoint(string authorizationEndpointName)
        {
            return await _imp.GetAuthorizationEndpoint(this, authorizationEndpointName);
        }


        /// <summary>
        /// 刷新令牌
        /// 获取新的令牌
        /// </summary>
        /// <param name="strToken">要刷新的令牌JWT字符串</param>
        /// <returns></returns>
        public async Task<string> RefreshToken(string strToken)
        {
            return await _imp.RefreshToken(this,strToken);
        }
        /// <summary>
        /// 登出指定的令牌
        /// </summary>
        /// <param name="strToken"></param>
        /// <returns></returns>
        public async Task LogoutToken(string strToken)
        {
            await _imp.LogoutToken(this,strToken);
        }

        /// <summary>
        /// 验证指定系统的客户端重定向地址是否合法
        /// 这里的客户端重定向地址为接入方系统提供的重定向地址
        /// </summary>
        /// <param name="clientRedirectUrl">客户端重定向地址</param>
        /// <returns></returns>
        public async Task<bool> ValidateClientRedirectUrl(string clientRedirectUrl)
        {
            return await _imp.ValidateClientRedirectUrl(this,clientRedirectUrl);
        }
        /// <summary>
        /// 获取通用令牌
        /// </summary>
        /// <param name="authorizationName">验证终结点名称</param>
        /// <param name="returnUrl">接入方系统的重定向地址</param>
        /// <returns>获取通用令牌动作的结果</returns>
        public async Task<GetCommonTokenResult> GetCommonToken(string authorizationName, string returnUrl)
        {
            return await _imp.GetCommonToken(this,authorizationName,returnUrl);
        }

        /// <summary>
        /// 获取从第三方登陆系统回调后产生的重定向回接入方系统的地址
        /// </summary>
        /// <param name="request">回调请求</param>
        /// <returns></returns>
        public async Task<string> GetCommonToken(HttpRequest request)
        {
            return await _imp.GetCommonToken(this, request);
        }

       /* /// <summary>
        /// 根据通用令牌的JWT字符串获取对应验证终结点的保持登录状态Url
        /// </summary>
        /// <param name="authorizationName"></param>
        /// <returns></returns>
        public async Task<string> GetKeepLoginUrl(string strToken)
        {
            return await _imp.GetKeepLoginUrl(this,strToken);
        }*/
        /// <summary>
        ///  根据通用令牌的JWT字符串获取对应验证终结点的登出Url
        /// </summary>
        /// <param name="strToken"></param>
        /// <returns></returns>
        /*public async Task<string> GetLogoutUrl(string strToken)
        {
            return await _imp.GetLogoutUrl(this,strToken);
        }*/

        /// <summary>
        /// 获取指定验证终结点的登陆地址
        /// </summary>
        /// <param name="authorizationName">验证终结点名称</param>
        /// <param name="returnUrl">实际重定向地址</param>
        /// <returns></returns>
        public async Task<string> GetLoginUrl(string authorizationName, string returnUrl)
        {
            return await _imp.GetLoginUrl(this, authorizationName, returnUrl);
        }

        /// <summary>
        /// 根据用户名密码获取指定验证终结点的令牌
        /// </summary>
        /// <param name="authorizationName"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<string> GetCommonToken(string authorizationName, string userName, string password)
        {
            return await _imp.GetCommonToken(this,authorizationName, userName, password);
        }
    }

    /// <summary>
    /// 系统登陆终结点具体实现接口
    /// </summary>
    public interface ISystemLoginEndpointIMP
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        Task Add(SystemLoginEndpoint endpoint);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        Task Update(SystemLoginEndpoint endpoint);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        Task Delete(SystemLoginEndpoint endpoint);
        /// <summary>
        /// 根据验证终结点名称匹配分页匹配查询关联的验证终结点
        /// </summary>
        /// <param name="authorizationName"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<QueryResult<AuthorizationEndpoint>> GetAuthorizationEndpoint(SystemLoginEndpoint endpoint,string authorizationName,int page,int pageSize);

        
        /// <summary>
        /// 关联一个验证终结点
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="authorizationEndpointId"></param>
        /// <returns></returns>
        Task AddAuthorizationEndpoint(SystemLoginEndpoint endpoint,Guid authorizationEndpointId);
        /// <summary>
        /// 移除一个关联的而验证终结点
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="authorizationEndpointId"></param>
        /// <returns></returns>
        Task RemoveAuthorizationEndpoint(SystemLoginEndpoint endpoint, Guid authorizationEndpointId);
        /// <summary>
        /// 根据验证终结点ID获取关联的验证终结点
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="authorizationEndpointId"></param>
        /// <returns></returns>
        Task<AuthorizationEndpoint> GetAuthorizationEndpoint(SystemLoginEndpoint endpoint, Guid authorizationEndpointId);

        /// <summary>
        /// 根据验证终结点名称获取关联的验证终结点
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="authorizationEndpointName"></param>
        /// <returns></returns>
        Task<AuthorizationEndpoint> GetAuthorizationEndpoint(SystemLoginEndpoint endpoint, string authorizationEndpointName);


        /// <summary>
        /// 刷新令牌
        /// 获取新的令牌
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="strToken">要刷新的令牌JWT字符串</param>
        /// <returns></returns>
        Task<string> RefreshToken(SystemLoginEndpoint endpoint, string strToken);
        /// <summary>
        /// 登出指定的令牌
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="strToken"></param>
        /// <returns></returns>
        Task LogoutToken(SystemLoginEndpoint endpoint, string strToken);
        /// <summary>
        /// 验证指定系统的客户端重定向地址是否合法
        /// 这里的客户端重定向地址为接入方系统提供的重定向地址
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="clientRedirectUrl">客户端重定向地址</param>
        /// <returns></returns>
        Task<bool> ValidateClientRedirectUrl(SystemLoginEndpoint endpoint, string clientRedirectUrl);
        /// <summary>
        /// 获取通用令牌
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="authorizationName">验证终结点名称</param>
        /// <param name="returnUrl">接入方系统的重定向地址</param>
        /// <returns>获取通用令牌动作的结果</returns>
        Task<GetCommonTokenResult> GetCommonToken(SystemLoginEndpoint endpoint,string authorizationName,string returnUrl);
        
        /// <summary>
        /// 获取从第三方登陆系统回调后产生的重定向回接入方系统的地址
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="request">回调请求</param>
        /// <returns></returns>
        Task<string> GetCommonToken(SystemLoginEndpoint endpoint, HttpRequest request);
        /// <summary>
        /// 根据通用令牌的JWT字符串获取对应验证终结点的保持登录状态Url
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="authorizationName"></param>
        /// <returns></returns>
        //Task<string> GetKeepLoginUrl(SystemLoginEndpoint endpoint, string strToken);
        /// <summary>
        ///  根据通用令牌的JWT字符串获取对应验证终结点的登出Url
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="strToken"></param>
        /// <returns></returns>
        //Task<string> GetLogoutUrl(SystemLoginEndpoint endpoint, string strToken);

        /// <summary>
        /// 获取指定验证终结点的登录地址
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="authorizationName"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        Task<string> GetLoginUrl(SystemLoginEndpoint endpoint, string authorizationName, string returnUrl);
        /// <summary>
        /// 通过用户名密码获取指定验证终结点的令牌
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="authorizationName"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<string> GetCommonToken(SystemLoginEndpoint endpoint, string authorizationName, string userName, string password);
    }

    [Injection(InterfaceType = typeof(SystemLoginEndpointIMP), Scope = InjectionScope.Transient)]
    public class SystemLoginEndpointIMP : ISystemLoginEndpointIMP
    {
        private IAuthorizationEndpointRepository _authorizationEndpointRepository;
        private ISecurityService _securityService;
        private ISystemLoginEndpointStore _systemLoginEndpointStore;
        private IAuthorizationEndpointStore _authorizationEndpointStore;



        public SystemLoginEndpointIMP(IAuthorizationEndpointRepository authorizationEndpointRepository, ISecurityService securityService, ISystemLoginEndpointStore systemLoginEndpointStore, IAuthorizationEndpointStore authorizationEndpointStore)
        {
            _authorizationEndpointRepository = authorizationEndpointRepository;
            _securityService = securityService;
            _systemLoginEndpointStore = systemLoginEndpointStore;
            _authorizationEndpointStore = authorizationEndpointStore;
        }

        public async Task Add(SystemLoginEndpoint endpoint)
        {
            await _systemLoginEndpointStore.Add(endpoint);
        }

        public async Task AddAuthorizationEndpoint(SystemLoginEndpoint endpoint, Guid authorizationEndpointId)
        {
            await _authorizationEndpointStore.AddSystemLoginEndpointRelation(authorizationEndpointId, endpoint.ID);
        }

        public async Task Delete(SystemLoginEndpoint endpoint)
        {
            await _systemLoginEndpointStore.Delete(endpoint.ID);
        }

        public async Task<QueryResult<AuthorizationEndpoint>> GetAuthorizationEndpoint(SystemLoginEndpoint endpoint, string authorizationName, int page, int pageSize)
        {
            return await _authorizationEndpointStore.QueryBySystemLoginEndpointRelationPage(endpoint.ID, authorizationName, page, pageSize);
        }
        public async Task<AuthorizationEndpoint> GetAuthorizationEndpoint(SystemLoginEndpoint endpoint, string authorizationEndpointName)
        {
            return await _authorizationEndpointStore.QueryBySystemLoginEndpointRelation(endpoint.ID, authorizationEndpointName);
        }

        public async Task<AuthorizationEndpoint> GetAuthorizationEndpoint(SystemLoginEndpoint endpoint, Guid authorizationEndpointId)
        {
            return await _authorizationEndpointStore.QueryBySystemLoginEndpointRelation(endpoint.ID, authorizationEndpointId);
        }

        /// <summary>
        /// 获取通用令牌
        /// 返回结果有两种情况
        /// 1，直接获取通用令牌，返回重定向回接入方系统的地址，该地址包含通用令牌的字符串信息
        /// 2，需要重定向到第三方认证系统，返回第三方认证系统的地址
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="authorizationName">验证终结点名称</param>
        /// <param name="returnUrl">接入方系统的重定向地址</param>
        /// <returns>获取通用令牌动作的结果</returns>
        public async Task<GetCommonTokenResult> GetCommonToken(SystemLoginEndpoint endpoint, string authorizationName, string returnUrl)
        {

            //验证客户端重定向地址
            await validateClientRedirectUrl(endpoint, returnUrl);

            //找到关联的验证终结点
            var authorizationEndpoint =await GetAuthorizationEndpoint(endpoint, authorizationName);
            if (authorizationEndpoint==null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundAuthorizationEndpointInSystemLoginEndpointByName,
                    DefaultFormatting = "名称为{0}的系统登录终结点中，找不到名称为{1}的关联认证终结点",
                    ReplaceParameters = new List<object>() { endpoint.Name, authorizationName }
                };

                throw new UtilityException((int)Errors.NotFoundAuthorizationEndpointInSystemLoginEndpointByName,fragment);
            }
            //生成验证中心重定向地址
            var sysUrl = GenerateSystemLoginUrl(endpoint, authorizationEndpoint);

            //var serviceReturnUrl = $"{endpoint.BaseUrl}?returnurl={WebUtility.UrlEncode(returnUrl)}";
            //调用认证终结点的获取系统令牌的方法
            var authResult=await authorizationEndpoint.GetSystemToken(endpoint, sysUrl, returnUrl);

            //从接入方系统的returnUrl上面获取Querystring的键值对

            //从请求中获取querystring,将它转成键值对
            Dictionary<string, string> returnUrlKV = new Dictionary<string, string>();
            Uri returnUrlUri = new Uri(returnUrl);
            var dictKV = QueryHelpers.ParseQuery(returnUrlUri.Query);
            foreach (var item in dictKV)
            {
                returnUrlKV.Add(item.Key,item.Value[0]);
            }


            GetCommonTokenResult result = new GetCommonTokenResult();
            if (authResult.Direct)
            {
                //组装结果
                result.Direct = true;
                var commonToken = new CommonToken()
                {
                    SystemName = endpoint.Name,
                    AuthorizationName = authorizationName,
                    UserInfoAttributes = authResult.TokenResult.Attributes
                };

                //生成通用令牌的JWT字符串
                var strCommonToken=_securityService.GenerateJWT(endpoint.SecretKey, new Dictionary<string, string>() { { "SystemName", commonToken.SystemName }, { "AuthorizationName", commonToken.AuthorizationName }, { "UserInfoAttributes", JsonSerializerHelper.Serializer<Dictionary<string, string>>(commonToken.UserInfoAttributes) } },endpoint.ExpireSecond);
                //生成重定向回接入方系统的地址
                var strReturnUrl=QueryHelpers.AddQueryString(returnUrl, authResult.TokenResult.AdditionalRedirectUrlQueryAttributes);
                strReturnUrl = QueryHelpers.AddQueryString(strReturnUrl, "commontoken", strCommonToken);

                result.CommonTokenRedirectUrl = strReturnUrl;
                


            }
            else
            {
                result.Direct = false;
                result.RedirectUrl = authResult.RedirectUrl;
            }

            return result;
        }




        public async Task<string> GetCommonToken(SystemLoginEndpoint endpoint, HttpRequest request)
        {
            //从request的query中获取authname
            if (!request.Query.TryGetValue("authname", out StringValues strAuthName))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundAuthNameQuerystringInAuthRedirectUrl,
                    DefaultFormatting = "名称为{0}的系统登录终结点的第三方认证系统回调请求处理中，回调请求的Url中不包含authname参数，回调请求的Url为{1}",
                    ReplaceParameters = new List<object>() { endpoint.Name, request.Path.Value }
                };

                throw new UtilityException((int)Errors.NotFoundAuthNameQuerystringInAuthRedirectUrl, fragment);
            }

            //根据authname获取登录终结点下面关联的验证终结点
            AuthorizationEndpoint authorizationEndpoint = await GetAuthorizationEndpoint(endpoint, strAuthName[0]);
            if (authorizationEndpoint == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundAuthorizationEndpointInSystemLoginEndpointCanExecuteCallback,
                    DefaultFormatting = "名称为{0}的系统登录终结点中，找不到可以处理从第三方认证系统回调请求的关联认证终结点，请求url为{1}",
                    ReplaceParameters = new List<object>() { endpoint.Name, request.Path.Value }
                };

                throw new UtilityException((int)Errors.NotFoundAuthorizationEndpointInSystemLoginEndpointCanExecuteCallback, fragment);
            }

            //调用验证终结点的方法，获取实际的重定向地址
            string redirectUrl = await authorizationEndpoint.GetRealRedirectUrl(request);

            //验证客户端重定向地址
            await validateClientRedirectUrl(endpoint, redirectUrl);

            //调用验证终结点的方法，获取第三方登陆系统处理后产生的键值对
            var authResult= await authorizationEndpoint.GetSystemAttributes(endpoint,request);


            //生成最终要重定向回接入方的Url
            var commonToken = new CommonToken()
            {
                SystemName = endpoint.Name,
                AuthorizationName = authorizationEndpoint.Name,
                UserInfoAttributes = authResult.Attributes
            };



            //生成通用令牌的JWT字符串
            var strCommonToken = _securityService.GenerateJWT(endpoint.SecretKey, new Dictionary<string, string>() { { "SystemName", commonToken.SystemName }, { "AuthorizationName", commonToken.AuthorizationName },  { "UserInfoAttributes", JsonSerializerHelper.Serializer<Dictionary<string, string>>(commonToken.UserInfoAttributes) } }, endpoint.ExpireSecond);
            //生成重定向回接入方系统的地址
            var strReturnUrl = QueryHelpers.AddQueryString(redirectUrl, authResult.AdditionalRedirectUrlQueryAttributes);
            strReturnUrl = QueryHelpers.AddQueryString(strReturnUrl, "commontoken", strCommonToken);

            return strReturnUrl;
        }

        /*public async Task<string> GetKeepLoginUrl(SystemLoginEndpoint endpoint, string strToken)
        {
            //验证JWT是否正确
            var jwtResult = _securityService.ValidateJWT(endpoint.SecretKey, strToken);
            if (!jwtResult.ValidateResult.Result)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.SystemLoginEndpointTokenValidateError,
                    DefaultFormatting = "名称为{0}的系统登录终结点验证令牌字符串{1}失败，失败原因{2}",
                    ReplaceParameters = new List<object>() { endpoint.Name, strToken, jwtResult.ValidateResult.Description }
                };

                //验证未通过，抛出异常
                throw new UtilityException((int)Errors.SystemLoginEndpointTokenValidateError, fragment);
            }

            //从JWT字符串中获取令牌相关信息
            Dictionary<string, string> jwtInfo = jwtResult.Playload;
            //查找验证终结点名称
            if (!jwtInfo.TryGetValue("AuthorizationName", out string strAuthorizationName))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundInfoInSystemLoginEndpointTokenByName,
                    DefaultFormatting = "名称为{0}的系统登录终结点验证令牌字符串{1}中，找不到名称为{2}的信息",
                    ReplaceParameters = new List<object>() { endpoint.Name, strToken, "AuthorizationName" }
                };

                throw new UtilityException((int)Errors.NotFoundInfoInSystemLoginEndpointTokenByName, fragment);
            }
            //查找用户信息键值对
            if (!jwtInfo.TryGetValue("UserInfoAttributes", out string strUserInfoAttributes))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundInfoInSystemLoginEndpointTokenByName,
                    DefaultFormatting = "名称为{0}的系统登录终结点验证令牌字符串{1}中，找不到名称为{2}的信息",
                    ReplaceParameters = new List<object>() { endpoint.Name, strToken, "UserInfoAttributes" }
                };

                throw new UtilityException((int)Errors.NotFoundInfoInSystemLoginEndpointTokenByName, fragment);
            }

            //查询出该登录终结点关联的验证终结点中相同名称的验证终结点
            //调用验证终结点的获取保持登录状态url方法
            var authorizationEndpoint = await GetAuthorizationEndpoint(endpoint, strAuthorizationName);
            if (authorizationEndpoint == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundAuthorizationEndpointInSystemLoginEndpointByName,
                    DefaultFormatting = "名称为{0}的系统登录终结点中，找不到名称为{1}的关联认证终结点",
                    ReplaceParameters = new List<object>() { endpoint.Name, strAuthorizationName }
                };

                throw new UtilityException((int)Errors.NotFoundAuthorizationEndpointInSystemLoginEndpointByName, fragment);
            }

            return await authorizationEndpoint.GetKeepLoginUrl(strSystemToken);
        }*/

        public async Task<string> GetLoginUrl(SystemLoginEndpoint endpoint, string authorizationName, string returnUrl)
        {
            //找到关联的验证终结点
            var authorizationEndpoint = await GetAuthorizationEndpoint(endpoint, authorizationName);
            if (authorizationEndpoint == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundAuthorizationEndpointInSystemLoginEndpointByName,
                    DefaultFormatting = "名称为{0}的系统登录终结点中，找不到名称为{1}的关联认证终结点",
                    ReplaceParameters = new List<object>() { endpoint.Name, authorizationName }
                };

                throw new UtilityException((int)Errors.NotFoundAuthorizationEndpointInSystemLoginEndpointByName, fragment);
            }

            //生成验证中心重定向地址
            var sysUrl=GenerateSystemLoginUrl(endpoint, authorizationEndpoint);
            return await authorizationEndpoint.GetLoginUrl(sysUrl, returnUrl);
        }

        /*public async Task<string> GetLogoutUrl(SystemLoginEndpoint endpoint, string strToken)
        {
            //验证JWT是否正确
            var jwtResult = _securityService.ValidateJWT(endpoint.SecretKey, strToken);
            if (!jwtResult.ValidateResult.Result)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.SystemLoginEndpointTokenValidateError,
                    DefaultFormatting = "名称为{0}的系统登录终结点验证令牌字符串{1}失败，失败原因{2}",
                    ReplaceParameters = new List<object>() { endpoint.Name, strToken, jwtResult.ValidateResult.Description }
                };

                //验证未通过，抛出异常
                throw new UtilityException((int)Errors.SystemLoginEndpointTokenValidateError,fragment);
            }

            //从JWT字符串中获取令牌相关信息
            Dictionary<string, string> jwtInfo = jwtResult.Playload;
            //查找验证终结点名称
            if (!jwtInfo.TryGetValue("AuthorizationName", out string strAuthorizationName))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundInfoInSystemLoginEndpointTokenByName,
                    DefaultFormatting = "名称为{0}的系统登录终结点验证令牌字符串{1}中，找不到名称为{2}的信息",
                    ReplaceParameters = new List<object>() { endpoint.Name, strToken, "AuthorizationName" }
                };

                throw new UtilityException((int)Errors.NotFoundInfoInSystemLoginEndpointTokenByName, fragment);
            }
            //查找用户信息键值对
            if (!jwtInfo.TryGetValue("UserInfoAttributes", out string strUserInfoAttributes))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundInfoInSystemLoginEndpointTokenByName,
                    DefaultFormatting = "名称为{0}的系统登录终结点验证令牌字符串{1}中，找不到名称为{2}的信息",
                    ReplaceParameters = new List<object>() { endpoint.Name, strToken, "UserInfoAttributes" }
                };

                throw new UtilityException((int)Errors.NotFoundInfoInSystemLoginEndpointTokenByName, fragment);
            }


            //查询出该登录终结点关联的验证终结点中相同名称的验证终结点
            //调用验证终结点的获取登出url方法
            var authorizationEndpoint = await GetAuthorizationEndpoint(endpoint, strAuthorizationName);
            if (authorizationEndpoint == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundAuthorizationEndpointInSystemLoginEndpointByName,
                    DefaultFormatting = "名称为{0}的系统登录终结点中，找不到名称为{1}的关联认证终结点",
                    ReplaceParameters = new List<object>() { endpoint.Name, strAuthorizationName }
                };

                throw new UtilityException((int)Errors.NotFoundAuthorizationEndpointInSystemLoginEndpointByName, fragment);
            }

            var userInfoAttributes=JsonSerializerHelper.Deserialize<Dictionary<string, string>>(strUserInfoAttributes);
            if (userInfoAttributes==null)
            {
                userInfoAttributes = new Dictionary<string, string>();
            }

            return await authorizationEndpoint.GetLogoutUrl(userInfoAttributes);
        }
        */
        public async Task<string> GetCommonToken(SystemLoginEndpoint endpoint, string authorizationName, string userName, string password)
        {
            //找到关联的验证终结点
            var authorizationEndpoint = await GetAuthorizationEndpoint(endpoint, authorizationName);
            if (authorizationEndpoint == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundAuthorizationEndpointInSystemLoginEndpointByName,
                    DefaultFormatting = "名称为{0}的系统登录终结点中，找不到名称为{1}的关联认证终结点",
                    ReplaceParameters = new List<object>() { endpoint.Name, authorizationName }
                };

                throw new UtilityException((int)Errors.NotFoundAuthorizationEndpointInSystemLoginEndpointByName, fragment);
            }

            var authResult= await authorizationEndpoint.GetSystemTokenByPassword(endpoint, userName, password);

            var commonToken = new CommonToken()
            {
                SystemName = endpoint.Name,
                AuthorizationName = authorizationName,
                UserInfoAttributes = authResult.Attributes
            };


            //生成通用令牌的JWT字符串
            var strCommonToken = _securityService.GenerateJWT(endpoint.SecretKey, new Dictionary<string, string>() { { "SystemName", commonToken.SystemName }, { "AuthorizationName", commonToken.AuthorizationName }, { "UserInfoAttributes", JsonSerializerHelper.Serializer<Dictionary<string, string>>(commonToken.UserInfoAttributes) } }, endpoint.ExpireSecond);

            return strCommonToken;
        }

        public async Task LogoutToken(SystemLoginEndpoint endpoint, string strToken)
        {
            //验证JWT是否正确
            var jwtResult = _securityService.ValidateJWT(endpoint.SecretKey, strToken);
            if (!jwtResult.ValidateResult.Result)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.SystemLoginEndpointTokenValidateError,
                    DefaultFormatting = "名称为{0}的系统登录终结点验证令牌字符串{1}失败，失败原因{2}",
                    ReplaceParameters = new List<object>() { endpoint.Name, strToken, jwtResult.ValidateResult.Description }
                };

                //验证未通过，抛出异常
                throw new UtilityException((int)Errors.SystemLoginEndpointTokenValidateError, fragment);
            }

            //从JWT字符串中获取令牌相关信息,这里需要补全代码
            Dictionary<string, string> jwtInfo = jwtResult.Playload;
            //查找验证终结点名称
            if (!jwtInfo.TryGetValue("AuthorizationName", out string strAuthorizationName))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundInfoInSystemLoginEndpointTokenByName,
                    DefaultFormatting = "名称为{0}的系统登录终结点验证令牌字符串{1}中，找不到名称为{2}的信息",
                    ReplaceParameters = new List<object>() { endpoint.Name, strToken, "AuthorizationName" }
                };

                throw new UtilityException((int)Errors.NotFoundInfoInSystemLoginEndpointTokenByName,fragment);
            }
            //查找用户信息键值对
            if (!jwtInfo.TryGetValue("UserInfoAttributes", out string strUserInfoAttributes))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundInfoInSystemLoginEndpointTokenByName,
                    DefaultFormatting = "名称为{0}的系统登录终结点验证令牌字符串{1}中，找不到名称为{2}的信息",
                    ReplaceParameters = new List<object>() { endpoint.Name, strToken, "UserInfoAttributes" }
                };

                throw new UtilityException((int)Errors.NotFoundInfoInSystemLoginEndpointTokenByName, fragment);
            }
            //查找第三方验证系统的系统令牌
            if (!jwtInfo.TryGetValue("SystemToken", out string strSystemToken))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundInfoInSystemLoginEndpointTokenByName,
                    DefaultFormatting = "名称为{0}的系统登录终结点验证令牌字符串{1}中，找不到名称为{2}的信息",
                    ReplaceParameters = new List<object>() { endpoint.Name, strToken, "SystemToken" }
                };

                throw new UtilityException((int)Errors.NotFoundInfoInSystemLoginEndpointTokenByName, fragment);
            }

            //查询出该登录终结点关联的验证终结点中相同名称的验证终结点
            //调用验证终结点的登出方法
            var authorizationEndpoint = await GetAuthorizationEndpoint(endpoint, strAuthorizationName);
            if (authorizationEndpoint == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundAuthorizationEndpointInSystemLoginEndpointByName,
                    DefaultFormatting = "名称为{0}的系统登录终结点中，找不到名称为{1}的关联认证终结点",
                    ReplaceParameters = new List<object>() { endpoint.Name, strAuthorizationName }
                };
                throw new UtilityException((int)Errors.NotFoundAuthorizationEndpointInSystemLoginEndpointByName, fragment);
            }

            await authorizationEndpoint.Logout(strSystemToken);
        }

        public async Task<string> RefreshToken(SystemLoginEndpoint endpoint, string strToken)
        {
            //验证JWT是否正确
            var jwtResult= _securityService.ValidateJWT(endpoint.SecretKey, strToken);
            if (!jwtResult.ValidateResult.Result)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.SystemLoginEndpointTokenValidateError,
                    DefaultFormatting = "名称为{0}的系统登录终结点验证令牌字符串{1}失败，失败原因{2}",
                    ReplaceParameters = new List<object>() { endpoint.Name, strToken, jwtResult.ValidateResult.Description }
                };

                //验证未通过，抛出异常
                throw new UtilityException((int)Errors.SystemLoginEndpointTokenValidateError, fragment);
            }

            //从JWT字符串中获取令牌相关信息,这里需要补全代码
            Dictionary<string, string> jwtInfo = jwtResult.Playload;
            //查找验证终结点名称
            if (!jwtInfo.TryGetValue("AuthorizationName",out string strAuthorizationName))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundInfoInSystemLoginEndpointTokenByName,
                    DefaultFormatting = "名称为{0}的系统登录终结点验证令牌字符串{1}中，找不到名称为{2}的信息",
                    ReplaceParameters = new List<object>() { endpoint.Name, strToken, "AuthorizationName" }
                };

                throw new UtilityException((int)Errors.NotFoundInfoInSystemLoginEndpointTokenByName,fragment);
            }
            //查找用户信息键值对
            if (!jwtInfo.TryGetValue("UserInfoAttributes", out string strUserInfoAttributes))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundInfoInSystemLoginEndpointTokenByName,
                    DefaultFormatting = "名称为{0}的系统登录终结点验证令牌字符串{1}中，找不到名称为{2}的信息",
                    ReplaceParameters = new List<object>() { endpoint.Name, strToken, "UserInfoAttributes" }
                };

                throw new UtilityException((int)Errors.NotFoundInfoInSystemLoginEndpointTokenByName,fragment);
            }

            
            //查询出该登录终结点关联的验证终结点中相同名称的验证终结点
            //调用验证终结点的刷新方法
           /* var authorizationEndpoint= await GetAuthorizationEndpoint(endpoint, strAuthorizationName);
            if (authorizationEndpoint==null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundAuthorizationEndpointInSystemLoginEndpointByName,
                    DefaultFormatting = "名称为{0}的系统登录终结点中，找不到名称为{1}的关联认证终结点",
                    ReplaceParameters = new List<object>() { endpoint.Name, strAuthorizationName }
                };

                throw new UtilityException((int)Errors.NotFoundAuthorizationEndpointInSystemLoginEndpointByName, fragment);
            }*/


            //重新生成通用令牌的JWT字符串
            var strCommonToken = _securityService.GenerateJWT(endpoint.SecretKey, new Dictionary<string, string>() { { "SystemName", endpoint.Name }, { "AuthorizationName", strAuthorizationName },  { "UserInfoAttributes", strUserInfoAttributes } }, endpoint.ExpireSecond);

            return await Task.FromResult(strCommonToken);
        }

        public async Task RemoveAuthorizationEndpoint(SystemLoginEndpoint endpoint, Guid authorizationEndpointId)
        {
            await _authorizationEndpointStore.DeleteSystemLoginEndpointRelation(authorizationEndpointId, endpoint.ID);
        }

        public async Task Update(SystemLoginEndpoint endpoint)
        {
            await _systemLoginEndpointStore.Update(endpoint);
        }

        public async Task<bool> ValidateClientRedirectUrl(SystemLoginEndpoint endpoint, string clientRedirectUrl)
        {
            bool result = false;
            foreach (var item in endpoint.ClientRedirectBaseUrls)
            {
                if (item.IsBaseOf(new Uri(clientRedirectUrl)))
                {
                    result = true;
                    break;
                }
            }

            return await Task.FromResult(result);
        }


        private async Task validateClientRedirectUrl(SystemLoginEndpoint endpoint, string clientRedirectUrl)
        {
            if (!await ValidateClientRedirectUrl(endpoint, clientRedirectUrl))
            {
               
                var fragment = new TextFragment()
                {
                    Code = TextCodes.SystemLoginEndpointClientRedirectUrl,
                    DefaultFormatting = "名称为{0}的系统登录终结点中，客户端重定向地址{1}非法，合法地址的基地址必须为{2}",
                    ReplaceParameters = new List<object>() { endpoint.Name, clientRedirectUrl, endpoint.ClientRedirectBaseUrls.ToDisplayString(async(item)=>await Task.FromResult(item.ToString()),async()=>await Task.FromResult(",")) }
                };

                throw new UtilityException((int)Errors.SystemLoginEndpointClientRedirectUrl, fragment);
            }
        }

        private string GenerateSystemLoginUrl(SystemLoginEndpoint sysEndpoint,AuthorizationEndpoint authEndpoint)
        {
            var serviceReturnUrl = QueryHelpers.AddQueryString(sysEndpoint.BaseUrl, new Dictionary<string, string>() { { "sysname", sysEndpoint.Name }, { "authname", authEndpoint.Name } });
            return serviceReturnUrl;
        }

    }


    /// <summary>
    /// 获取通用令牌动作产生的结果
    /// </summary>
    [DataContract]
    public class GetCommonTokenResult
    {
        /// <summary>
        /// 是否是直接返回通用令牌
        /// </summary>
        [DataMember]
        public bool Direct { get;  set; }
        /// <summary>
        /// 如果Direct=true，则CommonTokenRedirectUrl为重定向回接入方系统的地址
        /// 该地址包含了CommonToken的字符串
        /// 否则该值为null
        /// </summary>
        [DataMember]
        public string CommonTokenRedirectUrl { get; set; }
        /// <summary>
        /// 如果Direct=false，则RedirectUrl为第三方验证系统的登录地址
        /// </summary>
        [DataMember]
        public string RedirectUrl { get; set; }
    }

    /// <summary>
    /// 向接入方系统的获取用户信息服务发起的请求得到的响应
    /// </summary>
    [DataContract]
    public class GetUserInfoCallbackResponse
    {
        /// <summary>
        /// 用户信息键值对
        /// </summary>
        [DataMember]
        public Dictionary<string,string> UserInfos { get; set; }
        /// <summary>
        /// 需要附加到重定向地址上的键值对
        /// </summary>
        [DataMember]
        public Dictionary<string, string> AdditionalRedirectUrlQueryAttributes { get; set; }
    }

    
    /// <summary>
    /// 向接入方系统的获取用户信息服务发起的请求
    /// </summary>
    [DataContract]
    public class GetUserInfoCallbackRequest
    {
        /// <summary>
        /// 验证终结点名称
        /// </summary>
        [DataMember]
        public string AuthorizationName { get; set; }
        /// <summary>
        /// 系统令牌解析后的键值对
        /// </summary>
        [DataMember]
        public Dictionary<string,string> SystemTokenKV { get; set; }
        /// <summary>
        /// 客户端重定向地址的键值对
        /// </summary>
        [DataMember]
        public Dictionary<string,string> RedirectUrlKV { get; set; }
        /// <summary>
        /// 过期时间（UTC）
        /// </summary>
        [DataMember]
        public DateTime ExpireTime { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        [DataMember]
        public string Signature { get; set; }
    }
}
