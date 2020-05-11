using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Security.ConditionController.ElementConditionServices.EntityAttributeValueValidateServices
{
    [Injection(InterfaceType = typeof(EntityAttributeValueValidateServiceForEqualFactory), Scope = InjectionScope.Singleton)]
    public class EntityAttributeValueValidateServiceForEqualFactory : IFactory<IEntityAttributeValueValidateService>
    {
        private EntityAttributeValueValidateServiceForEqual _entityAttributeValueValidateServiceForEqual;
        public EntityAttributeValueValidateServiceForEqualFactory(EntityAttributeValueValidateServiceForEqual entityAttributeValueValidateServiceForEqual)
        {
            _entityAttributeValueValidateServiceForEqual = entityAttributeValueValidateServiceForEqual;
        }
        public IEntityAttributeValueValidateService Create()
        {
            return _entityAttributeValueValidateServiceForEqual;
        }
    }
}
