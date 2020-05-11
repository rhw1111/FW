using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.EntityMetadata.EntityAttributeValueKeyConvertServices
{
    [Injection(InterfaceType = typeof(EntityAttributeValueKeyConvertServiceForBoolFactory), Scope = InjectionScope.Singleton)]
    public class EntityAttributeValueKeyConvertServiceForBoolFactory : IFactory<IEntityAttributeValueKeyConvertService>
    {
        private EntityAttributeValueKeyConvertServiceForBool _entityAttributeValueKeyConvertServiceForBool;

        public EntityAttributeValueKeyConvertServiceForBoolFactory(EntityAttributeValueKeyConvertServiceForBool entityAttributeValueKeyConvertServiceForBool)
        {
            _entityAttributeValueKeyConvertServiceForBool = entityAttributeValueKeyConvertServiceForBool;
        }
        public IEntityAttributeValueKeyConvertService Create()
        {
            return _entityAttributeValueKeyConvertServiceForBool;
        }
    }
}
