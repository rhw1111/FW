using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Security.RequestController.TrackerStrategyServices
{
    [Injection(InterfaceType = typeof(TrackerStrategyServiceForTokenBucketFactory), Scope = InjectionScope.Singleton)]
    public class TrackerStrategyServiceForTokenBucketFactory : IFactory<ITrackerStrategyService>
    {
        private TrackerStrategyServiceForTokenBucket _trackerStrategyServiceForTokenBucket;

        public TrackerStrategyServiceForTokenBucketFactory(TrackerStrategyServiceForTokenBucket trackerStrategyServiceForTokenBucket)
        {
            _trackerStrategyServiceForTokenBucket = trackerStrategyServiceForTokenBucket;
        }
        public ITrackerStrategyService Create()
        {
            return _trackerStrategyServiceForTokenBucket;
        }
    }
}
