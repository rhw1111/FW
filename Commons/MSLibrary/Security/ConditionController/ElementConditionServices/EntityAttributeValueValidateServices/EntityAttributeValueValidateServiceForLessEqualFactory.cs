using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Security.ConditionController.ElementConditionServices.EntityAttributeValueValidateServices
{
    [Injection(InterfaceType = typeof(EntityAttributeValueValidateServiceForLessEqualFactory), Scope = InjectionScope.Singleton)]
    public class EntityAttributeValueValidateServiceForLessEqualFactory : IFactory<IEntityAttributeValueValidateService>
    {
        private EntityAttributeValueValidateServiceForLessEqual _entityAttributeValueValidateServiceForLessEqual;
        public EntityAttributeValueValidateServiceForLessEqualFactory(EntityAttributeValueValidateServiceForLessEqual entityAttributeValueValidateServiceForLessEqual)
        {
            _entityAttributeValueValidateServiceForLessEqual = entityAttributeValueValidateServiceForLessEqual;
        }
        public IEntityAttributeValueValidateService Create()
        {
            return _entityAttributeValueValidateServiceForLessEqual;
        }
    }
}
