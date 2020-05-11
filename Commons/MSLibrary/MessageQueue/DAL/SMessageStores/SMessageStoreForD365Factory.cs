using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.MessageQueue.DAL.SMessageStores
{
    [Injection(InterfaceType = typeof(SMessageStoreForD365Factory), Scope = InjectionScope.Singleton)]
    public class SMessageStoreForD365Factory : IFactory<ISMessageStore>
    {
        private SMessageStoreForD365 _sMessageStoreForD365;

        public SMessageStoreForD365Factory(SMessageStoreForD365 sMessageStoreForD365)
        {
            _sMessageStoreForD365 = sMessageStoreForD365;
        }
        public ISMessageStore Create()
        {
            return _sMessageStoreForD365;
        }
    }
}
