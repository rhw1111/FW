using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.Xrm
{
    /// <summary>
    /// Crm服务工厂
    /// Crm服务由工厂产生
    /// </summary>
    public class CrmServiceFactory : EntityBase<ICrmServiceFactoryIMP>
    {
        private static IFactory<ICrmServiceFactoryIMP> _crmServiceFactoryIMPFactory;

        public static IFactory<ICrmServiceFactoryIMP> CrmServiceFactoryIMPFactory
        {
            set
            {
                _crmServiceFactoryIMPFactory = value;
            }
        }
        public override IFactory<ICrmServiceFactoryIMP> GetIMPFactory()
        {
            return _crmServiceFactoryIMPFactory;
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
        /// 工厂名称
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
        /// 工厂类型
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
        /// 配置内容
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

        /// <summary>
        /// 创建Crm服务
        /// </summary>
        /// <returns></returns>
        public async Task<ICrmService> Create()
        {
            return await _imp.Create(this);
        }
    }

    public interface ICrmServiceFactoryIMP
    {
        Task<ICrmService> Create(CrmServiceFactory factory);
    }

    [Injection(InterfaceType = typeof(ICrmServiceFactoryIMP), Scope = InjectionScope.Transient)]
    public class CrmServiceFactoryIMP : ICrmServiceFactoryIMP
    {
        private static Dictionary<string, IFactory<ICrmServiceFactoryService>> _serviceFactories = new Dictionary<string, IFactory<ICrmServiceFactoryService>>();

        public static Dictionary<string, IFactory<ICrmServiceFactoryService>> ServiceFactories
        {
            get
            {
                return _serviceFactories;
            }
        }

        public async Task<ICrmService> Create(CrmServiceFactory factory)
        {
            var factoryService = getService(factory);
            return await factoryService.Create(factory.Configuration);
        }

        private ICrmServiceFactoryService getService(CrmServiceFactory factory)
        {
            if (!_serviceFactories.TryGetValue(factory.Type, out IFactory<ICrmServiceFactoryService> serviceFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundCrmServiceFactoryServiceByType,
                    DefaultFormatting = "找不到类型为{0}的Crm服务工厂服务，发生位置：{1}",
                    ReplaceParameters = new List<object>() { factory.Type, $"{this.GetType().FullName}.ServiceFactories" }
                };

                throw new UtilityException((int)Errors.NotFoundCrmServiceFactoryServiceByType, fragment);
            }

            return serviceFactory.Create();
        }
    }


    public interface ICrmServiceFactoryService
    {
        Task<ICrmService> Create(string configuration);
    }
}
