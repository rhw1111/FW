using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.EntityMetadata.EntityAttributeValueKeyConvertServices
{
    [Injection(InterfaceType = typeof(EntityAttributeValueKeyConvertServiceForGuidNullAbleFactory), Scope = InjectionScope.Singleton)]
    public class EntityAttributeValueKeyConvertServiceForGuidNullAbleFactory : IFactory<IEntityAttributeValueKeyConvertService>
    {
        private EntityAttributeValueKeyConvertServiceForGuidNullAble _entityAttributeValueKeyConvertServiceForGuidNullAble;
        public EntityAttributeValueKeyConvertServiceForGuidNullAbleFactory(EntityAttributeValueKeyConvertServiceForGuidNullAble entityAttributeValueKeyConvertServiceForGuidNullAble)
        {
            _entityAttributeValueKeyConvertServiceForGuidNullAble = entityAttributeValueKeyConvertServiceForGuidNullAble;
        }
        public IEntityAttributeValueKeyConvertService Create()
        {
            return _entityAttributeValueKeyConvertServiceForGuidNullAble;
        }
    }
}
