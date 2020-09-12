using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.Cache;
using MSLibrary.DI;

namespace MSLibrary.MessageQueue
{
    /// <summary>
    /// 队列仓储帮助类
    /// 提供缓存服务
    /// 简化需要缓存服务的调用方使用
    /// </summary>
    [Injection(InterfaceType = typeof(SQueueRepositoryHelper), Scope = InjectionScope.Singleton)]
    public class SQueueRepositoryHelper
    {
        private ISQueueRepository _sQueueRepository;


        private static int _cacheSize = 2000;

        /// <summary>
        /// 缓存数量
        /// 默认2000
        /// </summary>
        public static int CacheSize
        {
            get
            {
                return _cacheSize;
            }
            set
            {
                _cacheSize = value;
                _queuesByCode.Length = value;
                _queueCountByGroup.Length = value;
            }
        }
        /// <summary>
        /// 缓存时间
        /// 默认600秒
        /// </summary>
        public static int CacheTimeout { get; set; } = 600;

        /// <summary>
        /// 清除缓存
        /// </summary>
        public static bool Refreash
        {
            set
            {
                _queuesByCode.Clear();
                _queueCountByGroup.Clear();
            }
        }

        private static HashLinkedCache<string, CacheTimeContainer<SQueue>> _queuesByCode = new HashLinkedCache<string, CacheTimeContainer<SQueue>>() { Length = CacheSize };

        private static HashLinkedCache<string, CacheTimeContainer<int>> _queueCountByGroup = new HashLinkedCache<string, CacheTimeContainer<int>>() { Length = CacheSize };



        public SQueueRepositoryHelper(ISQueueRepository sQueueRepository)
        {
            _sQueueRepository = sQueueRepository;
        }

        public async Task<SQueue> QueryByCode(string groupName, bool isDead, int code)
        {
            var key = $"{groupName}-{Convert.ToInt32(isDead).ToString()}-{code.ToString()}";
            CacheTimeContainer<SQueue> queueItem = _queuesByCode.GetValue(key);
            if (queueItem == null || queueItem.Expire())
            {
                var squeue = await _sQueueRepository.QueryByCode(groupName, isDead, code);
                queueItem = new CacheTimeContainer<SQueue>(squeue, CacheTimeout,0);
                _queuesByCode.SetValue(key, queueItem);
            }

            return queueItem.Value;
        }

        public async Task<int> QueryGroupCount(string groupName, bool isDead)
        {
            var key = $"{groupName}-{Convert.ToInt32(isDead).ToString()}";
            CacheTimeContainer<int> countItem = _queueCountByGroup.GetValue(key); ;
            if (countItem == null || countItem.Expire())
            {
                var queryResult = await _sQueueRepository.QueryByGroup(groupName, isDead, 1,1);
                countItem = new CacheTimeContainer<int>(queryResult.TotalCount, CacheTimeout,0);
                _queueCountByGroup.SetValue(key, countItem);
            }

            return countItem.Value;
        }
    }
}
