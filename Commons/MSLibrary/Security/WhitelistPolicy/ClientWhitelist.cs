using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using MSLibrary.DI;
using MSLibrary.Security.WhitelistPolicy.DAL;
using MSLibrary.Serializer;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.Security.WhitelistPolicy
{
    /// <summary>
    /// 客户端白名单
    /// 供调用方使用
    /// </summary>
    public class ClientWhitelist : EntityBase<IClientWhitelistIMP>
    {
        private static IFactory<IClientWhitelistIMP> _clientWhitelistIMPFactory;

        public static IFactory<IClientWhitelistIMP> ClientWhitelistIMPFactory
        {
            set
            {
                _clientWhitelistIMPFactory = value;
            }
        }

        public override IFactory<IClientWhitelistIMP> GetIMPFactory()
        {
            return _clientWhitelistIMPFactory;
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
        public string SystemName
        {
            get
            {
                return GetAttribute<string>("SystemName");
            }
            set
            {
                SetAttribute<string>("SystemName", value);
            }
        }

        /// <summary>
        /// 系统密钥
        /// </summary>
        public string SystemSecret
        {
            get
            {
                return GetAttribute<string>("SystemSecret");
            }
            set
            {
                SetAttribute<string>("SystemSecret", value);
            }
        }

        /// <summary>
        /// 签名过期时间（毫秒）
        /// </summary>
        public int SignatureExpire
        {
            get
            {
                return GetAttribute<int>("SignatureExpire");
            }
            set
            {
                SetAttribute<int>("SignatureExpire", value);
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
        /// 生成签名
        /// </summary>
        /// <returns></returns>
        public async Task<string> GenerateSignature()
        {
            return await _imp.GenerateSignature(this);
        }
    }


    public interface IClientWhitelistIMP
    {
        Task Add(ClientWhitelist data);
        Task Update(ClientWhitelist data);
        Task Delete(ClientWhitelist data);
        Task<string> GenerateSignature(ClientWhitelist data);
    }


    [Injection(InterfaceType = typeof(IClientWhitelistIMP), Scope = InjectionScope.Transient)]
    public class ClientWhitelistIMP : IClientWhitelistIMP
    {
        private ISecurityService _securityService;
        private IClientWhitelistStore _clientWhitelistStore;

        public ClientWhitelistIMP(ISecurityService securityService, IClientWhitelistStore clientWhitelistStore)
        {
            _securityService = securityService;
            _clientWhitelistStore = clientWhitelistStore;
        }

        public async Task Add(ClientWhitelist data)
        {
            await _clientWhitelistStore.Add(data);
        }

        public async Task Delete(ClientWhitelist data)
        {
            await _clientWhitelistStore.Delete(data.ID);
        }

        public async Task<string> GenerateSignature(ClientWhitelist data)
        {
            //生成JWT字符串,playload格式为
            // {
            //     "iat":颁发时间,
            //     "exp":过期时间,
            //     "systemname":系统名称
            // }

            var utcNow = DateTime.UtcNow;
            Dictionary<string, string> playload = new Dictionary<string, string>()
            {
                { "systemname",data.SystemName}
            };
            var strSignature = _securityService.GenerateJWT(data.SystemSecret, playload, data.SignatureExpire);
            return await Task.FromResult(strSignature);
        }

        public async Task Update(ClientWhitelist data)
        {
            await _clientWhitelistStore.Update(data);
        }
    }
}
