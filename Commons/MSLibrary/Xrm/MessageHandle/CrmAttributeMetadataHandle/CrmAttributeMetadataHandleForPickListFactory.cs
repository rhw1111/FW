using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Xrm.MessageHandle.CrmAttributeMetadataHandle
{
    [Injection(InterfaceType = typeof(CrmAttributeMetadataHandleForPickListFactory), Scope = InjectionScope.Singleton)]
    public class CrmAttributeMetadataHandleForPickListFactory : IFactory<ICrmAttributeMetadataHandle>
    {
        private CrmAttributeMetadataHandleForPickList _crmAttributeMetadataHandleForPickList;

        public CrmAttributeMetadataHandleForPickListFactory(CrmAttributeMetadataHandleForPickList crmAttributeMetadataHandleForPickList)
        {
            _crmAttributeMetadataHandleForPickList = crmAttributeMetadataHandleForPickList;
        }
        public ICrmAttributeMetadataHandle Create()
        {
            return _crmAttributeMetadataHandleForPickList;
        }
    }
}
