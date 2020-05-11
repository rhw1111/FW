using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace MSLibrary.MessageQueue.Application
{
    /// <summary>
    /// 应用层创建消息
    /// </summary>
    public interface IAppCreateMessage
    {
        Task Do(SMessageData messageData);
    }
}
