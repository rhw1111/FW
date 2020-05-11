using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.FileManagement.UploadFileSourceExecuteServices
{
    class UploadFileSourceExecuteServiceForAliOSS : IUploadFileSourceExecuteService
    {
        public Task Delete(UploadFile uploadFile)
        {
            throw new NotImplementedException();
        }

        public Task<ValidateResult<Stream>> Read(UploadFile uploadFile, long start, long? end)
        {
            throw new NotImplementedException();
        }

        public Task Read(UploadFile uploadFile, Func<Stream, Task> action)
        {
            throw new NotImplementedException();
        }

        public Task Write(UploadFile uploadFile, Stream stream)
        {
            throw new NotImplementedException();
        }
    }
}
