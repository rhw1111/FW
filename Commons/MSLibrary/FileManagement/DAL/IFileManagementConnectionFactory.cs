using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DAL;

namespace MSLibrary.FileManagement.DAL
{
    /// <summary>
    /// 文件管理数据连接工厂
    /// </summary>
    public interface IFileManagementConnectionFactory
    {
        /// <summary>
        /// 创建上传文件的读写连接字符串
        /// </summary>
        /// <returns></returns>
        string CreateAllForUploadFile(DBConnectionNames connectionNames);
        /// <summary>
        /// 创建上传文件的只读连接字符串
        /// </summary>
        /// <returns></returns>
        string CreateReadForUploadFile(DBConnectionNames connectionNames);

        /// <summary>
        /// 创建上传文件处理记录的读写连接字符串
        /// </summary>
        /// <returns></returns>
        string CreateAllForUploadFileHandle();

        /// <summary>
        /// 创建上传文件处理记录的只读连接字符串
        /// </summary>
        /// <returns></returns>
        string CreateReadForUploadFileHandle();
        
    }
}
