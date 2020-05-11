using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;

namespace MSLibrary.Distribute.ApplicationLimitServices
{
    [Injection(InterfaceType = typeof(ApplicationLimitServiceForRedisTokenFactory), Scope = InjectionScope.Singleton)]
    public class ApplicationLimitServiceForRedisTokenFactory : IFactory<IApplicationLimitService>
    {
        private ApplicationLimitServiceForRedisToken _applicationLimitServiceForRedisToken;

        public ApplicationLimitServiceForRedisTokenFactory(ApplicationLimitServiceForRedisToken applicationLimitServiceForRedisToken)
        {
            _applicationLimitServiceForRedisToken = applicationLimitServiceForRedisToken;
        }

        public IApplicationLimitService Create()
        {
            return _applicationLimitServiceForRedisToken;
        }
    }
}
