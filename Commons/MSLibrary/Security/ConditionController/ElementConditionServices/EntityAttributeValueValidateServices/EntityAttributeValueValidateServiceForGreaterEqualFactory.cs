using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Security.ConditionController.ElementConditionServices.EntityAttributeValueValidateServices
{
    [Injection(InterfaceType = typeof(EntityAttributeValueValidateServiceForGreaterEqualFactory), Scope = InjectionScope.Singleton)]
    public class EntityAttributeValueValidateServiceForGreaterEqualFactory : IFactory<IEntityAttributeValueValidateService>
    {
        private EntityAttributeValueValidateServiceForGreaterEqual _entityAttributeValueValidateServiceForGreaterEqual;

        public EntityAttributeValueValidateServiceForGreaterEqualFactory(EntityAttributeValueValidateServiceForGreaterEqual entityAttributeValueValidateServiceForGreaterEqual)
        {
            _entityAttributeValueValidateServiceForGreaterEqual = entityAttributeValueValidateServiceForGreaterEqual;
        }
        public IEntityAttributeValueValidateService Create()
        {
            return _entityAttributeValueValidateServiceForGreaterEqual;
        }
    }
}
