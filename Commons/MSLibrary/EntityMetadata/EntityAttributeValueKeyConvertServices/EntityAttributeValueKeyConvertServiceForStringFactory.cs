using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.EntityMetadata.EntityAttributeValueKeyConvertServices
{
    [Injection(InterfaceType = typeof(EntityAttributeValueKeyConvertServiceForStringFactory), Scope = InjectionScope.Singleton)]
    public class EntityAttributeValueKeyConvertServiceForStringFactory : IFactory<IEntityAttributeValueKeyConvertService>
    {
        private EntityAttributeValueKeyConvertServiceForString _entityAttributeValueKeyConvertServiceForString;

        public EntityAttributeValueKeyConvertServiceForStringFactory(EntityAttributeValueKeyConvertServiceForString entityAttributeValueKeyConvertServiceForString)
        {
            _entityAttributeValueKeyConvertServiceForString = entityAttributeValueKeyConvertServiceForString;
        }
        public IEntityAttributeValueKeyConvertService Create()
        {
            return _entityAttributeValueKeyConvertServiceForString;
        }
    }
}
