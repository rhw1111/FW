using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.EntityMetadata.EntityAttributeValueKeyConvertServices
{

    [Injection(InterfaceType = typeof(EntityAttributeValueKeyConvertServiceForLongNullAbleFactory), Scope = InjectionScope.Singleton)]
    public class EntityAttributeValueKeyConvertServiceForLongNullAbleFactory : IFactory<IEntityAttributeValueKeyConvertService>
    {
        private EntityAttributeValueKeyConvertServiceForLongNullAble _entityAttributeValueKeyConvertServiceForLongNullAble;
        public EntityAttributeValueKeyConvertServiceForLongNullAbleFactory(EntityAttributeValueKeyConvertServiceForLongNullAble entityAttributeValueKeyConvertServiceForLongNullAble)
        {
            _entityAttributeValueKeyConvertServiceForLongNullAble = entityAttributeValueKeyConvertServiceForLongNullAble;
        }
        public IEntityAttributeValueKeyConvertService Create()
        {
            return _entityAttributeValueKeyConvertServiceForLongNullAble;
        }
    }
}
