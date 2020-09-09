using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary;
using MSLibrary.DI;

namespace FW.TestPlatform.Main.Entities.EntityTreeCopyServices
{
    [Injection(InterfaceType = typeof(EntityTreeCopyServiceForTestDataSourceFactory), Scope = InjectionScope.Singleton)]
    public class EntityTreeCopyServiceForTestDataSourceFactory : IFactory<IEntityTreeCopyService>
    {
        private readonly EntityTreeCopyServiceForTestDataSource _entityTreeCopyServiceForTestCase;

        public EntityTreeCopyServiceForTestDataSourceFactory(EntityTreeCopyServiceForTestDataSource entityTreeCopyServiceForTestDataSource)
        {
            _entityTreeCopyServiceForTestCase = entityTreeCopyServiceForTestDataSource;
        }
        public IEntityTreeCopyService Create()
        {
            return _entityTreeCopyServiceForTestCase;
        }
    }
}
