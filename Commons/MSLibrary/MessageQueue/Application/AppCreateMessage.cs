using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;

namespace MSLibrary.MessageQueue.Application
{
    [Injection(InterfaceType = typeof(IAppCreateMessage), Scope = InjectionScope.Singleton)]
    public class AppCreateMessage : IAppCreateMessage
    {
        public async Task Do(SMessageData messageData)
        {
            SMessage message = new SMessage()
            {
                Key = messageData.Key,
                Type = messageData.Type,
                Data = messageData.Data,
                 ExpectationExecuteTime=messageData.ExpectationExecuteTime
            };

            await message.Add();
        }
    }
}
