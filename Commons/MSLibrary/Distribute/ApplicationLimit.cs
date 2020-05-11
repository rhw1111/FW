using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.Distribute
{
    /// <summary>
    /// 应用程序限流
    /// 用来控制流速
    /// </summary>
    public class ApplicationLimit : EntityBase<IApplicationLimitIMP>
    {
        private IFactory<IApplicationLimitIMP> _applicationLimitIMPFactory;

        public IFactory<IApplicationLimitIMP> ApplicationLimitIMPFactory
        {
            set
            {
                _applicationLimitIMPFactory = value;
            }
        }
        public override IFactory<IApplicationLimitIMP> GetIMPFactory()
        {
            return _applicationLimitIMPFactory;
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
        /// 类型对应配置
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


    }

    /// <summary>
    /// 
    /// </summary>
    public interface IApplicationLimitIMP
    {
        Task Acquire(ApplicationLimit limit,string resourceName, int expireMillisecond);
        void AcquireSync(ApplicationLimit limit,string resourceName, int expireMillisecond);
    }

    public interface IApplicationLimitService
    {
        Task Acquire(string configuration, string resourceName, int expireMillisecond);
        void AcquireSync(string configuration, string resourceName, int expireMillisecond);
    }

    [Injection(InterfaceType = typeof(IApplicationLimitIMP), Scope = InjectionScope.Transient)]
    public class ApplicationLimitIMP : IApplicationLimitIMP
    {
        public static IDictionary<string, IFactory<IApplicationLimitService>> ApplicationLimitServiceFactories { get; } = new Dictionary<string, IFactory<IApplicationLimitService>>();

        public async Task Acquire(ApplicationLimit limit, string resourceName, int expireMillisecond)
        {
            var service = getService(limit.Type);
            await service.Acquire(limit.Configuration, resourceName, expireMillisecond);
        }

        public void AcquireSync(ApplicationLimit limit, string resourceName, int expireMillisecond)
        {
            var service = getService(limit.Type);
            service.AcquireSync(limit.Configuration, resourceName, expireMillisecond);
        }


        private IApplicationLimitService getService(string type)
        {
            if (!ApplicationLimitServiceFactories.TryGetValue(type, out IFactory<IApplicationLimitService> serviceFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundApplicationLimitServiceByType,
                    DefaultFormatting = "找不到类型为{0}的应用程序限流服务，发生位置{1}",
                    ReplaceParameters = new List<object>() { type, $"{this.GetType().FullName}.ApplicationLimitServiceFactories" }
                };
                throw new UtilityException((int)Errors.NotFoundApplicationLimitServiceByType, fragment);
            }

            return serviceFactory.Create();
        }

    }
}
