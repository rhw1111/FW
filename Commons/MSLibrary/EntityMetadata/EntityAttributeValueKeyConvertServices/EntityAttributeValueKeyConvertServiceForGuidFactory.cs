using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.EntityMetadata.EntityAttributeValueKeyConvertServices
{
    [Injection(InterfaceType = typeof(EntityAttributeValueKeyConvertServiceForGuidFactory), Scope = InjectionScope.Singleton)]
    public class EntityAttributeValueKeyConvertServiceForGuidFactory : IFactory<IEntityAttributeValueKeyConvertService>
    {
        private EntityAttributeValueKeyConvertServiceForGuid _entityAttributeValueKeyConvertServiceForGuid;

        public EntityAttributeValueKeyConvertServiceForGuidFactory(EntityAttributeValueKeyConvertServiceForGuid entityAttributeValueKeyConvertServiceForGuid)
        {
            _entityAttributeValueKeyConvertServiceForGuid = entityAttributeValueKeyConvertServiceForGuid;
        }
        public IEntityAttributeValueKeyConvertService Create()
        {
            return _entityAttributeValueKeyConvertServiceForGuid;
        }
    }
}
