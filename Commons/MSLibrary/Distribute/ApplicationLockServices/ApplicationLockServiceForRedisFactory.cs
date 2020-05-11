using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Distribute.ApplicationLockServices
{
    [Injection(InterfaceType = typeof(ApplicationLockServiceForRedisFactory), Scope = InjectionScope.Singleton)]
    public class ApplicationLockServiceForRedisFactory : IFactory<IApplicationLockService>
    {
        private ApplicationLockServiceForRedis _applicationLockServiceForRedis;
        public ApplicationLockServiceForRedisFactory(ApplicationLockServiceForRedis applicationLockServiceForRedis)
        {
            _applicationLockServiceForRedis = applicationLockServiceForRedis;
        }
        public IApplicationLockService Create()
        {
            return _applicationLockServiceForRedis;
        }
    }
}
