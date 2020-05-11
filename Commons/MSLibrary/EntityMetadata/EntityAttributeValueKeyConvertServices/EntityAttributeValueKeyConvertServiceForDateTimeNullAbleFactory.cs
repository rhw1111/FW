using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.EntityMetadata.EntityAttributeValueKeyConvertServices
{
    [Injection(InterfaceType = typeof(EntityAttributeValueKeyConvertServiceForDateTimeNullAbleFactory), Scope = InjectionScope.Singleton)]
    public class EntityAttributeValueKeyConvertServiceForDateTimeNullAbleFactory : IFactory<IEntityAttributeValueKeyConvertService>
    {
        private EntityAttributeValueKeyConvertServiceForDateTimeNullAble _entityAttributeValueKeyConvertServiceForDateTimeNullAble;

        public EntityAttributeValueKeyConvertServiceForDateTimeNullAbleFactory(EntityAttributeValueKeyConvertServiceForDateTimeNullAble entityAttributeValueKeyConvertServiceForDateTimeNullAble)
        {
            _entityAttributeValueKeyConvertServiceForDateTimeNullAble = entityAttributeValueKeyConvertServiceForDateTimeNullAble;
        }
        public IEntityAttributeValueKeyConvertService Create()
        {
            return _entityAttributeValueKeyConvertServiceForDateTimeNullAble;
        }
    }
}
