using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Coroutine.CoroutineContainerServices
{
    [Injection(InterfaceType = typeof(CoroutineServiceForCurrentThreadFactory), Scope = InjectionScope.Singleton)]
    public class CoroutineServiceForCurrentThreadFactory : IFactory<ICoroutineService>
    {
        public ICoroutineService Create()
        {
            return DIContainerContainer.Get<CoroutineServiceForCurrentThread>();
        }
    }
}
