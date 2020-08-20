using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;
using MSLibrary.MessageQueue.DAL;

namespace MSLibrary.MySqlStore.MessageQueue.DAL
{
    [Injection(InterfaceType = typeof(SMessageStoreForMySqlFactory), Scope = InjectionScope.Singleton)]
    public class SMessageStoreForMySqlFactory : IFactory<ISMessageStore>
    {
        private SMessageStoreForMySql _sMessageStoreForMySql;

        public ISMessageStore Create()
        {
            return _sMessageStoreForMySql;
        }
    }
}
