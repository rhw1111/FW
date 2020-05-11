using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Xrm.Convert.CrmExecuteEntityTypeHandle
{
    [Injection(InterfaceType = typeof(CrmExecuteEntityTypeHandleForCrmEntityReferenceNullFactory), Scope = InjectionScope.Singleton)]
    public class CrmExecuteEntityTypeHandleForCrmEntityReferenceNullFactory : IFactory<ICrmExecuteEntityTypeHandle>
    {
        private CrmExecuteEntityTypeHandleForCrmEntityReferenceNull _crmExecuteEntityTypeHandleForCrmEntityReferenceNull;

        public CrmExecuteEntityTypeHandleForCrmEntityReferenceNullFactory(CrmExecuteEntityTypeHandleForCrmEntityReferenceNull crmExecuteEntityTypeHandleForCrmEntityReferenceNull)
        {
            _crmExecuteEntityTypeHandleForCrmEntityReferenceNull = crmExecuteEntityTypeHandleForCrmEntityReferenceNull;
        }
        public ICrmExecuteEntityTypeHandle Create()
        {
            return _crmExecuteEntityTypeHandleForCrmEntityReferenceNull;
        }
    }
}
