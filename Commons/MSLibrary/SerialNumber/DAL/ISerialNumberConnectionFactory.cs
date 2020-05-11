using MSLibrary.DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.SerialNumber.DAL
{
    public interface ISerialNumberConnectionFactory
    {
        /// <summary>
        /// 创建序列号读写连接字符串
        /// </summary>
        /// <returns></returns>
        string CreateAllForSerialNumber();
        /// <summary>
        /// 创建序列号只读连接字符串 
        /// </summary>
        /// <returns></returns>
        string CreateReadForSerialNumber();
        /// <summary>
        /// 根据连接名称组创建序列号记录的读写连接字符串
        /// </summary>
        /// <param name="serverInfo"></param>
        /// <returns></returns>
        string CreateAllForSerialNumberRecord(DBConnectionNames connectionNames);
        /// <summary>
        /// 根据连接名称组创建序列号记录的只读连接字符串
        /// </summary>
        /// <param name="serverInfo"></param>
        /// <returns></returns>
        string CreateReadForSerialNumberRecord(DBConnectionNames connectionNames);

    }
}
