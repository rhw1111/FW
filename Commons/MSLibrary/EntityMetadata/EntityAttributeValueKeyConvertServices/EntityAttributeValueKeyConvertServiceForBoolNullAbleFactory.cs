using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.EntityMetadata.EntityAttributeValueKeyConvertServices
{
    [Injection(InterfaceType = typeof(EntityAttributeValueKeyConvertServiceForBoolNullAbleFactory), Scope = InjectionScope.Singleton)]
    public class EntityAttributeValueKeyConvertServiceForBoolNullAbleFactory : IFactory<IEntityAttributeValueKeyConvertService>
    {
        private EntityAttributeValueKeyConvertServiceForBoolNullAble _entityAttributeValueKeyConvertServiceForBoolNullAble;
        public EntityAttributeValueKeyConvertServiceForBoolNullAbleFactory(EntityAttributeValueKeyConvertServiceForBoolNullAble entityAttributeValueKeyConvertServiceForBoolNullAble)
        {
            _entityAttributeValueKeyConvertServiceForBoolNullAble = entityAttributeValueKeyConvertServiceForBoolNullAble;
        }
        public IEntityAttributeValueKeyConvertService Create()
        {
            return _entityAttributeValueKeyConvertServiceForBoolNullAble;
        }
    }
}
