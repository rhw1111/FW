using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Diagnostics.Contracts;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using MSLibrary.CommonQueue.DAL;
using MSLibrary.Thread;


namespace MSLibrary.CommonQueue
{
    public class CommonQueueConsumeEndpoint : EntityBase<ICommonQueueConsumeEndpointIMP>
    {
        private static IFactory<ICommonQueueConsumeEndpointIMP> _commonQueueConsumeEndpointIMPFactory;

        public static IFactory<ICommonQueueConsumeEndpointIMP> CommonQueueConsumeEndpointIMPFactory
        {
            set
            {
                _commonQueueConsumeEndpointIMPFactory = value;
            }
        }

        public override IFactory<ICommonQueueConsumeEndpointIMP> GetIMPFactory()
        {
            return _commonQueueConsumeEndpointIMPFactory;
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

        public async Task<ICommonQueueEndpointConsumeController> Consume(Func<CommonMessage, Task> messageHandle)
        {
            return await _imp.Consume(this, messageHandle);
        }

    }

    public interface ICommonQueueConsumeEndpointIMP
    {
        Task Add(CommonQueueConsumeEndpoint endpoint);
        Task Update(CommonQueueConsumeEndpoint endpoint);
        Task Delete(CommonQueueConsumeEndpoint endpoint);
        Task<ICommonQueueEndpointConsumeController> Consume(CommonQueueConsumeEndpoint endpoint,Func<CommonMessage,Task> messageHandle);
    }


    [Injection(InterfaceType = typeof(ICommonQueueConsumeEndpointIMP), Scope = InjectionScope.Transient)]
    public class CommonQueueConsumeEndpointIMP : ICommonQueueConsumeEndpointIMP
    {
        private static Dictionary<string, IFactory<IQueueRealExecuteService>> _queueRealExecuteServiceFactories = new Dictionary<string, IFactory<IQueueRealExecuteService>>();

        public static IDictionary<string, IFactory<IQueueRealExecuteService>> QueueRealExecuteServiceFactories
        {
            get
            {
                return _queueRealExecuteServiceFactories;
            }
        }
        private ICommonQueueConsumeEndpointStore _commonQueueConsumeEndpointStore;

        private ObjectWrapper<bool> _start = new ObjectWrapper<bool>(false);

        public CommonQueueConsumeEndpointIMP(ICommonQueueConsumeEndpointStore commonQueueConsumeEndpointStore)
        {
            _commonQueueConsumeEndpointStore = commonQueueConsumeEndpointStore;
        }


        public async Task Add(CommonQueueConsumeEndpoint endpoint)
        {
            await _commonQueueConsumeEndpointStore.Add(endpoint);
        }

        public async Task<ICommonQueueEndpointConsumeController> Consume(CommonQueueConsumeEndpoint endpoint, Func<CommonMessage, Task> messageHandle)
        {
            CommonQueueEndpointConsumeControllerWrapper wrapper=null;
            if (!_start.Value)
            {
                var service = getService(endpoint.QueueType);
                var controller= await service.Consume(endpoint, endpoint.QueueConfiguration, messageHandle);
                if (controller != null)
                {
                    _start.Value = true;
                    wrapper = new CommonQueueEndpointConsumeControllerWrapper(controller, _start);
                }
            }

            return wrapper;

        }

        public async Task Delete(CommonQueueConsumeEndpoint endpoint)
        {
            await _commonQueueConsumeEndpointStore.Delete(endpoint.ID);
        }

        public async Task Update(CommonQueueConsumeEndpoint endpoint)
        {
            await _commonQueueConsumeEndpointStore.Update(endpoint);
        }

        private IQueueRealExecuteService getService(string type)
        {
            if (!_queueRealExecuteServiceFactories.TryGetValue(type,out IFactory<IQueueRealExecuteService> serviceFactory))
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




    public interface ICommonQueueEndpointConsumeController
    {
        Task Stop();
    }

    public class CommonQueueEndpointConsumeControllerDefault : ICommonQueueEndpointConsumeController
    {
        private PollingResult _pollingResult;
        public CommonQueueEndpointConsumeControllerDefault(PollingResult pollingResult)
        {
            _pollingResult = pollingResult;
        }
        public async Task Stop()
        {
            _pollingResult.Stop();
            await Task.FromResult(0);
        }
    }

    public class CommonQueueEndpointConsumeControllerWrapper : ICommonQueueEndpointConsumeController
    {
        private ICommonQueueEndpointConsumeController _controller;
        private ObjectWrapper<bool> _start;

        public CommonQueueEndpointConsumeControllerWrapper(ICommonQueueEndpointConsumeController controller, ObjectWrapper<bool> start)
        {
            _controller = controller;
            _start = start;
        }

        public async Task Stop()
        {
            await _controller.Stop();
            _start.Value = false;
        }
    }

    
    
}
