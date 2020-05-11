using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Xrm.MessageHandle
{
    [Injection(InterfaceType = typeof(CrmMessageHandleForFileAttributeUploadChunkingFactory), Scope = InjectionScope.Singleton)]
    public class CrmMessageHandleForFileAttributeUploadChunkingFactory : IFactory<ICrmMessageHandle>
    {
        private CrmMessageHandleForFileAttributeUploadChunking _crmMessageHandleForFileAttributeUploadChunking;

        public CrmMessageHandleForFileAttributeUploadChunkingFactory(CrmMessageHandleForFileAttributeUploadChunking crmMessageHandleForFileAttributeUploadChunking)
        {
            _crmMessageHandleForFileAttributeUploadChunking = crmMessageHandleForFileAttributeUploadChunking;
        }
        public ICrmMessageHandle Create()
        {
            return _crmMessageHandleForFileAttributeUploadChunking;
        }
    }
}
