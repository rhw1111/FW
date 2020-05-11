using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
     
namespace MSLibrary.Transaction.DAL
{
    /// <summary>
    /// 分布式操作数据的数据操作
    /// </summary>
    public interface IDTOperationDataStore
    {
        Task Add(DTOperationData data);
        Task Delete(string storeGroupName, string hashInfo, Guid id);
        Task<bool> UpdateStatus(string storeGroupName, string hashInfo, Guid id,byte[] version, int status);
        Task<DTOperationData> QueryByID(string storeGroupName, string hashInfo, Guid id);
        Task QueryByRecordUniqueName(string storeGroupName, string hashInfo, string recordUniqueName,int status, Func<DTOperationData,Task> action);
    }
}
