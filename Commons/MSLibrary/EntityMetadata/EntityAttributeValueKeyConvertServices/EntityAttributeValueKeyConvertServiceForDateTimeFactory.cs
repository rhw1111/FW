using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.EntityMetadata.EntityAttributeValueKeyConvertServices
{
    [Injection(InterfaceType = typeof(EntityAttributeValueKeyConvertServiceForDateTimeFactory), Scope = InjectionScope.Singleton)]
    public class EntityAttributeValueKeyConvertServiceForDateTimeFactory : IFactory<IEntityAttributeValueKeyConvertService>
    {
        private EntityAttributeValueKeyConvertServiceForDateTime _entityAttributeValueKeyConvertServiceForDateTime;

        public EntityAttributeValueKeyConvertServiceForDateTimeFactory(EntityAttributeValueKeyConvertServiceForDateTime entityAttributeValueKeyConvertServiceForDateTime)
        {
            _entityAttributeValueKeyConvertServiceForDateTime = entityAttributeValueKeyConvertServiceForDateTime;
        }
        public IEntityAttributeValueKeyConvertService Create()
        {
            return _entityAttributeValueKeyConvertServiceForDateTime;
        }
    }
}
