using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Security.ConditionController.ElementConditionServices.EntityAttributeValueValidateServices
{
    [Injection(InterfaceType = typeof(EntityAttributeValueValidateServiceForNotNullFactory), Scope = InjectionScope.Singleton)]
    public class EntityAttributeValueValidateServiceForNotNullFactory : IFactory<IEntityAttributeValueValidateService>
    {
        private EntityAttributeValueValidateServiceForNotNull _entityAttributeValueValidateServiceForNotNull;
        public EntityAttributeValueValidateServiceForNotNullFactory(EntityAttributeValueValidateServiceForNotNull entityAttributeValueValidateServiceForNotNull)
        {
            _entityAttributeValueValidateServiceForNotNull = entityAttributeValueValidateServiceForNotNull;
        }
        public IEntityAttributeValueValidateService Create()
        {
            return _entityAttributeValueValidateServiceForNotNull;
        }
    }
}
