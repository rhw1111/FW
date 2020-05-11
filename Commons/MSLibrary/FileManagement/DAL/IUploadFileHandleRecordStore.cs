using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.FileManagement.DAL
{
    public interface IUploadFileHandleRecordStore
    {
        Task Add(UploadFileHandleRecord record);
        Task Delete(Guid recordId);
        Task UpdateStatus(Guid recordId,int status,string result,string error);
        Task<UploadFileHandleRecord> QueryId(Guid recordId);
    }
}
