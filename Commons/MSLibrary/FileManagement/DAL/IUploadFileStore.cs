using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.FileManagement.DAL
{
    /// <summary>
    /// 上传文件数据操作
    /// </summary>
    public interface IUploadFileStore
    {
        /// <summary>
        /// 新增上传文件
        /// </summary>
        /// <param name="uploadFile"></param>
        /// <returns></returns>
        Task Add(UploadFile uploadFile);
        /// <summary>
        /// 修改上传文件状态
        /// </summary>
        /// <param name="uploadFile"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        Task UpdateStatus(UploadFile uploadFile, int status);
        /// <summary>
        /// 删除上传文件
        /// </summary>
        /// <param name="regardingType"></param>
        /// <param name="regardingKey"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        Task Delete(string regardingType, string regardingKey, Guid id);

        /// <summary>
        /// 根据id查询上传文件
        /// </summary>
        /// <param name="regardingType"></param>
        /// <param name="regardingKey"></param>
        /// <param name="id">上传文件id</param>
        /// <returns>上传文件</returns>
        Task<UploadFile> QueryById(string regardingType, string regardingKey, Guid id);

        /// <summary>
        /// 根据id查询上传文件(同步方法)
        /// </summary>
        /// <param name="regardingType"></param>
        /// <param name="regardingKey"></param>
        /// <param name="id">上传文件id</param>
        /// <returns>上传文件</returns>
        UploadFile QueryByIdSync(string regardingType, string regardingKey, Guid id);

        /// <summary>
        /// 根据状态和唯一名称查询
        /// </summary>
        /// <param name="regardingType"></param>
        /// <param name="regardingKey"></param>
        /// <param name="status"></param>
        /// <param name="uniqueName"></param>
        /// <returns></returns>
        Task<UploadFile> QueryByUniqueName(string regardingType, string regardingKey, int status, string uniqueName);

        Task QueryByRegarding(string regardingType, string regardingKey, int status, Func<UploadFile, Task> callback);

        /// <summary>
        /// 根据关联信息分页查询上传文件
        /// </summary>
        /// <param name="regardingType">关联类型</param>
        /// <param name="regardingKey">关联关键信息</param>
        /// <param name="status">上传文件状态</param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<QueryResult<UploadFile>> QueryByRegarding(string regardingType, string regardingKey, int status, int page, int pageSize);
        /// <summary>
        /// 根据状态查询指定数量的上传文件
        /// </summary>
        /// <param name="status">上传文件状态</param>
        /// <param name="top">数量</param>
        /// <returns></returns>
        Task<List<UploadFile>> QueryTop(int status, int top);

        /// <summary>
        /// 根据状态查询指定数量和指定创建时间之前的上传文件
        /// </summary>
        /// <param name="status">上传文件状态</param>
        /// <param name="top">数量</param>
        /// <param name="dateTime">指定创建时间</param>
        /// <returns></returns>
        Task<List<UploadFile>> QueryTopBefore(int status, int top, DateTime dateTime);

        /// <summary>
        /// 修改上传文件可修改的属性
        /// 目前只能修改DisplayName
        /// </summary>
        /// <param name="uploadFile"></param>
        /// <returns></returns>
        Task Update(UploadFile uploadFile);
    }
}