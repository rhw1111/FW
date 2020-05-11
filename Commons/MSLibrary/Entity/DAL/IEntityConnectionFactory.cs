using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.Entity.DAL
{
    public interface IEntityConnectionFactory
    {
        /// <summary>
        /// 创建实体读写连接字符串
        /// </summary>
        /// <returns></returns>
        string CreateAllForEntity();
        /// <summary>
        /// 创建实体只读连接字符串 
        /// </summary>
        /// <returns></returns>
        string CreateReadForEntity();
    }
}
