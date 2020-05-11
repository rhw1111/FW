using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Security.ConditionController.ElementConditionServices
{
    [Injection(InterfaceType = typeof(ElementConditionServiceForEntityAttributeFactory), Scope = InjectionScope.Singleton)]
    public class ElementConditionServiceForEntityAttributeFactory : IFactory<IElementConditionService>
    {
        private ElementConditionServiceForEntityAttribute _elementConditionServiceForEntityAttribute;

        public ElementConditionServiceForEntityAttributeFactory(ElementConditionServiceForEntityAttribute elementConditionServiceForEntityAttribute)
        {
            _elementConditionServiceForEntityAttribute = elementConditionServiceForEntityAttribute;
        }
        public IElementConditionService Create()
        {
            return _elementConditionServiceForEntityAttribute;
        }
    }
}
