using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.Security.Jwt.DAL
{
    /// <summary>
    /// Jwt的数据连接字符串工厂
    /// </summary>
    public interface IJwtConnectionFactory
    {
        /// <summary>
        /// 创建Jwt的读写连接字符串
        /// </summary>
        /// <returns></returns>
        string CreateAllForJwt();
        /// <summary>
        /// 创建Jwt的只读连接字符串
        /// </summary>
        /// <returns></returns>
        string CreateReadForJwt();
    }
}
