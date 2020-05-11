using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using MSLibrary.Security;
using MSLibrary.SystemToken.DAL;
using MSLibrary.Serializer;

namespace MSLibrary.SystemToken
{
    /// <summary>
    /// 客户端系统登陆终结点
    /// 负责维护接入方系统与认证中心配置对应的信息以及数据验证
    /// 供接入方系统使用
    /// </summary>
    public class ClientSystemLoginEndpoint : EntityBase<IClientSystemLoginEndpointIMP>
    {
        private static IFactory<IClientSystemLoginEndpointIMP> _clientSystemLoginEndpointIMPFactory;

        public static IFactory<IClientSystemLoginEndpointIMP> ClientSystemLoginEndpointIMPFactory
        {
            set
            {
                _clientSystemLoginEndpointIMPFactory = value;
            }
        }

        public override IFactory<IClientSystemLoginEndpointIMP> GetIMPFactory()
        {
            return _clientSystemLoginEndpointIMPFactory;
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
        /// 名称
        /// 此名称需要与认证中心配置的系统登陆终结点名称一样
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
        /// 签名密钥
        /// 此密钥需要与认证中心配置的系统登陆终结点中的密钥一样
        /// </summary>
        public string SignatureKey
        {
            get
            {
                return GetAttribute<string>("SignatureKey");
            }
            set
            {
                SetAttribute<string>("SignatureKey", value);
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
    }

    public interface IClientSystemLoginEndpointIMP
    {
        Task Add(ClientSystemLoginEndpoint endpoint);
        Task Update(ClientSystemLoginEndpoint endpoint);
        Task Delete(ClientSystemLoginEndpoint endpoint);
        /// <summary>
        /// 验证获取用户信息的回调请求是否正确
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ValidateResult> ValidateGetUserInfoCallbackRequest(ClientSystemLoginEndpoint endpoint, GetUserInfoCallbackRequest request);
    }


    [Injection(InterfaceType = typeof(IClientSystemLoginEndpointIMP), Scope = InjectionScope.Transient)]
    public class ClientSystemLoginEndpointIMP : IClientSystemLoginEndpointIMP
    {
        private IClientSystemLoginEndpointStore _clientSystemLoginEndpointStore;
        private ISecurityService _securityService;

        public ClientSystemLoginEndpointIMP(IClientSystemLoginEndpointStore clientSystemLoginEndpointStore, ISecurityService securityService)
        {
            _clientSystemLoginEndpointStore = clientSystemLoginEndpointStore;
            _securityService = securityService;
        }
        public async Task Add(ClientSystemLoginEndpoint endpoint)
        {
            await _clientSystemLoginEndpointStore.Add(endpoint);
        }

        public async Task Delete(ClientSystemLoginEndpoint endpoint)
        {
            await _clientSystemLoginEndpointStore.Delete(endpoint.ID);
        }

        public async Task Update(ClientSystemLoginEndpoint endpoint)
        {
            await _clientSystemLoginEndpointStore.Update(endpoint);
        }

        public async Task<ValidateResult> ValidateGetUserInfoCallbackRequest(ClientSystemLoginEndpoint endpoint, GetUserInfoCallbackRequest request)
        {
            ValidateResult result = new ValidateResult()
            {
                Result = true
            };

            string strRedirectKV = JsonSerializerHelper.Serializer(request.RedirectUrlKV);
            string strSystemKV = JsonSerializerHelper.Serializer(request.SystemTokenKV);
            string strExpireTime = JsonSerializerHelper.Serializer(request.ExpireTime);


            //var strSignature = _securityService.SignByKey($"{endpoint.Name}{request.SystemToken}{strRedirectKV}{strSystemKV}{strExpireTime}", endpoint.SignatureKey);
            var strSignature = "";
            if (strSignature != request.Signature)
            {
                result.Result = false;
                result.Description = string.Format(StringLanguageTranslate.Translate(TextCodes.ClientSystemLoginEndpointSignatureError, "客户端系统登陆终结点{0}在解析请求数据时请求数据的签名错误，请检查客户端的签名与服务端对应的签名是否一致"), endpoint.Name);
                return result;
            }

            if (request.ExpireTime < DateTime.UtcNow)
            {
                result.Result = false;
                result.Description = string.Format(StringLanguageTranslate.Translate(TextCodes.ClientSystemLoginEndpointRequestExpire, "客户端系统登陆终结点{0}接收的请求已过期，过期时间为{1}"), endpoint.Name, request.ExpireTime.ToString());
                return result;
            }

            return await Task.FromResult(result);
        }
    }
}
