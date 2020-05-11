using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.Storge.DAL
{
    public interface IStorgeConnectionFactory
    {
        /// <summary>
        /// 创建有关存储的读写连接字符串
        /// </summary>
        /// <returns></returns>
        string CreateAllForStorge();
        /// <summary>
        /// 创建有关存储的只读连接字符串
        /// </summary>
        /// <returns></returns>
        string CreateReadForStorge();
    }
}
