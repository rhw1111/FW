using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Xrm.Convert.CrmExecuteEntityTypeHandle
{
    [Injection(InterfaceType = typeof(CrmExecuteEntityTypeHandleForCrmEntityReferenceFactory), Scope = InjectionScope.Singleton)]
    public class CrmExecuteEntityTypeHandleForCrmEntityReferenceFactory : IFactory<ICrmExecuteEntityTypeHandle>
    {
        private CrmExecuteEntityTypeHandleForCrmEntityReference _crmExecuteEntityTypeHandleForCrmEntityReference;

        public CrmExecuteEntityTypeHandleForCrmEntityReferenceFactory(CrmExecuteEntityTypeHandleForCrmEntityReference crmExecuteEntityTypeHandleForCrmEntityReference)
        {
            _crmExecuteEntityTypeHandleForCrmEntityReference = crmExecuteEntityTypeHandleForCrmEntityReference;
        }
        public ICrmExecuteEntityTypeHandle Create()
        {
            return _crmExecuteEntityTypeHandleForCrmEntityReference;
        }
    }
}
