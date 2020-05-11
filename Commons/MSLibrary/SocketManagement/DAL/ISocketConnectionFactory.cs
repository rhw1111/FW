using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.SocketManagement.DAL
{
    /// <summary>
    /// Socket连接字符串工厂
    /// </summary>
    public interface ISocketConnectionFactory
    {
        /// <summary>
        /// 创建Socket读写连接字符串
        /// </summary>
        /// <returns></returns>
        string CreateAllForSocket();
        /// <summary>
        /// 创建Socket只读连接字符串 
        /// </summary>
        /// <returns></returns>
        string CreateReadForSocket();

        /// <summary>
        /// 创建针对监听名称的读写连接字符串
        /// </summary>
        /// <param name="listenerName">监听名称</param>
        /// <returns></returns>
        string CreateAllForTcpLog(string listenerName);
        /// <summary>
        /// 创建针对监听名称的只读连接字符串 
        /// </summary>
        /// <param name="listenerName">监听名称</param>
        /// <returns></returns>
        string CreateReadForTcpLog(string listenerName);
    }
}
