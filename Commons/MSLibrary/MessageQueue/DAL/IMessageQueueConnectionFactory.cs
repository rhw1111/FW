using MSLibrary.DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.MessageQueue.DAL
{
    /// <summary>
    /// 消息队列连接字符串工厂
    /// </summary>
    public interface IMessageQueueConnectionFactory
    {
        /// <summary>
        /// 创建消息队列的主信息读写连接字符串
        /// </summary>
        /// <returns></returns>
        string CreateAllForMessageQueueMain();
        /// <summary>
        /// 创建消息队列的主信息只读连接字符串 
        /// </summary>
        /// <returns></returns>
        string CreateReadForMessageQueueMain();
        /// <summary>
        /// 根据队列的存储类型和服务器名称创建消息队列的消息读写连接字符串
        /// </summary>
        /// <param name="storeType"></param>
        /// <param name="serverName"></param>
        /// <returns></returns>
        string CreateAllForMessageQueue(int storeType, string serverName);
        /// <summary>
        /// 根据队列的存储类型的服务器名称创建消息队列的消息只读连接字符串
        /// </summary>
        /// <param name="storeType"></param>
        /// <param name="serverName"></param>
        /// <returns></returns>
        string CreateReadForMessageQueue(int storeType, string serverName);
        /// <summary>
        /// 创建消息历史记录的读写连接字符串
        /// </summary>
        /// <returns></returns>
        string CreateAllForSMessageHistory(DBConnectionNames connectionNames);
        /// <summary>
        /// 创建消息历史记录的只读连接字符串
        /// </summary>
        /// <returns></returns>
        string CreateReadForSMessageHistory(DBConnectionNames connectionNames);
        /// <summary>
        /// 创建消息历史监听明细的读写连接字符串
        /// </summary>
        /// <returns></returns>
        string CreateAllForSMessageHistoryListenerDetail(DBConnectionNames connectionNames);
        /// <summary>
        /// 创建消息历史监听明细的只读连接字符串
        /// </summary>
        /// <returns></returns>
        string CreateReadForSMessageHistoryListenerDetail(DBConnectionNames connectionNames);

    }
}
