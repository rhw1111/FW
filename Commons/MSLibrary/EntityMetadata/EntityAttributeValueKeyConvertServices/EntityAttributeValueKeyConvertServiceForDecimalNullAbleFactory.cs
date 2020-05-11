using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.EntityMetadata.EntityAttributeValueKeyConvertServices
{
    [Injection(InterfaceType = typeof(EntityAttributeValueKeyConvertServiceForDecimalNullAbleFactory), Scope = InjectionScope.Singleton)]
    public class EntityAttributeValueKeyConvertServiceForDecimalNullAbleFactory : IFactory<IEntityAttributeValueKeyConvertService>
    {
        private EntityAttributeValueKeyConvertServiceForDecimalNullAble _entityAttributeValueKeyConvertServiceForDecimalNullAble;

        public EntityAttributeValueKeyConvertServiceForDecimalNullAbleFactory(EntityAttributeValueKeyConvertServiceForDecimalNullAble entityAttributeValueKeyConvertServiceForDecimalNullAble)
        {
            _entityAttributeValueKeyConvertServiceForDecimalNullAble = entityAttributeValueKeyConvertServiceForDecimalNullAble;
        }
        public IEntityAttributeValueKeyConvertService Create()
        {
            return _entityAttributeValueKeyConvertServiceForDecimalNullAble;
        }
    }
}
