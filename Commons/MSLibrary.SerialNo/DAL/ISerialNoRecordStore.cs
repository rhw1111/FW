using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MSLibrary.SerialNo.DAL
{
    public interface ISerialNoRecordStore
    {
        Task Add(SerialNoRecord record, CancellationToken cancellationToken = default);
        Task<SerialNoRecord?> QueryFirstNolockByPrefix(string prefix, CancellationToken cancellationToken = default);
        Task<int> UpdateCurrentValue(Guid recordID,int addValue, CancellationToken cancellationToken = default);
        Task Delete(Guid recordID, CancellationToken cancellationToken = default);
        Task<SerialNoRecord> QueryByPrefix(string prefix, CancellationToken cancellationToken = default);
    }
}
