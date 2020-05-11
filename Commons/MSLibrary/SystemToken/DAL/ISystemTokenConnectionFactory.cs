using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DAL;

namespace MSLibrary.SystemToken.DAL
{
    /// <summary>
    /// 系统令牌连接字符串工厂
    /// </summary>
    public interface ISystemTokenConnectionFactory
    {
        /// <summary>
        /// 创建系统令牌数据读写连接字符串
        /// </summary>
        /// <returns></returns>
        string CreateAllForSystemToken();
        /// <summary>
        /// 创建系统令牌只读数据连接字符串
        /// </summary>
        /// <returns></returns>
        string CreateReadForSystemToken();
        /// <summary>
        /// 创建第三方令牌数据的读写连接字符串
        /// </summary>
        /// <param name="connectionNames"></param>
        /// <returns></returns>
        string CreateAllForThirdPartySystemToken(DBConnectionNames connectionNames);
        /// <summary>
        /// 创建第三方令牌数据的只读连接字符串
        /// </summary>
        /// <param name="connectionNames"></param>
        /// <returns></returns>
        string CreateReadForThirdPartySystemToken(DBConnectionNames connectionNames);
    }
}
