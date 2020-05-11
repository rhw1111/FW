using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.Cache.DAL
{
    /// <summary>
    /// 缓存连接字符串工厂
    /// </summary>
    public interface ICacheConnectionFactory
    {
        /// <summary>
        /// 创建缓存读写连接字符串
        /// </summary>
        /// <returns></returns>
        string CreateAllForCache();
        /// <summary>
        /// 创建缓存只读连接字符串 
        /// </summary>
        /// <returns></returns>
        string CreateReadForCache();
    }
}
