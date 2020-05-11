using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Security.ConditionController.ConditionServices
{
    [Injection(InterfaceType = typeof(ElementConditionServiceForAndFactory), Scope = InjectionScope.Singleton)]
    public class ElementConditionServiceForAndFactory : IFactory<IElementConditionService>
    {
        private ElementConditionServiceForAnd _elementConditionServiceForAnd;

        public ElementConditionServiceForAndFactory(ElementConditionServiceForAnd elementConditionServiceForAnd)
        {
            _elementConditionServiceForAnd = elementConditionServiceForAnd;
        }
        public IElementConditionService Create()
        {
            return _elementConditionServiceForAnd;
        }
    }
}
