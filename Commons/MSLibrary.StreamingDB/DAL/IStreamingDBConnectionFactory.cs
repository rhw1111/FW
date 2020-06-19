using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.StreamingDB.DAL
{
    /// <summary>
    /// 与流式数据库相关的连接字符串工厂
    /// </summary>
    public interface IStreamingDBConnectionFactory
    {
        /// <summary>
        /// 创建有关与流式数据库的读写连接字符串
        /// </summary>
        /// <returns></returns>
        string CreateAllForStreamingDB();
        /// <summary>
        /// 创建有关与流式数据库的只读连接字符串
        /// </summary>
        /// <returns></returns>
        string CreateReadForStreamingDB();
    }
}
