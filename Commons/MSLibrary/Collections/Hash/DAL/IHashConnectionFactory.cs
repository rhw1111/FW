using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.Collections.Hash.DAL
{
    /// <summary>
    /// 一致性哈希连接字符串工厂
    /// </summary>
    public interface IHashConnectionFactory
    {
        /// <summary>
        /// 创建哈希管理的读写连接字符串
        /// </summary>
        /// <returns></returns>
        string CreateAllForHash();
        /// <summary>
        /// 创建哈希管理的只读连接字符串 
        /// </summary>
        /// <returns></returns>
        string CreateReadForHash();
    }
}
