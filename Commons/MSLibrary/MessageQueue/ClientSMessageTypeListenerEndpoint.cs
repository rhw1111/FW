using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.MessageQueue.DAL;
using MSLibrary.Security;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.MessageQueue
{
    /// <summary>
    /// 客户端消息类型监听终结点
    /// 负责维护监听终结点的配置信息，验证数据是否合法
    /// 用于接收消息的系统
    /// </summary>
    public class ClientSMessageTypeListenerEndpoint : EntityBase<IClientSMessageTypeListenerEndpointIMP>
    {
        private static IFactory<IClientSMessageTypeListenerEndpointIMP> _clientSMessageTypeListenerEndpointIMPFactory;

        public static IFactory<IClientSMessageTypeListenerEndpointIMP> ClientSMessageTypeListenerEndpointIMPFactory
        {
            set
            {
                _clientSMessageTypeListenerEndpointIMPFactory = value;
            }
        }


        public override IFactory<IClientSMessageTypeListenerEndpointIMP> GetIMPFactory()
        {
            return _clientSMessageTypeListenerEndpointIMPFactory;
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
        /// 需要与服务端注册的监听所用的密钥一致
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
        /// 验证消息数据是否合法
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<ValidateResult> ValidateSMessageData(SMessagePostData data)
        {
            return await _imp.ValidateSMessageData(this,data);
        }

    }

    public interface IClientSMessageTypeListenerEndpointIMP
    {
        Task Add(ClientSMessageTypeListenerEndpoint endpoint);
        Task Update(ClientSMessageTypeListenerEndpoint endpoint);
        Task Delete(ClientSMessageTypeListenerEndpoint endpoint);

        Task<ValidateResult> ValidateSMessageData(ClientSMessageTypeListenerEndpoint endpoint,SMessagePostData data);
    }

    [Injection(InterfaceType = typeof(IClientSMessageTypeListenerEndpointIMP), Scope = InjectionScope.Transient)]
    public class ClientSMessageTypeListenerEndpointIMP : IClientSMessageTypeListenerEndpointIMP
    {
        private IClientSMessageTypeListenerEndpointStore _clientSMessageTypeListenerEndpointStore;
        private ISecurityService _securityService;

        public ClientSMessageTypeListenerEndpointIMP(IClientSMessageTypeListenerEndpointStore clientSMessageTypeListenerEndpointStore,ISecurityService securityService)
        {
            _clientSMessageTypeListenerEndpointStore = clientSMessageTypeListenerEndpointStore;
            _securityService = securityService;
        }
        public async Task Add(ClientSMessageTypeListenerEndpoint endpoint)
        {
            await _clientSMessageTypeListenerEndpointStore.Add(endpoint);
        }

        public async Task Delete(ClientSMessageTypeListenerEndpoint endpoint)
        {
            await _clientSMessageTypeListenerEndpointStore.Delete(endpoint.ID);
        }

        public async Task Update(ClientSMessageTypeListenerEndpoint endpoint)
        {
            await _clientSMessageTypeListenerEndpointStore.Update(endpoint);
        }

        public async Task<ValidateResult> ValidateSMessageData(ClientSMessageTypeListenerEndpoint endpoint, SMessagePostData data)
        {
            ValidateResult result = new ValidateResult()
            {
                 Result=true
            };
            var strContent = $"{data.ID.ToString()}{data.Type}{data.Data}{data.ExpireTime.ToString("yyyy-MM-dd hh:mm:ss")}";
            var strSignature = _securityService.SignByKey(strContent, endpoint.SignatureKey);
            if (strSignature!=data.Signature)
            {
                result.Result = false;
                result.Description = string.Format(StringLanguageTranslate.Translate(TextCodes.ClientSMessageTypeListenerEndpointSignatureError, "客户端消息类型监听终结点{0}在解析消息数据时消息数据的签名错误，请检查客户端的签名与服务端对应的签名是否一致"), endpoint.Name);
                return result;
            }

            if (data.ExpireTime<DateTime.UtcNow)
            {
                result.Result = false;
                result.Description = string.Format(StringLanguageTranslate.Translate(TextCodes.ClientSMessageTypeListenerEndpointMessageExpire, "客户端消息类型监听终结点{0}接收的消息已过期，过期时间为{1}"), endpoint.Name,data.ExpireTime.ToString());
                return result;
            }

            return await Task.FromResult(result);
        }
    }
}
