using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.FileManagement.DAL;

namespace MSLibrary.FileManagement
{
    [Injection(InterfaceType = typeof(IUploadFileHandleConfigurationRepository), Scope = InjectionScope.Singleton)]
    public class UploadFileHandleConfigurationRepository : IUploadFileHandleConfigurationRepository
    {
        private IUploadFileHandleConfigurationStore _uploadFileHandleConfigurationStore;
        public UploadFileHandleConfigurationRepository(IUploadFileHandleConfigurationStore uploadFileHandleConfigurationStore)
        {
            _uploadFileHandleConfigurationStore = uploadFileHandleConfigurationStore;
        }
        public async Task<UploadFileHandleConfiguration> QueryById(Guid id)
        {
            return await _uploadFileHandleConfigurationStore.QueryById(id);
        }

        public async Task<UploadFileHandleConfiguration> QueryByName(string name)
        {
            return await _uploadFileHandleConfigurationStore.QueryByName(name);
        }

        public async Task<UploadFileHandleConfiguration> QueryByNameStatus(string name, int status)
        {
            return await _uploadFileHandleConfigurationStore.QueryByNameStatus(name,status);
        }
    }
}
