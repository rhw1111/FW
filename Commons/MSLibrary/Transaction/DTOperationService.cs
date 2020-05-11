using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;

namespace MSLibrary.Transaction
{
    [Injection(InterfaceType = typeof(IDTOperationService), Scope = InjectionScope.Singleton)]
    public class DTOperationService : IDTOperationService
    {
        private IDTOperationRecordRepository _dtOperationRecordRepository;
       

        public DTOperationService(IDTOperationRecordRepository dtOperationRecordRepository)
        {
            _dtOperationRecordRepository = dtOperationRecordRepository;
        }
        public async Task Execute(string name, string storeGroupName, string hashInfo, string type,string typeInfo, int timeout, Func<Task> action)
        {
            var record=await _dtOperationRecordRepository.QueryByUniqueName(storeGroupName,hashInfo,name);
            if (record!=null)
            {
                await record.Cancel();
                await record.Delete();
            }

            record = new DTOperationRecord()
            {
                HashInfo = hashInfo,
                Status = (int)DTOperationRecordStatus.UnComplete,
                StoreGroupName = storeGroupName,
                UniqueName = name,
                Timeout = timeout,
                Type = type,
                TypeInfo = typeInfo,
                Version = Guid.NewGuid().ToString()
            };
            await record.Add();

            await record.Execute(action);

            await record.Delete();
        }
    }
}
