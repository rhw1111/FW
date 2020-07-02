using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;

namespace MSLibrary.MessageQueue
{
    /// <summary>
    /// 监听消息的Key生成服务
    /// 在该实现中，key的格式为监听的队列组名称-初始消息Key，队列组名称为SQueue中的GroupName
    /// </summary>
    [Injection(InterfaceType = typeof(IListenerMessageKeyGenerateService), Scope = InjectionScope.Singleton)]
    public class ListenerMessageKeyGenerateService : IListenerMessageKeyGenerateService
    {
        public async Task<string> Generate(string originalMessageKey, SMessageTypeListener listener)
        {
            return await Task.FromResult($"{listener.QueueGroupName}-{originalMessageKey}");
        }
    }
}
