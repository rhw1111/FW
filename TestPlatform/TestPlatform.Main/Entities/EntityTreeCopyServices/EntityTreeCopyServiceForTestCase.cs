using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.DI;

namespace FW.TestPlatform.Main.Entities.EntityTreeCopyServices
{
    [Injection(InterfaceType = typeof(EntityTreeCopyServiceForTestCase), Scope = InjectionScope.Singleton)]
    public class EntityTreeCopyServiceForTestCase : IEntityTreeCopyService
    {
        public Task Execute(string type, Guid entityID, Guid parentTreeID)
        {
            throw new NotImplementedException();
        }
    }
}
