using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;

namespace MSLibrary.Security.ConditionController.ElementConditionServices.EntityAttributeValueValidateServices
{
    [Injection(InterfaceType = typeof(EntityAttributeValueValidateServiceForNullFactory), Scope = InjectionScope.Singleton)]
    public class EntityAttributeValueValidateServiceForNullFactory : IFactory<IEntityAttributeValueValidateService>
    {
        private EntityAttributeValueValidateServiceForNull _entityAttributeValueValidateServiceForNull;

        public EntityAttributeValueValidateServiceForNullFactory(EntityAttributeValueValidateServiceForNull entityAttributeValueValidateServiceForNull)
        {
            _entityAttributeValueValidateServiceForNull = entityAttributeValueValidateServiceForNull;
        }

        public IEntityAttributeValueValidateService Create()
        {
            return _entityAttributeValueValidateServiceForNull;
        }
    }
}
