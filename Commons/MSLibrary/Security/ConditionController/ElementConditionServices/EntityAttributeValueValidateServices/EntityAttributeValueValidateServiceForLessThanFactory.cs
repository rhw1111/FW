using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Security.ConditionController.ElementConditionServices.EntityAttributeValueValidateServices
{
    [Injection(InterfaceType = typeof(EntityAttributeValueValidateServiceForLessThanFactory), Scope = InjectionScope.Singleton)]
    public class EntityAttributeValueValidateServiceForLessThanFactory : IFactory<IEntityAttributeValueValidateService>
    {
        private EntityAttributeValueValidateServiceForLessThan  _entityAttributeValueValidateServiceForLessThan;

        public EntityAttributeValueValidateServiceForLessThanFactory(EntityAttributeValueValidateServiceForLessThan entityAttributeValueValidateServiceForLessThan)
        {
            _entityAttributeValueValidateServiceForLessThan = entityAttributeValueValidateServiceForLessThan;
        }
        public IEntityAttributeValueValidateService Create()
        {
            return _entityAttributeValueValidateServiceForLessThan;
        }
    }
}
