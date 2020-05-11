using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.EntityMetadata.EntityAttributeValueKeyConvertServices
{
    [Injection(InterfaceType = typeof(EntityAttributeValueKeyConvertServiceForDecimalFactory), Scope = InjectionScope.Singleton)]
    public class EntityAttributeValueKeyConvertServiceForDecimalFactory : IFactory<IEntityAttributeValueKeyConvertService>
    {
        private EntityAttributeValueKeyConvertServiceForDecimal _entityAttributeValueKeyConvertServiceForDecimal;

        public EntityAttributeValueKeyConvertServiceForDecimalFactory(EntityAttributeValueKeyConvertServiceForDecimal entityAttributeValueKeyConvertServiceForDecimal)
        {
            _entityAttributeValueKeyConvertServiceForDecimal = entityAttributeValueKeyConvertServiceForDecimal;
        }
        public IEntityAttributeValueKeyConvertService Create()
        {
            return _entityAttributeValueKeyConvertServiceForDecimal;
        }
    }
}
