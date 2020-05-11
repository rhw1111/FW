using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Security.ConditionController.ElementConditionServices.EntityAttributeValueValidateServices
{
    [Injection(InterfaceType = typeof(EntityAttributeValueValidateServiceForGreaterThanFactory), Scope = InjectionScope.Singleton)]
    public class EntityAttributeValueValidateServiceForGreaterThanFactory : IFactory<IEntityAttributeValueValidateService>
    {
        private EntityAttributeValueValidateServiceForGreaterThan _entityAttributeValueValidateServiceForGreaterThan;
        public EntityAttributeValueValidateServiceForGreaterThanFactory(EntityAttributeValueValidateServiceForGreaterThan entityAttributeValueValidateServiceForGreaterThan)
        {
            _entityAttributeValueValidateServiceForGreaterThan = entityAttributeValueValidateServiceForGreaterThan;
        }
        public IEntityAttributeValueValidateService Create()
        {
            return _entityAttributeValueValidateServiceForGreaterThan;
        }
    }
}
