using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.Security.WhitelistPolicy.DAL
{
    /// <summary>
    /// 客户端白名单策略的数据连接字符串工厂
    /// </summary>
    public interface IClientWhitelistPolicyConnectionFactory
    {
        /// <summary>
        /// 创建客户端白名单策略的读写连接字符串
        /// </summary>
        /// <returns></returns>
        string CreateAllForClientWhitelistPolicy();
        /// <summary>
        /// 创建客户端白名单策略的只读连接字符串
        /// </summary>
        /// <returns></returns>
        string CreateReadForClientWhitelistPolicy();
    }
}
