using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Security.RequestController
{
    /// <summary>
    /// 跟踪策略仓储
    /// </summary>
    public interface ITrackerStrategyRepository
    {
        Task<TrackerStrategy> QueryByName(string name);
    }
}
