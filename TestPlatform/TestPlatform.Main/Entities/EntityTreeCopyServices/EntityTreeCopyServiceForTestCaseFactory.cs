using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary;
using MSLibrary.DI;

namespace FW.TestPlatform.Main.Entities.EntityTreeCopyServices
{
    [Injection(InterfaceType = typeof(EntityTreeCopyServiceForTestCaseFactory), Scope = InjectionScope.Singleton)]
    public class EntityTreeCopyServiceForTestCaseFactory : IFactory<IEntityTreeCopyService>
    {
        private readonly EntityTreeCopyServiceForTestCase _entityTreeCopyServiceForTestCase;

        public EntityTreeCopyServiceForTestCaseFactory(EntityTreeCopyServiceForTestCase entityTreeCopyServiceForTestCase)
        {
            _entityTreeCopyServiceForTestCase = entityTreeCopyServiceForTestCase;
        }
        public IEntityTreeCopyService Create()
        {
            return _entityTreeCopyServiceForTestCase;
        }
    }
}
