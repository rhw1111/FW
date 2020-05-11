using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.MessageQueue.Application
{
    /// <summary>
    /// 应用层执行队列执行组
    /// </summary>
    public interface IAppExecuteQueueProcessGroup
    {
        /// <summary>
        /// 执行指定组名称的执行组
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        Task Do(string groupName);
    }
}
