using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.MessageQueue
{
    /// <summary>
    /// 监听消息的Key生成服务
    /// </summary>
    public interface IListenerMessageKeyGenerateService
    {
        /// <summary>
        /// 生成Key
        /// </summary>
        /// <param name="originalMessageKey">初始消息的Key</param>
        /// <param name="listener">监听消息所属的监听</param>
        /// <returns></returns>
        Task<string> Generate(string originalMessageKey, SMessageTypeListener listener);
    }
}
