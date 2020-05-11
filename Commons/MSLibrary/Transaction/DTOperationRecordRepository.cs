using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.Transaction.DAL;

namespace MSLibrary.Transaction
{
    [Injection(InterfaceType = typeof(IDTOperationRecordRepository), Scope = InjectionScope.Singleton)]
    public class DTOperationRecordRepository : IDTOperationRecordRepository
    {
        private IDTOperationRecordStore _dtOperationRecordStore;

        public DTOperationRecordRepository(IDTOperationRecordStore dtOperationRecordStore)
        {
            _dtOperationRecordStore = dtOperationRecordStore;
        }
        public async Task<List<DTOperationRecord>> QueryBySkip(string storeInfo, int skip, int take)
        {
            return await _dtOperationRecordStore.QueryBySkip(storeInfo, skip, take);
        }

        public async Task<List<DTOperationRecord>> QueryBySkip(string storeInfo, int status, int skip, int take)
        {
            return await _dtOperationRecordStore.QueryBySkip(storeInfo, status, skip, take);
        }

        public async Task<DTOperationRecord> QueryByUniqueName(string storeGroupName, string hashInfo, string uniqueName)
        {
            return await _dtOperationRecordStore.QueryByUniqueName(storeGroupName, hashInfo, uniqueName);
        }
    }
}
