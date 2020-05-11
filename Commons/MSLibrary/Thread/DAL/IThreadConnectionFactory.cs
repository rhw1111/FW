using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DAL;

namespace MSLibrary.Thread.DAL
{
    /// <summary>
    /// 线程处理连接字符串工厂
    /// </summary>
    public interface IThreadConnectionFactory
    {
        /// <summary>
        /// 创建系统锁读写数据连接字符串
        /// </summary>
        /// <param name="lockName"></param>
        /// <returns></returns>
        string CreateAllForApplicationLock(DBConnectionNames connNames, string lockName);
    }
}
