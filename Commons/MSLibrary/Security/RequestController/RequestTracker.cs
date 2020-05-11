using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;

namespace MSLibrary.Security.RequestController
{
    /// <summary>
    /// 请求跟踪实体
    /// 负责管理对请求阈值的管理
    /// </summary>
    public class RequestTracker : EntityBase<IRequestTrackerIMP>
    {
        private IFactory<IRequestTrackerIMP> _requestTrackerIMPFactory;

        public IFactory<IRequestTrackerIMP> RequestTrackerIMPFactory
        {
            set
            {
                _requestTrackerIMPFactory = value;
            }
        }
        public override IFactory<IRequestTrackerIMP> GetIMPFactory()
        {
            return _requestTrackerIMPFactory;
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
        /// 请求标识
        /// </summary>
        public string RequestKey
        {
            get
            {
                return GetAttribute<string>("RequestKey");
            }
            set
            {
                SetAttribute<string>("RequestKey", value);
            }
        }


        /// <summary>
        /// 最大限制数量
        /// </summary>
        public int MaxNumber
        {
            get
            {
                return GetAttribute<int>("MaxNumber");
            }
            set
            {
                SetAttribute<int>("MaxNumber", value);
            }
        }

        /// <summary>
        /// 获取所有的跟踪策略
        /// </summary>
        /// <returns></returns>
        public async Task GetAllStrategies(Func<TrackerStrategy, Task> callback)
        {
            await _imp.GetAllStrategies(this, callback);
        }

        public async Task<ValidateResult> Access()
        {
            return await _imp.Access(this);
        }

        public async Task Exit()
        {
            await _imp.Exit(this);
        }
    }

    public interface IRequestTrackerIMP
    {
        Task GetAllStrategies(RequestTracker tracker, Func<TrackerStrategy, Task> callback);

        Task<ValidateResult> Access(RequestTracker tracker);
        Task Exit(RequestTracker tracker);
    }


    [Injection(InterfaceType = typeof(IRequestTrackerIMP), Scope = InjectionScope.Transient)]
    public class RequestTrackerIMP : IRequestTrackerIMP
    {
        private ITrackerStrategyRepository _trackerStrategyRepository;

        public RequestTrackerIMP(ITrackerStrategyRepository trackerStrategyRepository)
        {
            _trackerStrategyRepository = trackerStrategyRepository;
        }

        public async Task<ValidateResult> Access(RequestTracker tracker)
        {
            ValidateResult result = new ValidateResult()
            {
                Result = true
            };
            //获取所有的策略
            await GetAllStrategies(tracker, async (strategy) =>
            {
                //对每一个策略执行Access
                var strategyResult = await strategy.Access(tracker);
                if (!strategyResult.Result)
                {
                    result.Result = false;
                    result.Description = strategyResult.Description;
                }
            });
            return result;
        }

        public async Task Exit(RequestTracker tracker)
        {
            //获取所有的策略
            await GetAllStrategies(tracker, async (strategy) =>
            {
                //对每一个策略执行Exit
                await strategy.Exit(tracker);
            });
        }

        public async Task GetAllStrategies(RequestTracker tracker, Func<TrackerStrategy, Task> callback)
        {
            var strategies = (string[])tracker.Extensions["Strategies"];
            foreach (var item in strategies)
            {
                var strategy = await _trackerStrategyRepository.QueryByName(item);
                if (strategy != null)
                {
                    await callback(strategy);
                }
            }

            await Task.FromResult(0);
        }
    }



}
