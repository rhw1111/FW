using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;


namespace MSLibrary.EntityMetadata.EntityAttributeValueKeyConvertServices
{
    [Injection(InterfaceType = typeof(EntityAttributeValueKeyConvertServiceForIntNullAbleFactory), Scope = InjectionScope.Singleton)]
    public class EntityAttributeValueKeyConvertServiceForIntNullAbleFactory : IFactory<IEntityAttributeValueKeyConvertService>
    {
        private EntityAttributeValueKeyConvertServiceForIntNullAble _entityAttributeValueKeyConvertServiceForIntNullAble;
        public EntityAttributeValueKeyConvertServiceForIntNullAbleFactory(EntityAttributeValueKeyConvertServiceForIntNullAble entityAttributeValueKeyConvertServiceForIntNullAble)
        {
            _entityAttributeValueKeyConvertServiceForIntNullAble = entityAttributeValueKeyConvertServiceForIntNullAble;
        }
        public IEntityAttributeValueKeyConvertService Create()
        {
            return _entityAttributeValueKeyConvertServiceForIntNullAble;
        }
    }
}
