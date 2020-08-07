using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MSLibrary.SerialNo
{
    public interface ISerialNoRecordRepository
    {
        Task<SerialNoRecord?> QueryByPrefix(string prefix, CancellationToken cancellationToken = default);
    }
}
