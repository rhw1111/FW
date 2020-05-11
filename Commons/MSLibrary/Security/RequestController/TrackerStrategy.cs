using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;

namespace MSLibrary.Security.RequestController
{
    /// <summary>
    /// 跟踪策略
    /// 负责请求跟踪的具体算法执行
    /// </summary>
    public class TrackerStrategy : EntityBase<ITrackerStrategyIMP>
    {
        private static IFactory<ITrackerStrategyIMP> _trackerStrategyIMPFactory;

        public static IFactory<ITrackerStrategyIMP> TrackerStrategyIMPFactory
        {
            set
            {
                _trackerStrategyIMPFactory = value;
            }
        }


        public override IFactory<ITrackerStrategyIMP> GetIMPFactory()
        {
            return _trackerStrategyIMPFactory;
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
        /// 请求进入
        /// </summary>
        /// <param name="tracker"></param>
        /// <returns></returns>
        public async Task<ValidateResult> Access(RequestTracker tracker)
        {
            return await _imp.Access(this, tracker);
        }
        /// <summary>
        /// 请求退出
        /// </summary>
        /// <param name="tracker"></param>
        /// <returns></returns>
        public async Task Exit(RequestTracker tracker)
        {
            await _imp.Exit(this, tracker);
        }
    }



    /// <summary>
    /// 跟踪策略具体实现接口
    /// </summary>
    public interface ITrackerStrategyIMP
    {
        Task<ValidateResult> Access(TrackerStrategy trackerStrategy, RequestTracker tracker);
        Task Exit(TrackerStrategy trackerStrategy, RequestTracker tracker);
    }

    [Injection(InterfaceType = typeof(ITrackerStrategyIMP), Scope = InjectionScope.Transient)]
    public class TrackerStrategyIMP : ITrackerStrategyIMP
    {
        private static Dictionary<string, IFactory<ITrackerStrategyService>> _strategyServiceFactories = new Dictionary<string, IFactory<ITrackerStrategyService>>();

        public static Dictionary<string, IFactory<ITrackerStrategyService>> StrategyServiceFactories
        {
            get
            {
                return _strategyServiceFactories;
            }
        }

        public async Task<ValidateResult> Access(TrackerStrategy trackerStrategy, RequestTracker tracker)
        {
            var service = await GetStrategyService(trackerStrategy.Name);
            return await service.Access(tracker);
        }

        public async Task Exit(TrackerStrategy trackerStrategy, RequestTracker tracker)
        {
            var service = await GetStrategyService(trackerStrategy.Name);
            await service.Exit(tracker);
        }

        private async Task<ITrackerStrategyService> GetStrategyService(string name)
        {
            if (!_strategyServiceFactories.TryGetValue(name, out IFactory < ITrackerStrategyService > serviceFactory))
            {
                throw new Exception("not found TrackerStrategyService Factory {0} in TrackerStrategyIMP.StrategyServiceFactories");
            }

            return await Task.FromResult(serviceFactory.Create());
        }
    }


    /// <summary>
    /// 策略服务
    /// 负责最终的策略实现
    /// </summary>
    public interface ITrackerStrategyService
    {
        Task<ValidateResult> Access(RequestTracker tracker);
        Task Exit(RequestTracker tracker);
    }
}
