using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.Security
{
    /// <summary>
    /// 机密数据终结点
    /// </summary>
    public class SecurityVaultEndpoint : EntityBase<ISecurityVaultEndpointIMP>
    {
        private static IFactory<ISecurityVaultEndpointIMP> _securityVaultEndpointIMPFactory;

        public static IFactory<ISecurityVaultEndpointIMP> SecurityVaultEndpointIMPFactory
        {
            set
            {
                _securityVaultEndpointIMPFactory = value;
            }
        }
        public override IFactory<ISecurityVaultEndpointIMP> GetIMPFactory()
        {
            return _securityVaultEndpointIMPFactory;
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
        /// 类型
        /// </summary>
        public string Type
        {
            get
            {
                return GetAttribute<string>("Type");
            }
            set
            {
                SetAttribute<string>("Type", value);
            }
        }

        /// <summary>
        /// 配置
        /// </summary>
        public string Configuration
        {
            get
            {
                return GetAttribute<string>("Configuration");
            }
            set
            {
                SetAttribute<string>("Configuration", value);
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

        public async Task<string> GetData(string name)
        {
            return await _imp.GetData(this, name);
        }
    }

    public interface ISecurityVaultEndpointIMP
    {
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<string> GetData(SecurityVaultEndpoint endpoint, string name);
    }

    public interface ISecurityVaultService
    {
        Task<string> GetData(string configuration, string name);
    }

    [Injection(InterfaceType = typeof(ISecurityVaultEndpointIMP), Scope = InjectionScope.Transient)]
    public class SecurityVaultEndpointIMP : ISecurityVaultEndpointIMP
    {
        public static IDictionary<string, IFactory<ISecurityVaultService>> SecurityVaultServiceFactories { get; } = new Dictionary<string, IFactory<ISecurityVaultService>>();

        public async Task<string> GetData(SecurityVaultEndpoint endpoint, string name)
        {
            var service = getService(endpoint.Type);
            return await service.GetData(endpoint.Configuration, name);
        }

        private ISecurityVaultService getService(string type)
        {
            if (!SecurityVaultServiceFactories.TryGetValue(type,out IFactory<ISecurityVaultService> serviceFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundSecurityVaultServiceByType,
                    DefaultFormatting = "找不到类型为{0}的机密数据服务，发生位置为{1}",
                    ReplaceParameters = new List<object>() { type,$"{this.GetType().FullName}.SecurityVaultServiceFactories" }
                };

                throw new UtilityException((int)Errors.NotFoundSecurityVaultServiceByType, fragment);
            }

            return serviceFactory.Create();
        }
    }
}