using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.FileManagement.DAL;

namespace MSLibrary.FileManagement
{
    [Injection(InterfaceType = typeof(IUploadFileHandleRecordRepository), Scope = InjectionScope.Singleton)]
    public class UploadFileHandleRecordRepository : IUploadFileHandleRecordRepository
    {
        private IUploadFileHandleRecordStore _uploadFileHandleRecordStore;

        public UploadFileHandleRecordRepository(IUploadFileHandleRecordStore uploadFileHandleRecordStore)
        {
            _uploadFileHandleRecordStore = uploadFileHandleRecordStore;
        }
        public async Task<UploadFileHandleRecord> QueryById(Guid id)
        {
            return await _uploadFileHandleRecordStore.QueryId(id);
        }
    }
}
