using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.CommonQueue.DAL
{
    /// <summary>
    /// 通用队列连接字符串工厂
    /// </summary>
    public interface ICommonQueueConnectionFactory
    {
        string CreateAllForCommonQueue();
        string CreateReadForCommonQueue();
    }
}
