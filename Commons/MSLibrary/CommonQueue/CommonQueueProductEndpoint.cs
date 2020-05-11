using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using MSLibrary.CommonQueue.DAL;

namespace MSLibrary.CommonQueue
{
    public class CommonQueueProductEndpoint : EntityBase<ICommonQueueProductEndpointIMP>
    {
        private static IFactory<ICommonQueueProductEndpointIMP> _commonQueueProductEndpointIMPFactory;

        public static IFactory<ICommonQueueProductEndpointIMP> CommonQueueProductEndpointIMPFactory
        {
            set
            {
                _commonQueueProductEndpointIMPFactory = value;
            }
        }

        public override IFactory<ICommonQueueProductEndpointIMP> GetIMPFactory()
        {
            return _commonQueueProductEndpointIMPFactory;
        }

        /// <summary>
        /// id
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
        /// 队列类型
        /// </summary>
        public string QueueType
        {
            get
            {
                return GetAttribute<string>("QueueType");
            }
            set
            {
                SetAttribute<string>("QueueType", value);
            }
        }

        /// <summary>
        /// 队列配置
        /// </summary>
        public string QueueConfiguration
        {
            get
            {
                return GetAttribute<string>("QueueConfiguration");
            }
            set
            {
                SetAttribute<string>("QueueConfiguration", value);
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

        public async Task Add()
        {
            await _imp.Add(this);
        }

        public async Task Update()
        {
            await _imp.Update(this);
        }

        public async Task Delete()
        {
            await _imp.Delete(this);
        }

        public async Task Product(CommonMessage message)
        {
            await _imp.Product(this, message);
        }
    }

    public interface ICommonQueueProductEndpointIMP
    {
        Task Add(CommonQueueProductEndpoint endpoint);
        Task Update(CommonQueueProductEndpoint endpoint);
        Task Delete(CommonQueueProductEndpoint endpoint);
        Task Product(CommonQueueProductEndpoint endpoint, CommonMessage message);
    }

    [Injection(InterfaceType = typeof(ICommonQueueProductEndpointIMP), Scope = InjectionScope.Transient)]
    public class CommonQueueProductEndpointIMP : ICommonQueueProductEndpointIMP
    {
        private static Dictionary<string, IFactory<IQueueRealExecuteService>> _queueRealExecuteServiceFactories = new Dictionary<string, IFactory<IQueueRealExecuteService>>();

        public static IDictionary<string, IFactory<IQueueRealExecuteService>> QueueRealExecuteServiceFactories
        {
            get
            {
                return _queueRealExecuteServiceFactories;
            }
        }

        private ICommonQueueProductEndpointStore _commonQueueProductEndpointStore;

        public CommonQueueProductEndpointIMP(ICommonQueueProductEndpointStore commonQueueProductEndpointStore)
        {
            _commonQueueProductEndpointStore = commonQueueProductEndpointStore;
        }

        public async Task Add(CommonQueueProductEndpoint endpoint)
        {
            await _commonQueueProductEndpointStore.Add(endpoint);
        }

        public async Task Delete(CommonQueueProductEndpoint endpoint)
        {
            await _commonQueueProductEndpointStore.Delete(endpoint.ID);
        }

        public async Task Product(CommonQueueProductEndpoint endpoint, CommonMessage message)
        {
            var service=getService(endpoint.QueueType);
            await service.Product(endpoint,endpoint.QueueConfiguration, message);
        }

        public async Task Update(CommonQueueProductEndpoint endpoint)
        {
            await _commonQueueProductEndpointStore.Update(endpoint);
        }

        private IQueueRealExecuteService getService(string type)
        {
            if (!_queueRealExecuteServiceFactories.TryGetValue(type, out IFactory<IQueueRealExecuteService> serviceFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFountCommonQueueRealExecuteServiceByType,
                    DefaultFormatting = "找不到队列类型为{0}的队列实际处理服务，发生位置为{1}",
                    ReplaceParameters = new List<object>() { type, $"{this.GetType().FullName}.QueueRealExecuteServiceFactories" }
                };

                throw new UtilityException((int)Errors.NotFountCommonQueueRealExecuteServiceByType, fragment);
            }

            return serviceFactory.Create();
        }
    }
}
