using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.Entity.DAL;
using MSLibrary.FileManagement.DAL;

namespace MSLibrary.FileManagement
{
    [Injection(InterfaceType = typeof(IUploadFileRepository), Scope = InjectionScope.Singleton)]
    public class UploadFileRepository : IUploadFileRepository
    {
        private IUploadFileStore _uploadFileStore;

        public UploadFileRepository(IUploadFileStore uploadFileStore)
        {
            _uploadFileStore = uploadFileStore;
        }
        public async Task<UploadFile> QueryByUniqueName(string regardingType, string regardingKey, int status, string uniqueName)
        {
            return await _uploadFileStore.QueryByUniqueName(regardingType, regardingKey, status, uniqueName);
        }

        public async Task<UploadFile> QueryById(string regardingType, string regardingKey, Guid id)
        {
            return await _uploadFileStore.QueryById(regardingType, regardingKey, id);
        }

        public async Task QueryByRegarding(string regardingType, string regardingKey, int status, Func<UploadFile, Task> callback)
        {
            await _uploadFileStore.QueryByRegarding(regardingType, regardingKey, status, callback);
        }

        public async Task<QueryResult<UploadFile>> QueryByRegarding(string regardingType, string regardingKey, int status, int page, int pageSize)
        {
            return await _uploadFileStore.QueryByRegarding(regardingType, regardingKey, status, page, pageSize);
        }

        public async Task<List<UploadFile>> QueryTop(int status, int top)
        {
            return await _uploadFileStore.QueryTop(status, top);
        }

        public async Task<List<UploadFile>> QueryTopBefore(int status, int top, DateTime dateTime)
        {
            return await _uploadFileStore.QueryTopBefore(status, top, dateTime);
        }
    }
}
