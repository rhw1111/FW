using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.EntityMetadata.EntityAttributeValueKeyConvertServices
{
    [Injection(InterfaceType = typeof(EntityAttributeValueKeyConvertServiceForIntFactory), Scope = InjectionScope.Singleton)]
    public class EntityAttributeValueKeyConvertServiceForIntFactory : IFactory<IEntityAttributeValueKeyConvertService>
    {
        private EntityAttributeValueKeyConvertServiceForInt _entityAttributeValueKeyConvertServiceForInt;

        public EntityAttributeValueKeyConvertServiceForIntFactory(EntityAttributeValueKeyConvertServiceForInt entityAttributeValueKeyConvertServiceForInt)
        {
            _entityAttributeValueKeyConvertServiceForInt = entityAttributeValueKeyConvertServiceForInt;
        }
        public IEntityAttributeValueKeyConvertService Create()
        {
            return _entityAttributeValueKeyConvertServiceForInt;
        }
    }
}
