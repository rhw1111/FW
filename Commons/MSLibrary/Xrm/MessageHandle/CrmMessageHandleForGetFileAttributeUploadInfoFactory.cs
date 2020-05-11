using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Xrm.MessageHandle
{
    [Injection(InterfaceType = typeof(CrmMessageHandleForGetFileAttributeUploadInfoFactory), Scope = InjectionScope.Singleton)]
    public class CrmMessageHandleForGetFileAttributeUploadInfoFactory : IFactory<ICrmMessageHandle>
    {
        private CrmMessageHandleForGetFileAttributeUploadInfo _crmMessageHandleForGetFileAttributeUploadInfo;

        public CrmMessageHandleForGetFileAttributeUploadInfoFactory(CrmMessageHandleForGetFileAttributeUploadInfo crmMessageHandleForGetFileAttributeUploadInfo)
        {
            _crmMessageHandleForGetFileAttributeUploadInfo = crmMessageHandleForGetFileAttributeUploadInfo;
        }
        public ICrmMessageHandle Create()
        {
            return _crmMessageHandleForGetFileAttributeUploadInfo;
        }
    }
}
