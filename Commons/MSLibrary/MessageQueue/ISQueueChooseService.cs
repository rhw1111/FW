using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.MessageQueue
{
    /// <summary>
    /// 队列选择服务
    /// 负责根据key返回队列
    /// </summary>
    public interface ISQueueChooseService
    {
        Task<SQueue> Choose(string key);
        Task<SQueue> ChooseDead(string key);
    }
}
