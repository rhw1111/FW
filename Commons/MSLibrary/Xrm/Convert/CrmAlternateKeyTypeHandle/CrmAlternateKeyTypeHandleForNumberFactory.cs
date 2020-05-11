using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Xrm.Convert.CrmAlternateKeyTypeHandle
{
    [Injection(InterfaceType = typeof(CrmAlternateKeyTypeHandleForNumberFactory), Scope = InjectionScope.Singleton)]
    public class CrmAlternateKeyTypeHandleForNumberFactory : IFactory<ICrmAlternateKeyTypeHandle>
    {
        private CrmAlternateKeyTypeHandleForNumber _crmAlternateKeyTypeHandleForNumber;

        public CrmAlternateKeyTypeHandleForNumberFactory(CrmAlternateKeyTypeHandleForNumber crmAlternateKeyTypeHandleForNumber)
        {
            _crmAlternateKeyTypeHandleForNumber = crmAlternateKeyTypeHandleForNumber;
        }
        public ICrmAlternateKeyTypeHandle Create()
        {
            return _crmAlternateKeyTypeHandleForNumber;
        }
    }
}
