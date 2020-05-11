using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.Redis.DAL
{
    /// <summary>
    /// 有关Redis的数据连接字符串工厂
    /// </summary>
    /// </summary>
    public interface IRedisConnectionFactory
    {
        /// <summary>
        /// 创建有关Redis客户端工厂的读写连接字符串
        /// </summary>
        /// <returns></returns>
        string CreateAllForRedisClientFactory();
        /// <summary>
        /// 创建有关Redis客户端工厂的只读连接字符串
        /// </summary>
        /// <returns></returns>
        string CreateReadForRedisClientFactory();
    }
}
