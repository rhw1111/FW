using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.MessageQueue.DAL.SMessageStores
{
    [Injection(InterfaceType = typeof(SMessageStoreForSQLDBFactory), Scope = InjectionScope.Singleton)]
    public class SMessageStoreForSQLDBFactory : IFactory<ISMessageStore>
    {
        private SMessageStoreForSQLDB _sMessageStoreForSQLDB;

        public SMessageStoreForSQLDBFactory(SMessageStoreForSQLDB sMessageStoreForSQLDB)
        {
            _sMessageStoreForSQLDB = sMessageStoreForSQLDB;
        }
        public ISMessageStore Create()
        {
            return _sMessageStoreForSQLDB;
        }
    }
}
