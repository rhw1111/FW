using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.FileManagement
{
    /// <summary>
    /// 上传文件处理配置仓储
    /// </summary>
    public interface IUploadFileHandleConfigurationRepository
    {
        /// <summary>
        /// 根据Id查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<UploadFileHandleConfiguration> QueryById(Guid id);

        /// <summary>
        /// 根据名称查询
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<UploadFileHandleConfiguration> QueryByName(string name);
        /// <summary>
        /// 根据名称和状态查询
        /// </summary>
        /// <param name="name"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        Task<UploadFileHandleConfiguration> QueryByNameStatus(string name,int status);
    }
}
