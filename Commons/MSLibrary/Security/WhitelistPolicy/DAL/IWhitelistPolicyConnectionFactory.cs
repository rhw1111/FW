using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DAL;

namespace MSLibrary.Security.WhitelistPolicy.DAL
{
    /// <summary>
    /// 白名单策略的数据连接字符串工厂
    /// </summary>
    public interface IWhitelistPolicyConnectionFactory : IDBConnectionFactory
    {
        /// <summary>
        /// 创建白名单策略的读写连接字符串
        /// </summary>
        /// <returns></returns>
        string CreateAllForWhitelistPolicy();
        /// <summary>
        /// 创建白名单策略的只读连接字符串
        /// </summary>
        /// <returns></returns>
        string CreateReadForWhitelistPolicy();

    }
}
