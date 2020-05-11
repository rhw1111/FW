using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.DAL
{
    /// <summary>
    /// StoreInfo解析连接服务
    /// </summary>
    public interface IStoreInfoResolveConnectionService
    {
        /// <summary>
        /// 获取读写连接信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        Task<string> GetReadAndWrite(StoreInfo info);
        /// <summary>
        /// 获取只读连接信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        Task<string> GetRead(StoreInfo info);
        /// <summary>
        /// 获取读写连接信息（同步）
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        string GetReadAndWriteSync(StoreInfo info);
        /// <summary>
        /// 获取只读连接信息（同步）
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        string GetReadSync(StoreInfo info);
    }
}
