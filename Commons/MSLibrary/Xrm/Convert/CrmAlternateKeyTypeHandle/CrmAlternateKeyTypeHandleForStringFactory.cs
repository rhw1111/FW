using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Xrm.Convert.CrmAlternateKeyTypeHandle
{
    [Injection(InterfaceType = typeof(CrmAlternateKeyTypeHandleForStringFactory), Scope = InjectionScope.Singleton)]
    public class CrmAlternateKeyTypeHandleForStringFactory : IFactory<ICrmAlternateKeyTypeHandle>
    {
        private CrmAlternateKeyTypeHandleForString _crmAlternateKeyTypeHandleForString;

        public CrmAlternateKeyTypeHandleForStringFactory(CrmAlternateKeyTypeHandleForString crmAlternateKeyTypeHandleForString)
        {
            _crmAlternateKeyTypeHandleForString = crmAlternateKeyTypeHandleForString;
        }
        public ICrmAlternateKeyTypeHandle Create()
        {
            return _crmAlternateKeyTypeHandleForString;
        }
    }
}
