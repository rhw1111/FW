using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Xrm.MessageHandle
{
    [Injection(InterfaceType = typeof(CrmMessageHandleForFileAttributeDownloadChunkingFactory), Scope = InjectionScope.Singleton)]
    public class CrmMessageHandleForFileAttributeDownloadChunkingFactory : IFactory<ICrmMessageHandle>
    {
        private CrmMessageHandleForFileAttributeDownloadChunking _crmMessageHandleForFileAttributeDownloadChunking;

        public CrmMessageHandleForFileAttributeDownloadChunkingFactory(CrmMessageHandleForFileAttributeDownloadChunking crmMessageHandleForFileAttributeDownloadChunking)
        {
            _crmMessageHandleForFileAttributeDownloadChunking = crmMessageHandleForFileAttributeDownloadChunking;
        }
        public ICrmMessageHandle Create()
        {
            return _crmMessageHandleForFileAttributeDownloadChunking;
        }
    }
}
