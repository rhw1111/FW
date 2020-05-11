using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Transaction
{
    public interface IDTOperationRecordService
    {
        Task Cancel(string typeInfo,string uniqueName);
    }
}
