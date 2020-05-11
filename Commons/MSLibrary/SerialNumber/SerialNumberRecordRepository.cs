using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.SerialNumber.DAL;

namespace MSLibrary.SerialNumber
{
    [Injection(InterfaceType = typeof(ISerialNumberRecordRepository), Scope = InjectionScope.Singleton)]
    public class SerialNumberRecordRepository : ISerialNumberRecordRepository
    {
        private ISerialNumberRecordStore _serialNumberRecordStore;

        public SerialNumberRecordRepository(ISerialNumberRecordStore serialNumberRecordStore)
        {
            _serialNumberRecordStore = serialNumberRecordStore;
        }
        public async Task<SerialNumberRecord> QueryByPrefix(string prefix)
        {
            return await _serialNumberRecordStore.QueryByPrefix(prefix);
        }
    }
}
