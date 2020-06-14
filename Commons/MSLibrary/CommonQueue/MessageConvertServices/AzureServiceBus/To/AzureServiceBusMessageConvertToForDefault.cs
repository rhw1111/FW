using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using MSLibrary.DI;
using MSLibrary.Serializer;

namespace MSLibrary.CommonQueue.MessageConvertServices.AzureServiceBus.To
{
    [Injection(InterfaceType = typeof(AzureServiceBusMessageConvertToForDefault), Scope = InjectionScope.Singleton)]
    public class AzureServiceBusMessageConvertToForDefault : IAzureServiceBusMessageConvertTo
    {
        public async Task<Message> To(CommonMessage commonMessage)
        {
            Message message = new Message(UTF8Encoding.UTF8.GetBytes(JsonSerializerHelper.Serializer(commonMessage)));
            if (commonMessage.ExpectationExecuteTime.HasValue)
            {
                message.ScheduledEnqueueTimeUtc = commonMessage.ExpectationExecuteTime.Value;
            }
            
            return await Task.FromResult(message);
        }
    }
}
