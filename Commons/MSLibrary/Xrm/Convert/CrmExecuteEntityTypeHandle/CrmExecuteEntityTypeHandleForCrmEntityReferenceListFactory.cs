using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Xrm.Convert.CrmExecuteEntityTypeHandle
{
    [Injection(InterfaceType = typeof(CrmExecuteEntityTypeHandleForCrmEntityReferenceListFactory), Scope = InjectionScope.Singleton)]
    public class CrmExecuteEntityTypeHandleForCrmEntityReferenceListFactory : IFactory<ICrmExecuteEntityTypeHandle>
    {
        private CrmExecuteEntityTypeHandleForCrmEntityReferenceList _crmExecuteEntityTypeHandleForCrmEntityReferenceList;

        public CrmExecuteEntityTypeHandleForCrmEntityReferenceListFactory(CrmExecuteEntityTypeHandleForCrmEntityReferenceList crmExecuteEntityTypeHandleForCrmEntityReferenceList)
        {
            _crmExecuteEntityTypeHandleForCrmEntityReferenceList = crmExecuteEntityTypeHandleForCrmEntityReferenceList;
        }
        public ICrmExecuteEntityTypeHandle Create()
        {
            return _crmExecuteEntityTypeHandleForCrmEntityReferenceList;
        }
    }
}
