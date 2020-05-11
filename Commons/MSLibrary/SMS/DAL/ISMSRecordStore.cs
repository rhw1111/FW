using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.SMS.DAL
{
    public interface ISMSRecordStore
    {
        Task Add(SMSRecord record);
        Task UpdateStatus(SMSRecord record, int status);
    }
}
