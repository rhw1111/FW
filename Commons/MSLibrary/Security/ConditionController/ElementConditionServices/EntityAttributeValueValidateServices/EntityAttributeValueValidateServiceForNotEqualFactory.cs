using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Security.ConditionController.ElementConditionServices.EntityAttributeValueValidateServices
{
    [Injection(InterfaceType = typeof(EntityAttributeValueValidateServiceForNotEqualFactory), Scope = InjectionScope.Singleton)]
    public class EntityAttributeValueValidateServiceForNotEqualFactory : IFactory<IEntityAttributeValueValidateService>
    {
        private EntityAttributeValueValidateServiceForNotEqual _entityAttributeValueValidateServiceForNotEqual;
        public EntityAttributeValueValidateServiceForNotEqualFactory(EntityAttributeValueValidateServiceForNotEqual entityAttributeValueValidateServiceForNotEqual)
        {
            _entityAttributeValueValidateServiceForNotEqual = entityAttributeValueValidateServiceForNotEqual;
        }

        public IEntityAttributeValueValidateService Create()
        {
            return _entityAttributeValueValidateServiceForNotEqual;
        }
    }
}
