using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.FileManagement
{
    /// <summary>
    /// 上传文件处理仓储
    /// </summary>
    public interface IUploadFileHandleRecordRepository
    {
        /// <summary>
        /// 根据Id查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<UploadFileHandleRecord> QueryById(Guid id);
    }
}
