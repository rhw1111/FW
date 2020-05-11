using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace MSLibrary.DAL
{
    /// <summary>
    /// 存储信息解析服务
    /// 负责将一个含有存储信息的字符串解析为格式化数据
    /// </summary>
    public interface IStoreInfoResolveService
    {
        /// <summary>
        /// 解析成结果
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        Task<StoreInfo> Execute(string info);
        /// <summary>
        /// 解析结果（同步）
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        StoreInfo ExecuteSync(string info);

    }

}
