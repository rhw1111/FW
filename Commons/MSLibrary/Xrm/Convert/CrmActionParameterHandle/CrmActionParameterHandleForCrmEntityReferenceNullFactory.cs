using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Xrm.Convert.CrmActionParameterHandle
{
    [Injection(InterfaceType = typeof(CrmActionParameterHandleForCrmEntityReferenceNullFactory), Scope = InjectionScope.Singleton)]
    public class CrmActionParameterHandleForCrmEntityReferenceNullFactory : IFactory<ICrmActionParameterHandle>
    {
        private CrmActionParameterHandleForCrmEntityReferenceNull _crmActionParameterHandleForCrmEntityReferenceNull;

        public CrmActionParameterHandleForCrmEntityReferenceNullFactory(CrmActionParameterHandleForCrmEntityReferenceNull crmActionParameterHandleForCrmEntityReferenceNull)
        {
            _crmActionParameterHandleForCrmEntityReferenceNull = crmActionParameterHandleForCrmEntityReferenceNull;
        }
        public ICrmActionParameterHandle Create()
        {
            return _crmActionParameterHandleForCrmEntityReferenceNull;
        }
    }
}
