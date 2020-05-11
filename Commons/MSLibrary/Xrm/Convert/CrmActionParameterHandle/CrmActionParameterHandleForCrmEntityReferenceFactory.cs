using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Xrm.Convert.CrmActionParameterHandle
{
    [Injection(InterfaceType = typeof(CrmActionParameterHandleForCrmEntityReferenceFactory), Scope = InjectionScope.Singleton)]
    public class CrmActionParameterHandleForCrmEntityReferenceFactory : IFactory<ICrmActionParameterHandle>
    {
        private CrmActionParameterHandleForCrmEntityReference _crmActionParameterHandleForCrmEntityReference;

        public CrmActionParameterHandleForCrmEntityReferenceFactory(CrmActionParameterHandleForCrmEntityReference crmActionParameterHandleForCrmEntityReference)
        {
            _crmActionParameterHandleForCrmEntityReference = crmActionParameterHandleForCrmEntityReference;
        }
        public ICrmActionParameterHandle Create()
        {
            return _crmActionParameterHandleForCrmEntityReference;
        }
    }
}
