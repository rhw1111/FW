using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Xrm.Convert.CrmActionParameterHandle
{
    [Injection(InterfaceType = typeof(CrmActionParameterHandleForCrmEntityReferenceListFactory), Scope = InjectionScope.Singleton)]
    public class CrmActionParameterHandleForCrmEntityReferenceListFactory : IFactory<ICrmActionParameterHandle>
    {
        private CrmActionParameterHandleForCrmEntityReferenceList _crmActionParameterHandleForCrmEntityReferenceList;

        public CrmActionParameterHandleForCrmEntityReferenceListFactory(CrmActionParameterHandleForCrmEntityReferenceList crmActionParameterHandleForCrmEntityReferenceList)
        {
            _crmActionParameterHandleForCrmEntityReferenceList = crmActionParameterHandleForCrmEntityReferenceList;
        }
        public ICrmActionParameterHandle Create()
        {
            return _crmActionParameterHandleForCrmEntityReferenceList;
        }
    }
}
