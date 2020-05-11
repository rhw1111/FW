using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;

namespace MSLibrary.Coroutine
{
    [Injection(InterfaceType = typeof(ICoroutineContainerRepository), Scope = InjectionScope.Singleton)]
    public class CoroutineContainerRepository : ICoroutineContainerRepository
    {
        public async Task<CoroutineContainer> QueryByName(string name)
        {
            CoroutineContainer container = new CoroutineContainer()
            {
                 Name=name
            };

            return await Task.FromResult(container);
        }
    }
}
