using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Transaction
{
    public interface IDTOperationRecordRepository
    {
        Task<DTOperationRecord> QueryByUniqueName(string storeGroupName,string hashInfo,string uniqueName);

        Task<List<DTOperationRecord>> QueryBySkip(string storeInfo, int skip, int take);
        Task<List<DTOperationRecord>> QueryBySkip(string storeInfo,int status, int skip, int take);
    }
}
