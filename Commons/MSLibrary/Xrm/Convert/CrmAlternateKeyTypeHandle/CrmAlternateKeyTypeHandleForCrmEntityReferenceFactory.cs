using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Xrm.Convert.CrmAlternateKeyTypeHandle
{
    [Injection(InterfaceType = typeof(CrmAlternateKeyTypeHandleForCrmEntityReferenceFactory), Scope = InjectionScope.Singleton)]
    public class CrmAlternateKeyTypeHandleForCrmEntityReferenceFactory : IFactory<ICrmAlternateKeyTypeHandle>
    {
        private CrmAlternateKeyTypeHandleForCrmEntityReference _crmAlternateKeyTypeHandleForCrmEntityReference;

        public CrmAlternateKeyTypeHandleForCrmEntityReferenceFactory(CrmAlternateKeyTypeHandleForCrmEntityReference crmAlternateKeyTypeHandleForCrmEntityReference)
        {
            _crmAlternateKeyTypeHandleForCrmEntityReference = crmAlternateKeyTypeHandleForCrmEntityReference;
        }
        public ICrmAlternateKeyTypeHandle Create()
        {
            return _crmAlternateKeyTypeHandleForCrmEntityReference;
        }
    }
}
