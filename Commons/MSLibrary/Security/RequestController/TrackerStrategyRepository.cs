using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Extensions.Primitives;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Configuration;
using MSLibrary.DI;
using MSLibrary.Serializer;
using MSLibrary.Configuration;


namespace MSLibrary.Security.RequestController
{
    /// <summary>
    /// 跟踪策略仓储
    /// </summary>
    [Injection(InterfaceType = typeof(ITrackerStrategyRepository), Scope = InjectionScope.Singleton)]
    public class TrackerStrategyRepository : ITrackerStrategyRepository
    {
        private static Dictionary<string, TrackerStrategy> _trackerStrategyList =null;

        static TrackerStrategyRepository()
        {

            ConfigurationContainer.RegisterJsonListener<string[]>("TrackerStrategyData.json", (names) =>
            {
                var trackerStrategyList = new Dictionary<string, TrackerStrategy>();

                foreach (var item in names)
                {
                    trackerStrategyList.Add(item, new TrackerStrategy()
                    {
                        ID = Guid.NewGuid(),
                        Name = item
                    });
                }
                _trackerStrategyList = trackerStrategyList;
            });

        }

        /// <summary>
        /// 根据名称获取跟踪策略
        /// 这里的实现是直接创建跟踪策略
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<TrackerStrategy> QueryByName(string name)
        {
            TrackerStrategy strategy = new TrackerStrategy()
            {
                ID = Guid.NewGuid(),
                Name = name
            };

            return await Task.FromResult(strategy);
        }
    }
}
