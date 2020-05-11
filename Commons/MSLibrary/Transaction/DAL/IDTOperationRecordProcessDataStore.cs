using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Transaction.DAL
{
    public interface IDTOperationRecordProcessDataStore
    {
        Task Add(DTOperationRecordProcessData data);
        Task Delete(string recordUniqueName);

        Task<DTOperationRecordProcessData> QueryByRecordUniqueNameNoLock(string recordUniqueName);
    }
}
