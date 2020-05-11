using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Transaction.DAL
{
    public interface IDTOperationDataCancelLogStore
    {
        Task Add(DTOperationDataCancelLog log);
        Task DeleteByDataID(Guid dataID);
        Task<DTOperationDataCancelLog> QueryByDataID(Guid dataID);
    }
}
