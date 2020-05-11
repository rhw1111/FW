using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DAL;

namespace MSLibrary.Transaction.DAL
{
    public interface IDTOperationRecordStore
    {
        Task Add(DTOperationRecord record);
        Task Delete(string storeGroupName,string hashInfo, Guid id);
        Task UpdateStatus(string storeGroupName, string hashInfo, Guid id,int status);

        Task UpdateVersion(string storeGroupName, string hashInfo, Guid id, string version);
        Task UpdateErroeMessage(string storeGroupName, string hashInfo, Guid id, string errorMessage);
        Task<DTOperationRecord> QueryByIDNoLock(string storeGroupName, string hashInfo, Guid id);
        Task<DTOperationRecord> QueryByID(string storeGroupName, string hashInfo, Guid id);
        Task<DTOperationRecord> QueryByUniqueName(string storeGroupName, string hashInfo, string uniqueName);
        Task<List<DTOperationRecord>> QueryBySkip(string storeInfo, int skip,int take);
        Task<List<DTOperationRecord>> QueryBySkip(string storeInfo, int status, int skip, int take);
    }
}
