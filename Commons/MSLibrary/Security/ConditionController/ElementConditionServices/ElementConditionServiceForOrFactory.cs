using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Security.ConditionController.ConditionServices
{
    [Injection(InterfaceType = typeof(ElementConditionServiceForOrFactory), Scope = InjectionScope.Singleton)]
    public class ElementConditionServiceForOrFactory : IFactory<IElementConditionService>
    {
        private ElementConditionServiceForOr _conditionServiceForOr;

        public ElementConditionServiceForOrFactory(ElementConditionServiceForOr conditionServiceForOr)
        {
            _conditionServiceForOr = conditionServiceForOr;
        }

        public IElementConditionService Create()
        {
            return _conditionServiceForOr;
        }
    }
}
