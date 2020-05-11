using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.EntityMetadata.EntityAttributeValueKeyConvertServices
{
    [Injection(InterfaceType = typeof(EntityAttributeValueKeyConvertServiceForLongFactory), Scope = InjectionScope.Singleton)]
    public class EntityAttributeValueKeyConvertServiceForLongFactory : IFactory<IEntityAttributeValueKeyConvertService>
    {
        private EntityAttributeValueKeyConvertServiceForLong _entityAttributeValueKeyConvertServiceForLong;

        public EntityAttributeValueKeyConvertServiceForLongFactory(EntityAttributeValueKeyConvertServiceForLong entityAttributeValueKeyConvertServiceForLong)
        {
            _entityAttributeValueKeyConvertServiceForLong = entityAttributeValueKeyConvertServiceForLong;
        }
        public IEntityAttributeValueKeyConvertService Create()
        {
            return _entityAttributeValueKeyConvertServiceForLong;
        }
    }
}
