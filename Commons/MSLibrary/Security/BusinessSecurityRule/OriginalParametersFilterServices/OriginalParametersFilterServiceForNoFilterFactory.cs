using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Security.BusinessSecurityRule.OriginalParametersFilterServices
{
    [Injection(InterfaceType = typeof(OriginalParametersFilterServiceForNoFilterFactory), Scope = InjectionScope.Singleton)]
    public class OriginalParametersFilterServiceForNoFilterFactory : IFactory<IOriginalParametersFilterService>
    {
        private OriginalParametersFilterServiceForNoFilter _originalParametersFilterServiceForNoFilter;

        public OriginalParametersFilterServiceForNoFilterFactory(OriginalParametersFilterServiceForNoFilter originalParametersFilterServiceForNoFilter)
        {
            _originalParametersFilterServiceForNoFilter = originalParametersFilterServiceForNoFilter;
        }
        public IOriginalParametersFilterService Create()
        {
            return _originalParametersFilterServiceForNoFilter;
        }
    }
}
